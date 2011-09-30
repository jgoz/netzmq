namespace ZeroMQ.Sockets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ZeroMQ.Proxy;

    /// <summary>
    /// Represents a ZeroMQ context object.
    /// </summary>
    /// <remarks>
    /// The <see cref="ZmqContext"/> object is a container for all sockets in a single process,
    /// and acts as the transport for inproc sockets. <see cref="ZmqContext"/> is thread safe.
    /// </remarks>
    public sealed class ZmqContext : IZmqContext
    {
        private const int DefaultThreadPoolSize = 1;

        private readonly IProxyFactory proxyFactory;
        private readonly IContextProxy proxy;

        private static readonly Lazy<IProxyFactory> Factory = new Lazy<IProxyFactory>(ProxyFactory.Create);

        private static Encoding defaultEncoding = Encoding.UTF8;

        private bool disposed;

        internal ZmqContext(IProxyFactory proxyFactory, IContextProxy proxy)
        {
            if (proxyFactory == null)
            {
                throw new ArgumentNullException("proxyFactory");
            }

            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }

            this.proxyFactory = proxyFactory;
            this.proxy = proxy;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ZmqContext"/> class.
        /// </summary>
        ~ZmqContext()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets or sets the default encoding for all sockets in the current process
        /// </summary>
        public static Encoding DefaultEncoding
        {
            get { return defaultEncoding; }
            set { defaultEncoding = value; }
        }

        /// <summary>
        /// Create a concrete <see cref="IZmqContext"/> instance.
        /// </summary>
        /// <returns>An <see cref="IZmqContext"/> instance with the default thread pool size (1).</returns>
        public static IZmqContext Create()
        {
            return Create(DefaultThreadPoolSize);
        }

        /// <summary>
        /// Create a concrete <see cref="IZmqContext"/> instance.
        /// </summary>
        /// <param name="threadPoolSize">Number of threads to use in the ZMQ thread pool.</param>
        /// <returns>An <see cref="IZmqContext"/> instance with the specified thread pool size.</returns>
        public static IZmqContext Create(int threadPoolSize)
        {
            if (threadPoolSize < 0)
            {
                throw new ArgumentOutOfRangeException("threadPoolSize", threadPoolSize, "Thread pool size must be non-negative.");
            }

            try
            {
                return new ZmqContext(Factory.Value, Factory.Value.CreateContext(threadPoolSize));
            }
            catch (ProxyException ex)
            {
                throw new ZmqLibException(ex);
            }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="CreatePairSocket"]/*'/>
        public IDuplexSocket CreatePairSocket()
        {
            return this.TryCreateSocket((p, e) => new DuplexSocket(p, e), SocketType.Pair);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="CreatePublishExtSocket"]/*'/>
        public IDuplexSocket CreatePublishExtSocket()
        {
            return this.TryCreateSocket((p, e) => new DuplexSocket(p, e), SocketType.Xpub);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="CreatePublishSocket"]/*'/>
        public ISendSocket CreatePublishSocket()
        {
            return this.TryCreateSocket((p, e) => new SendSocket(p, e), SocketType.Pub);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="CreatePullSocket"]/*'/>
        public IReceiveSocket CreatePullSocket()
        {
            return this.TryCreateSocket((p, e) => new ReceiveSocket(p, e), SocketType.Pull);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="CreatePushSocket"]/*'/>
        public ISendSocket CreatePushSocket()
        {
            return this.TryCreateSocket((p, e) => new SendSocket(p, e), SocketType.Push);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="CreateReplyExtSocket"]/*'/>
        public IDuplexSocket CreateReplyExtSocket()
        {
            return this.TryCreateSocket((p, e) => new DuplexSocket(p, e), SocketType.Xrep);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="CreateReplySocket"]/*'/>
        public IDuplexSocket CreateReplySocket()
        {
            return this.TryCreateSocket((p, e) => new DuplexSocket(p, e), SocketType.Rep);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="CreateRequestExtSocket"]/*'/>
        public IDuplexSocket CreateRequestExtSocket()
        {
            return this.TryCreateSocket((p, e) => new DuplexSocket(p, e), SocketType.Xreq);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="CreateRequestSocket"]/*'/>
        public IDuplexSocket CreateRequestSocket()
        {
            return this.TryCreateSocket((p, e) => new DuplexSocket(p, e), SocketType.Req);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="CreateSubscribeExtSocket"]/*'/>
        public ISubscribeExtSocket CreateSubscribeExtSocket()
        {
            return this.TryCreateSocket((p, e) => new SubscribeExtSocket(p, e), SocketType.Xsub);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="CreateSubscribeSocket"]/*'/>
        public ISubscribeSocket CreateSubscribeSocket()
        {
            return this.TryCreateSocket((p, e) => new SubscribeSocket(p, e), SocketType.Sub);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="CreatePollSet"]/*'/>
        public IPollSet CreatePollSet(IEnumerable<ISocket> sockets)
        {
            if (sockets == null)
            {
                throw new ArgumentNullException("sockets");
            }

            if (!sockets.Any())
            {
                throw new ArgumentException("At least one socket is needed for a poll set", "sockets");
            }

            if (!sockets.All(s => typeof(ZmqSocket).IsAssignableFrom(s.GetType())))
            {
                throw new ArgumentException("All sockets used for polling must inherit from ZmqSocket.", "sockets");
            }

            try
            {
                IPollItem[] pollItems = sockets.Select(s => new PollItem((ZmqSocket)s)).ToArray();

                return new ZmqPollSet(this.proxyFactory.CreatePollSet(pollItems.Length), pollItems, this.proxyFactory.ErrorProvider);
            }
            catch (ProxyException ex)
            {
                throw new ZmqLibException(ex);
            }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Terminate"]/*'/>
        public void Terminate()
        {
            this.EnsureNotDisposed();

            this.proxy.Terminate();
        }

        /// <summary>
        /// Frees the underlying ZeroMQ context handle.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !this.disposed)
            {
                this.Terminate();
            }

            this.disposed = true;
        }

        private TSocket TryCreateSocket<TSocket>(Func<ISocketProxy, IErrorProviderProxy, TSocket> constructor, SocketType socketType)
        {
            this.EnsureNotDisposed();

            try
            {
                return constructor(this.proxyFactory.CreateSocket(this.proxy.CreateSocket((int)socketType)), this.proxyFactory.ErrorProvider);
            }
            catch (ProxyException ex)
            {
                throw new ZmqLibException(ex);
            }
        }

        private void EnsureNotDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("ZmqContext", "The current ZmqContext has already been terminated and cannot be reused.");
            }
        }
    }
}
