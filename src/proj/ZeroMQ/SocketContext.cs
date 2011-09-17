namespace ZeroMQ
{
    using System;

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

            try
            {
                this.Context = new Proxy.SocketContext(threadPoolSize);
            }
            catch (Proxy.ZmqException ex)
            {
                throw new ZmqLibException(ex);
            }

            this.ThreadPoolSize = threadPoolSize;
        }

        ~SocketContext()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the size of the thread pool for this context. Default is 1.
        /// </summary>
        public int ThreadPoolSize { get; private set; }

        /// <summary>
        /// Gets the underlying socket context handle.
        /// </summary>
        public Proxy.SocketContext Context { get; private set; }

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

            this.Context.Dispose();

            this.disposed = true;
        }
    }
}
