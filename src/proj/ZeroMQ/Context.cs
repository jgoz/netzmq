namespace ZeroMQ
{
    using System;

    using ZeroMQ.Interop;

    /// <summary>
    /// Represents a ZeroMQ context object.
    /// </summary>
    /// <remarks>
    /// The <see cref="Context"/> object is a container for all sockets in a single process,
    /// and acts as the transport for inproc sockets. <see cref="Context"/> is thread safe.
    /// </remarks>
    public class Context : IContext
    {
        private const int DefaultThreadPoolSize = 1;

        private readonly int threadPoolSize;

        private IntPtr context;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Context"/> class. Uses the default thread pool size.
        /// </summary>
        public Context() : this(DefaultThreadPoolSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Context"/> class.
        /// </summary>
        /// <param name="threadPoolSize">Number of threads to use in the ZMQ thread pool.</param>
        /// <remarks>
        /// The size of the thread pool should be at least 1. If all sockets in this context use the
        /// inproc transport, then the thread pool size may be 0.
        /// </remarks>
        public Context(int threadPoolSize)
        {
            if (threadPoolSize < 0)
            {
                throw new ArgumentOutOfRangeException("threadPoolSize", threadPoolSize, "Thread pool size must be non-negative.");
            }

            this.threadPoolSize = threadPoolSize;
            this.context = LibZmq.ZmqInit(threadPoolSize);

            if (this.context == IntPtr.Zero)
            {
                throw ZmqLibException.GetLastError();
            }
        }

        ~Context()
        {
            Dispose(false);
        }

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

            if (this.context != IntPtr.Zero)
            {
                this.TerminateContext();
                this.context = IntPtr.Zero;
            }

            this.disposed = true;
        }

        private void TerminateContext()
        {
            while (LibZmq.ZmqTerm(this.context) != 0)
            {
                int errno = LibZmq.ZmqErrno();

                // If zmq_term fails, valid return codes are EFault or EIntr. If EIntr is set, termination
                // was interrupted by a signal and may be safely retried.
                if (errno == (int)SystemError.EFault)
                {
                    throw new ZmqLibException(errno, ZmqLibException.GetErrorMessage(errno));
                }
            }
        }
    }
}
