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

        private readonly IContextProxy proxy;

        private static Encoding defaultEncoding = Encoding.UTF8;

        private bool disposed;

        internal ZmqContext(IContextProxy proxy)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }

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
        /// Gets the underlying socket context handle.
        /// </summary>
        internal IntPtr Handle
        {
            get { return this.proxy.Handle; }
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
                return new ZmqContext(ProxyFactory.CreateContext(threadPoolSize));
            }
            catch (ProxyException ex)
            {
                throw new ZmqLibException(ex);
            }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="CreatePairSocket"]/*'/>
        public IDuplexSocket CreatePairSocket()
        {
            return this.TryCreateSocket(p => new DuplexSocket(p), SocketType.Pair);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="CreatePublishExtSocket"]/*'/>
        public IDuplexSocket CreatePublishExtSocket()
        {
            return this.TryCreateSocket(p => new DuplexSocket(p), SocketType.Xpub);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="CreatePublishSocket"]/*'/>
        public ISendSocket CreatePublishSocket()
        {
            return this.TryCreateSocket(p => new SendSocket(p), SocketType.Pub);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="CreatePullSocket"]/*'/>
        public IReceiveSocket CreatePullSocket()
        {
            return this.TryCreateSocket(p => new ReceiveSocket(p), SocketType.Pull);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="CreatePushSocket"]/*'/>
        public ISendSocket CreatePushSocket()
        {
            return this.TryCreateSocket(p => new SendSocket(p), SocketType.Push);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="CreateReplyExtSocket"]/*'/>
        public IDuplexSocket CreateReplyExtSocket()
        {
            return this.TryCreateSocket(p => new DuplexSocket(p), SocketType.Xrep);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="CreateReplySocket"]/*'/>
        public IDuplexSocket CreateReplySocket()
        {
            return this.TryCreateSocket(p => new DuplexSocket(p), SocketType.Rep);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="CreateRequestExtSocket"]/*'/>
        public IDuplexSocket CreateRequestExtSocket()
        {
            return this.TryCreateSocket(p => new DuplexSocket(p), SocketType.Xreq);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="CreateRequestSocket"]/*'/>
        public IDuplexSocket CreateRequestSocket()
        {
            return this.TryCreateSocket(p => new DuplexSocket(p), SocketType.Req);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="CreateSubscribeExtSocket"]/*'/>
        public ISubscribeExtSocket CreateSubscribeExtSocket()
        {
            return this.TryCreateSocket(p => new SubscribeExtSocket(p), SocketType.Xsub);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="CreateSubscribeSocket"]/*'/>
        public ISubscribeSocket CreateSubscribeSocket()
        {
            return this.TryCreateSocket(p => new SubscribeSocket(p), SocketType.Sub);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="CreatePollSet"]/*'/>
        public IPollSet CreatePollSet(IEnumerable<ISocket> sockets)
        {
            if (sockets == null)
            {
                throw new ArgumentNullException("sockets");
            }

            if (!sockets.All(s => typeof(ZmqSocket).IsAssignableFrom(s.GetType())))
            {
                throw new ArgumentException("All sockets used for polling must inherit from ZmqSocket.", "sockets");
            }

            try
            {
                IPollItem[] pollItems = sockets.Select(s => new PollItem((ZmqSocket)s)).ToArray();

                return new ZmqPollSet(ProxyFactory.CreatePollSet(pollItems.Length), pollItems);
            }
            catch (ProxyException ex)
            {
                throw new ZmqLibException(ex);
            }
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

        private TSocket TryCreateSocket<TSocket>(Func<ISocketProxy, TSocket> constructor, SocketType socketType)
        {
            try
            {
                return constructor(ProxyFactory.CreateSocket(this.proxy.Handle, (int)socketType));
            }
            catch (ProxyException ex)
            {
                throw new ZmqLibException(ex);
            }
        }
    }
}
