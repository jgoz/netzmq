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
        /// Creates a new <see cref="ZmqDevice"/> using the specified context that will
        /// run in the current thread.
        /// </summary>
        /// <param name="context">An <see cref="IZmqContext"/> for creating the frontend and backend sockets.</param>
        /// <returns>A thread-safe <see cref="ZmqDevice"/> object implementing the Forwarder pattern.</returns>
        public static ZmqDevice Create(IZmqContext context)
        {
            return new ZmqDevice((ZmqSocket)context.CreateSubscribeSocket(), (ZmqSocket)context.CreatePublishSocket());
        }

        /// <summary>
        /// Creates a new <see cref="ThreadDevice"/> using the specified context that will
        /// run in its own thread.
        /// </summary>
        /// <param name="context">An <see cref="IZmqContext"/> for creating the frontend and backend sockets.</param>
        /// <returns>A thread-safe <see cref="ThreadDevice"/> object implementing the Forwarder pattern.</returns>
        public static ThreadDevice CreateThreaded(IZmqContext context)
        {
            return new ThreadDevice((ZmqSocket)context.CreateSubscribeSocket(), (ZmqSocket)context.CreatePublishSocket());
        }
    }
}
