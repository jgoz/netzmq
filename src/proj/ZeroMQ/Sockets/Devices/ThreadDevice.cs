namespace ZeroMQ.Sockets.Devices
{
    using System;
    using System.Threading;

    using ZeroMQ.Proxy;

    /// <summary>
    /// A <see cref="ZmqDevice{TFrontend,TBackend}"/> that runs in a self-managed <see cref="Thread"/>.
    /// </summary>
    /// <typeparam name="TFrontend">The frontend socket type.</typeparam>
    /// <typeparam name="TBackend">The backend socket type.</typeparam>
    /// <remarks>
    /// <para>
    /// The base implementation of <see cref="ThreadDevice{TFrontend,TBackend}"/>
    /// is <b>not</b> threadsafe. It is possible to construct a device with sockets that were
    /// created in separate threads or separate contexts.
    /// </para>
    /// <para>
    /// For this reason, the preferred way to create devices is use a factory method to construct the
    /// <see cref="ThreadDevice{TFrontend,TBackend}"/> for a given <see cref="IZmqContext"/>.
    /// </para>
    /// </remarks>
    public class ThreadDevice<TFrontend, TBackend> : ZmqDevice<TFrontend, TBackend>
        where TFrontend : class, ISocket
        where TBackend : class, ISocket
    {
        private readonly Thread runThread;

        internal ThreadDevice(TFrontend frontend, TBackend backend, IDeviceProxy device, IErrorProviderProxy errorProvider)
            : base(frontend, backend, device, errorProvider)
        {
            this.runThread = new Thread(this.Run);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadDevice{TFrontend,TBackend}"/> class.
        /// </summary>
        /// <param name="frontend">
        /// A <see cref="ZmqSocket"/> that will pass incoming messages to <paramref name="backend"/>.
        /// </param>
        /// <param name="backend">
        /// A <see cref="ZmqSocket"/> that will receive messages from (and optionally send replies
        /// to) <paramref name="frontend"/>.
        /// </param>
        /// <returns>A new <see cref="ThreadDevice{TFrontend,TBackend}"/> object for the specified sockets.</returns>
        /// <remarks>
        /// To avoid potential thread safety issues, <paramref name="frontend"/> and <paramref name="backend"/>
        /// must be created with the same <see cref="ZmqContext"/>.
        /// </remarks>
        public static new ThreadDevice<TFrontend, TBackend> Create(TFrontend frontend, TBackend backend)
        {
            ValidateSockets(frontend, backend);

            var deviceProxy = CreateDeviceProxy(frontend, backend);

            return new ThreadDevice<TFrontend, TBackend>(frontend, backend, deviceProxy, ZmqContext.ProxyFactory.ErrorProvider);
        }

        /// <summary>
        /// Start the device in a new thread and return execution to the calling thread.
        /// </summary>
        public override void Start()
        {
            this.runThread.Start();
        }

        /// <summary>
        /// Blocks the calling thread until the device thread terminates.
        /// </summary>
        public override void Join()
        {
            this.runThread.Join();
        }

        /// <summary>
        /// Blocks the calling thread until the device thread terminates or the specified time elapses.
        /// </summary>
        /// <param name="timeout">
        /// A <see cref="TimeSpan"/> set to the amount of time to wait for the device to terminate.
        /// </param>
        /// <returns>
        /// true if the device terminated; false if the device has not terminated after
        /// the amount of time specified by <paramref name="timeout"/> has elapsed.
        /// </returns>
        public override bool Join(TimeSpan timeout)
        {
            return this.runThread.Join(timeout);
        }
    }
}
