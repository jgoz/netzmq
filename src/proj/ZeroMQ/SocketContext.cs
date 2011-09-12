namespace ZeroMQ
{
    using System;

    using ZeroMQ.Interop;

    /// <summary>
    /// Represents a thread-safe ZeroMQ context object.
    /// </summary>
    /// <remarks>
    /// The <see cref="SocketContext"/> object is a container for all sockets in a single process,
    /// and acts as the transport for inproc sockets. <see cref="SocketContext"/> is thread safe.
    /// </remarks>
    public class SocketContext : ISocketContext
    {
        private const int DefaultThreadPoolSize = 1;

        private readonly int threadPoolSize;

        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketContext"/> class. Uses the default thread pool size.
        /// </summary>
        public SocketContext() : this(DefaultThreadPoolSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketContext"/> class.
        /// </summary>
        /// <param name="threadPoolSize">Number of threads to use in the ZMQ thread pool.</param>
        /// <remarks>
        /// The size of the thread pool should be at least 1. If all sockets in this context use the
        /// inproc transport, then the thread pool size may be 0.
        /// </remarks>
        public SocketContext(int threadPoolSize)
        {
            if (threadPoolSize < 0)
            {
                throw new ArgumentOutOfRangeException("threadPoolSize", threadPoolSize, "Thread pool size must be non-negative.");
            }

            this.threadPoolSize = threadPoolSize;
            this.Handle = LibZmq.Init(threadPoolSize);

            if (this.Handle == IntPtr.Zero)
            {
                throw ZmqLibException.GetLastError();
            }
        }

        ~SocketContext()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the underlying ZeroMQ context object.
        /// </summary>
        public IntPtr Handle { get; private set; }

        /// <summary>
        /// Gets the size of the thread pool for this context. Default is 1.
        /// </summary>
        public int ThreadPoolSize
        {
            get { return this.threadPoolSize; }
        }

        /// <summary>
        /// Frees the underlying ZeroMQ context handle.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Frees the underlying ZeroMQ context handle.
        /// </summary>
        /// <param name="disposing">True if the object is being disposed or false if it is being finalized.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (this.Handle != IntPtr.Zero)
            {
                this.TerminateContext();
                this.Handle = IntPtr.Zero;
            }

            this.disposed = true;
        }

        private void TerminateContext()
        {
            while (LibZmq.Term(this.Handle) != 0)
            {
                int errno = LibZmq.Errno();

                // If zmq_term fails, valid return codes are EFault or EIntr. If EIntr is set, termination
                // was interrupted by a signal and may be safely retried.
                if (errno == (int)SystemError.EFault)
                {
                    throw new ZmqLibException(errno, LibZmq.StrError(errno));
                }
            }
        }
    }
}
