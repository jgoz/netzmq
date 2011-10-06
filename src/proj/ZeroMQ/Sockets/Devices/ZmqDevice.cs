namespace ZeroMQ.Sockets.Devices
{
    using System;
    using System.Threading;

    using ZeroMQ.Proxy;

    /// <summary>
    /// Forwards messages received by a front-end socket to a back-end socket, from which
    /// they are then sent.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The base implementation of <see cref="ZmqDevice"/> is <b>not</b> threadsafe. It is
    /// possible to construct a device with sockets that were created in separate threads or
    /// separate contexts.
    /// </para>
    /// <para>
    /// For this reason, the preferred way to create devices is to inherit from <see cref="ZmqDevice"/>
    /// and create the frontend and backend sockets from within the subclass using a single context.
    /// </para>
    /// </remarks>
    public abstract class ZmqDevice
    {
        private readonly ISocket frontend;
        private readonly ISocket backend;
        private readonly DeviceSocketSetup frontendSetup;
        private readonly DeviceSocketSetup backendSetup;

        private readonly IDeviceProxy device;
        private readonly ZmqErrorProvider errorProvider;

        private readonly ManualResetEvent runningEvent;

        internal ZmqDevice(IDeviceProxy device, IErrorProviderProxy errorProvider)
        {
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

            this.frontend = new ZmqSocket(device.Frontend, errorProvider);
            this.frontendSetup = new DeviceSocketSetup(this.frontend);

            this.backend = new ZmqSocket(device.Backend, errorProvider);
            this.backendSetup = new DeviceSocketSetup(this.backend);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZmqDevice"/> class.
        /// </summary>
        /// <param name="frontend">
        /// A <see cref="ZmqSocket"/> that will pass incoming messages to <paramref name="backend"/>.
        /// </param>
        /// <param name="backend">
        /// A <see cref="ZmqSocket"/> that will receive messages from (and optionally send replies
        /// to) <paramref name="frontend"/>.
        /// </param>
        /// <remarks>
        /// To avoid potential thread safety issues, <paramref name="frontend"/> and <paramref name="backend"/>
        /// must be created with the same <see cref="ZmqContext"/>.
        /// </remarks>
        protected ZmqDevice(ZmqSocket frontend, ZmqSocket backend)
        {
            if (frontend == null)
            {
                throw new ArgumentNullException("frontend");
            }

            if (backend == null)
            {
                throw new ArgumentNullException("backend");
            }

            this.frontend = frontend;
            this.frontendSetup = new DeviceSocketSetup(this.frontend);
            this.backend = backend;
            this.backendSetup = new DeviceSocketSetup(this.backend);

            this.device = ZmqContext.ProxyFactory.CreateDevice(frontend.Handle, backend.Handle);
            this.errorProvider = new ZmqErrorProvider(ZmqContext.ProxyFactory.ErrorProvider);
            this.runningEvent = new ManualResetEvent(true);
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
        /// Configure the frontend socket using a fluent interface.
        /// </summary>
        /// <returns>A <see cref="DeviceSocketSetup"/> object used to define socket configuration options.</returns>
        public DeviceSocketSetup ConfigureFrontend()
        {
            return this.frontendSetup;
        }

        /// <summary>
        /// Configure the backend socket using a fluent interface.
        /// </summary>
        /// <returns>A <see cref="DeviceSocketSetup"/> object used to define socket configuration options.</returns>
        public DeviceSocketSetup ConfigureBackend()
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
        /// Start the device in the current thread. Should be used by implementations of
        /// the <see cref="Start"/> method.
        /// </summary>
        protected void Run()
        {
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
    }
}
