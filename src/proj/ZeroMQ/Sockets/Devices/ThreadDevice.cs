namespace ZeroMQ.Sockets.Devices
{
    using System;
    using System.Threading;

    /// <summary>
    /// A <see cref="ZmqDevice"/> that runs in a self-managed <see cref="Thread"/>.
    /// </summary>
    public class ThreadDevice : ZmqDevice
    {
        private readonly Thread runThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadDevice"/> class.
        /// </summary>
        /// <param name="frontend">
        /// A <see cref="ZmqSocket"/> that will pass incoming messages to <paramref name="backend"/>.
        /// </param>
        /// <param name="backend">
        /// A <see cref="ZmqSocket"/> that will receive messages from (and optionally send replies
        /// to) <paramref name="frontend"/>.
        /// </param>
        protected internal ThreadDevice(ZmqSocket frontend, ZmqSocket backend)
            : base(frontend, backend)
        {
            this.runThread = new Thread(this.Run);
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
