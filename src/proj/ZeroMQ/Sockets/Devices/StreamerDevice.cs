namespace ZeroMQ.Sockets.Devices
{
    /// <summary>
    /// Collects tasks from a set of pushers and forwards these to a set of pullers.
    /// </summary>
    /// <remarks>
    /// Generally used to bridge networks. Messages are fair-queued from pushers and
    /// load-balanced to pullers. This device is part of the pipeline pattern. The
    /// frontend speaks to pushers and the backend speaks to pullers.
    /// </remarks>
    public static class StreamerDevice
    {
        /// <summary>
        /// Creates a new <see cref="IStreamerDevice"/> that will run in the current thread.
        /// </summary>
        /// <param name="context">An <see cref="IZmqContext"/> for creating the frontend and backend sockets.</param>
        /// <returns>A thread-safe <see cref="IStreamerDevice"/> object implementing the Streamer pattern.</returns>
        public static IDevice<IReceiveSocket, ISendSocket> Create(IZmqContext context)
        {
            return ZmqDevice<IReceiveSocket, ISendSocket>.Create(context.CreatePullSocket(), context.CreatePushSocket(), (f, b) => new Streamer(f, b));
        }

        /// <summary>
        /// Creates a new <see cref="IStreamerDevice"/> that will run in a separate thread.
        /// </summary>
        /// <param name="context">An <see cref="IZmqContext"/> for creating the frontend and backend sockets.</param>
        /// <returns>A thread-safe <see cref="IStreamerDevice"/> object implementing the Streamer pattern.</returns>
        public static IDevice<IReceiveSocket, ISendSocket> CreateThreaded(IZmqContext context)
        {
            return ThreadDevice<IReceiveSocket, ISendSocket>.Create(context.CreatePullSocket(), context.CreatePushSocket(), (f, b) => new ThreadStreamer(f, b));
        }

        internal class Streamer : ZmqDevice<IReceiveSocket, ISendSocket>, IStreamerDevice
        {
            internal Streamer(IReceiveSocket frontend, ISendSocket backend)
                : base(frontend, backend)
            {
            }
        }

        internal class ThreadStreamer : ThreadDevice<IReceiveSocket, ISendSocket>, IStreamerDevice
        {
            internal ThreadStreamer(IReceiveSocket frontend, ISendSocket backend)
                : base(frontend, backend)
            {
            }
        }
    }
}
