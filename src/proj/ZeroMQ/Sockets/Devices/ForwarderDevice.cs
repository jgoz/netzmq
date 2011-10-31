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
        /// Creates a new <see cref="IForwarderDevice"/> that will run in the current thread.
        /// </summary>
        /// <param name="context">An <see cref="IZmqContext"/> for creating the frontend and backend sockets.</param>
        /// <returns>A thread-safe <see cref="IForwarderDevice"/> object implementing the Forwarder pattern.</returns>
        public static IForwarderDevice Create(IZmqContext context)
        {
            return ZmqDevice<ISubscribeSocket, ISendSocket>.Create(context.CreateSubscribeSocket(), context.CreatePublishSocket(), (f, b) => new Forwarder(f, b));
        }

        /// <summary>
        /// Creates a new <see cref="IForwarderDevice"/> that will run in a separate thread.
        /// </summary>
        /// <param name="context">An <see cref="IZmqContext"/> for creating the frontend and backend sockets.</param>
        /// <returns>A thread-safe <see cref="IForwarderDevice"/> object implementing the Forwarder pattern.</returns>
        public static IForwarderDevice CreateThreaded(IZmqContext context)
        {
            return ThreadDevice<ISubscribeSocket, ISendSocket>.Create(context.CreateSubscribeSocket(), context.CreatePublishSocket(), (f, b) => new ThreadForwarder(f, b));
        }

        internal class Forwarder : ZmqDevice<ISubscribeSocket, ISendSocket>, IForwarderDevice
        {
            internal Forwarder(ISubscribeSocket frontend, ISendSocket backend)
                : base(frontend, backend)
            {
            }
        }

        internal class ThreadForwarder : ThreadDevice<ISubscribeSocket, ISendSocket>, IForwarderDevice
        {
            internal ThreadForwarder(ISubscribeSocket frontend, ISendSocket backend)
                : base(frontend, backend)
            {
            }
        }
    }
}
