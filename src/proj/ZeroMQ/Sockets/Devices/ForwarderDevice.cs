namespace ZeroMQ.Sockets.Devices
{
    /// <summary>
    /// Collects messages from a set of publishers and forwards these to a set of subscribers.
    /// </summary>
    /// <remarks>
    /// Generally used to bridge networks. E.g. read on TCP unicast and forward on multicast.
    /// This device is part of the publish-subscribe pattern. The frontend speaks to publishers
    /// and the backend speaks to subscribers.
    /// </remarks>
    public static class ForwarderDevice
    {
        /// <summary>
        /// Creates a new <see cref="ZmqDevice{TFrontendSocket,TBackendSocket}"/> using the
        /// specified context that will run in the current thread.
        /// </summary>
        /// <param name="context">An <see cref="IZmqContext"/> for creating the frontend and backend sockets.</param>
        /// <returns>A thread-safe <see cref="ZmqDevice{TFrontendSocket,TBackendSocket}"/> object implementing the Forwarder pattern.</returns>
        public static ZmqDevice<ISubscribeSocket, ISendSocket> Create(IZmqContext context)
        {
            return ZmqDevice<ISubscribeSocket, ISendSocket>.Create(context.CreateSubscribeSocket(), context.CreatePublishSocket());
        }

        /// <summary>
        /// Creates a new <see cref="ThreadDevice{TFrontend,TBackend}"/> using the specified context that will
        /// run in its own thread.
        /// </summary>
        /// <param name="context">An <see cref="IZmqContext"/> for creating the frontend and backend sockets.</param>
        /// <returns>A thread-safe <see cref="ThreadDevice{TFrontend,TBackend}"/> object implementing the Forwarder pattern.</returns>
        public static ThreadDevice<ISubscribeSocket, ISendSocket> CreateThreaded(IZmqContext context)
        {
            return ThreadDevice<ISubscribeSocket, ISendSocket>.Create(context.CreateSubscribeSocket(), context.CreatePublishSocket());
        }
    }
}
