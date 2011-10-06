namespace ZeroMQ.Sockets.Devices
{
    /// <summary>
    /// Defines extensions to <see cref="DeviceSocketSetup{TSocket}"/> objects for concrete socket types.
    /// </summary>
    public static class DeviceSocketSetupExtensions
    {
        /// <summary>
        /// Configure the socket to subscribe to a specific prefix. See <see cref="SubscribeSocket.Subscribe"/> for details.
        /// </summary>
        /// <param name="setup">The <see cref="DeviceSocketSetup{TSocket}"/> object.</param>
        /// <param name="prefix">A byte array containing the prefix to which the socket will subscribe.</param>
        /// <returns>The current <see cref="DeviceSocketSetup{TSocket}"/> object.</returns>
        public static DeviceSocketSetup<ISubscribeSocket> SubscribeTo(this DeviceSocketSetup<ISubscribeSocket> setup, byte[] prefix)
        {
            return setup.AddSocketInitializer(s => s.Subscribe(prefix));
        }

        /// <summary>
        /// Configure the socket to subscribe to all incoming messages. See <see cref="SubscribeSocket.SubscribeAll"/> for details.
        /// </summary>
        /// <param name="setup">The <see cref="DeviceSocketSetup{TSocket}"/> object.</param>
        /// <returns>The current <see cref="DeviceSocketSetup{TSocket}"/> object.</returns>
        public static DeviceSocketSetup<ISubscribeSocket> SubscribeToAll(this DeviceSocketSetup<ISubscribeSocket> setup)
        {
            return setup.AddSocketInitializer(s => s.SubscribeAll());
        }
    }
}
