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
        /// Creates a new <see cref="ZmqDevice"/> using the specified context that will
        /// run in the current thread.
        /// </summary>
        /// <param name="context">An <see cref="IZmqContext"/> for creating the frontend and backend sockets.</param>
        /// <returns>A thread-safe <see cref="ZmqDevice"/> object implementing the Streamer pattern.</returns>
        public static ZmqDevice Create(IZmqContext context)
        {
            return new ZmqDevice((ZmqSocket)context.CreatePullSocket(), (ZmqSocket)context.CreatePushSocket());
        }

        /// <summary>
        /// Creates a new <see cref="ThreadDevice"/> using the specified context that will
        /// run in its own thread.
        /// </summary>
        /// <param name="context">An <see cref="IZmqContext"/> for creating the frontend and backend sockets.</param>
        /// <returns>A thread-safe <see cref="ThreadDevice"/> object implementing the Streamer pattern.</returns>
        public static ThreadDevice CreateThreaded(IZmqContext context)
        {
            return new ThreadDevice((ZmqSocket)context.CreatePullSocket(), (ZmqSocket)context.CreatePushSocket());
        }
    }
}
