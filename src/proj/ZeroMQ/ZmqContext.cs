namespace ZeroMQ
{
    using System;
    using System.Text;

    using ZeroMQ.Proxy;

    /// <summary>
    /// Represents a thread-safe ZeroMQ context object.
    /// </summary>
    /// <remarks>
    /// The <see cref="ZmqContext"/> object is a container for all sockets in a single process,
    /// and acts as the transport for inproc sockets. <see cref="ZmqContext"/> is thread safe.
    /// </remarks>
    public class ZmqContext : IZmqContext
    {
        private const int DefaultThreadPoolSize = 1;

        private readonly IContextProxy proxy;

        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZmqContext"/> class. Uses the default thread pool size.
        /// </summary>
        public ZmqContext() : this(DefaultThreadPoolSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZmqContext"/> class.
        /// </summary>
        /// <param name="threadPoolSize">Number of threads to use in the ZMQ thread pool.</param>
        /// <exception cref="ZmqLibException">An error occured while initializing the underlying context.</exception>
        /// <remarks>
        /// The size of the thread pool should be at least 1. If all sockets in this context use the
        /// inproc transport, then the thread pool size may be 0.
        /// </remarks>
        public ZmqContext(int threadPoolSize)
        {
            if (threadPoolSize < 0)
            {
                throw new ArgumentOutOfRangeException("threadPoolSize", threadPoolSize, "Thread pool size must be non-negative.");
            }

            try
            {
                this.proxy = ProxyFactory.CreateContext(threadPoolSize);
            }
            catch (ProxyException ex)
            {
                throw new ZmqLibException(ex);
            }

            this.ThreadPoolSize = threadPoolSize;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ZmqContext"/> class.
        /// </summary>
        ~ZmqContext()
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
        internal IntPtr Handle
        {
            get { return this.proxy.Handle; }
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

            if (disposing)
            {
                this.proxy.Dispose();
            }

            this.disposed = true;
        }

        private static Encoding defaultEncoding = Encoding.UTF8;

        /// <summary>
        /// Gets or sets the default encoding for all sockets in the current process
        /// </summary>
        public static Encoding DefaultEncoding
        {
            get { return defaultEncoding; }
            set { defaultEncoding = value; }
        }
    }
}
