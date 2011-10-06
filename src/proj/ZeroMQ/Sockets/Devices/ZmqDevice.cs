namespace ZeroMQ.Sockets.Devices
{
    using System;
    using System.Threading;

    using ZeroMQ.Proxy;

    /// <summary>
    /// Forwards messages received by a front-end socket to a back-end socket, from which
    /// they are then sent.
    /// </summary>
    /// <typeparam name="TFrontend">The frontend socket type.</typeparam>
    /// <typeparam name="TBackend">The backend socket type.</typeparam>
    /// <remarks>
    /// <para>
    /// The base implementation of <see cref="ZmqDevice{TFrontend,TBackend}"/>
    /// is <b>not</b> threadsafe. It is possible to construct a device with sockets that were
    /// created in separate threads or separate contexts.
    /// </para>
    /// <para>
    /// For this reason, the preferred way to create devices is use a factory method to construct the
    /// <see cref="ZmqDevice{TFrontend,TBackend}"/> for a given <see cref="IZmqContext"/>.
    /// </para>
    /// </remarks>
    public class ZmqDevice<TFrontend, TBackend> : IDisposable
        where TFrontend : class, ISocket
        where TBackend : class, ISocket
    {
        private const int PollingIntervalMsec = 500;

        private readonly TFrontend frontend;
        private readonly TBackend backend;
        private readonly DeviceSocketSetup<TFrontend> frontendSetup;
        private readonly DeviceSocketSetup<TBackend> backendSetup;

        private readonly IDeviceProxy device;
        private readonly ZmqErrorProvider errorProvider;

        private readonly ManualResetEvent runningEvent;

        private bool disposed;

        internal ZmqDevice(TFrontend frontend, TBackend backend, IDeviceProxy device, IErrorProviderProxy errorProvider)
        {
            if (frontend == null)
            {
                throw new ArgumentNullException("frontend");
            }

            if (backend == null)
            {
                throw new ArgumentNullException("backend");
            }

            if (device == null)
            {
                throw new ArgumentNullException("device");
            }

            if (errorProvider == null)
            {
                throw new ArgumentNullException("errorProvider");
            }

            this.device = device;
            this.errorProvider = new ZmqErrorProvider(errorProvider);
            this.runningEvent = new ManualResetEvent(true);

            this.frontend = frontend;
            this.frontendSetup = new DeviceSocketSetup<TFrontend>(this.frontend);

            this.backend = backend;
            this.backendSetup = new DeviceSocketSetup<TBackend>(this.backend);
        }

        ~ZmqDevice()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets a value indicating whether the device loop is running;
        /// </summary>
        public bool IsRunning
        {
            get { return this.device.IsRunning; }
            private set { this.device.IsRunning = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZmqDevice{TFrontend,TBackend}"/> class.
        /// </summary>
        /// <param name="frontend">
        /// A <see cref="ZmqSocket"/> that will pass incoming messages to <paramref name="backend"/>.
        /// </param>
        /// <param name="backend">
        /// A <see cref="ZmqSocket"/> that will receive messages from (and optionally send replies
        /// to) <paramref name="frontend"/>.
        /// </param>
        /// <returns>A new <see cref="ZmqDevice{TFrontend,TBackend}"/> object for the specified sockets.</returns>
        /// <remarks>
        /// To avoid potential thread safety issues, <paramref name="frontend"/> and <paramref name="backend"/>
        /// must be created with the same <see cref="ZmqContext"/>.
        /// </remarks>
        public static ZmqDevice<TFrontend, TBackend> Create(TFrontend frontend, TBackend backend)
        {
            ValidateSockets(frontend, backend);

            var deviceProxy = CreateDeviceProxy(frontend, backend);

            return new ZmqDevice<TFrontend, TBackend>(frontend, backend, deviceProxy, ZmqContext.ProxyFactory.ErrorProvider);
        }

        /// <summary>
        /// Configure the frontend socket using a fluent interface.
        /// </summary>
        /// <returns>A <see cref="DeviceSocketSetup{TSocket}"/> object used to define socket configuration options.</returns>
        public DeviceSocketSetup<TFrontend> ConfigureFrontend()
        {
            return this.frontendSetup;
        }

        /// <summary>
        /// Configure the backend socket using a fluent interface.
        /// </summary>
        /// <returns>A <see cref="DeviceSocketSetup{TSocket}"/> object used to define socket configuration options.</returns>
        public DeviceSocketSetup<TBackend> ConfigureBackend()
        {
            return this.backendSetup;
        }

        /// <summary>
        /// Start the device in the current thread.
        /// </summary>
        public virtual void Start()
        {
            this.Run();
        }

        /// <summary>
        /// Blocks the calling thread until the device terminates.
        /// </summary>
        public virtual void Join()
        {
            this.runningEvent.WaitOne();
        }

        /// <summary>
        /// Blocks the calling thread until the device terminates or the specified time elapses.
        /// </summary>
        /// <param name="timeout">
        /// A <see cref="TimeSpan"/> set to the amount of time to wait for the device to terminate.
        /// </param>
        /// <returns>
        /// true if the device terminated; false if the device has not terminated after
        /// the amount of time specified by <paramref name="timeout"/> has elapsed.
        /// </returns>
        public virtual bool Join(TimeSpan timeout)
        {
            return this.runningEvent.WaitOne(timeout);
        }

        /// <summary>
        /// Terminate the device safely.
        /// </summary>
        public virtual void Stop()
        {
            this.IsRunning = false;
        }

        /// <summary>
        /// Releases all resources used by the current instance, including the frontend and backend sockets.
        /// </summary>
        public virtual void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal static void ValidateSockets(TFrontend frontend, TBackend backend)
        {
            if (frontend == null)
            {
                throw new ArgumentNullException("frontend");
            }

            if (!(frontend is ZmqSocket))
            {
                throw new ArgumentException("Device sockets must inherit from ZmqSocket.", "frontend");
            }

            if (backend == null)
            {
                throw new ArgumentNullException("backend");
            }

            if (!(backend is ZmqSocket))
            {
                throw new ArgumentException("Device sockets must inherit from ZmqSocket.", "backend");
            }
        }

        internal static IDeviceProxy CreateDeviceProxy(TFrontend frontend, TBackend backend)
        {
            return ZmqContext.ProxyFactory.CreateDevice(((ZmqSocket)(ISocket)frontend).Handle, ((ZmqSocket)(ISocket)backend).Handle);
        }

        /// <summary>
        /// Start the device in the current thread. Should be used by implementations of
        /// the <see cref="Start"/> method.
        /// </summary>
        protected void Run()
        {
            this.EnsureNotDisposed();

            this.frontendSetup.Configure();
            this.backendSetup.Configure();

            this.runningEvent.Reset();
            this.IsRunning = true;

            int errorCode = this.device.Run();

            this.IsRunning = false;
            this.runningEvent.Set();

            if (errorCode == -1 && !this.errorProvider.ContextWasTerminated)
            {
                throw this.errorProvider.GetLastError();
            }
        }

        /// <summary>
        /// Stops the device and releases the underlying sockets. Optionally disposes of managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (this.IsRunning)
            {
                this.Stop();
                this.Join(TimeSpan.FromMilliseconds(PollingIntervalMsec * 2));
            }

            if (disposing)
            {
                this.frontend.Dispose();
                this.backend.Dispose();
            }

            this.disposed = true;
        }

        private void EnsureNotDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("ZmqSocket", "The current ZmqSocket has already been disposed and cannot be reused.");
            }
        }
    }
}
