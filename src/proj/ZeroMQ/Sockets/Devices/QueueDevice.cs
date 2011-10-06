namespace ZeroMQ.Sockets.Devices
{
    /// <summary>
    /// A shared queue that collects requests from a set of clients and distributes
    /// these fairly among a set of services.
    /// </summary>
    /// <remarks>
    /// Requests are fair-queued from frontend connections and load-balanced between
    /// backend connections. Replies automatically return to the client that made the
    /// original request. This device is part of the request-reply pattern. The frontend
    /// speaks to clients and the backend speaks to services.
    /// </remarks>
    public static class QueueDevice
    {
        /// <summary>
        /// Creates a new <see cref="ZmqDevice{TFrontend,TBackend}"/> using the specified context that will
        /// run in the current thread.
        /// </summary>
        /// <param name="context">An <see cref="IZmqContext"/> for creating the frontend and backend sockets.</param>
        /// <returns>A thread-safe <see cref="ZmqDevice{TFrontend,TBackend}"/> object implementing the Queue pattern.</returns>
        public static ZmqDevice<IDuplexSocket, IDuplexSocket> Create(IZmqContext context)
        {
            return ZmqDevice<IDuplexSocket, IDuplexSocket>.Create(context.CreateReplyExtSocket(), context.CreateRequestExtSocket());
        }

        /// <summary>
        /// Creates a new <see cref="ThreadDevice{TFrontend,TBackend}"/> using the specified context that will
        /// run in its own thread.
        /// </summary>
        /// <param name="context">An <see cref="IZmqContext"/> for creating the frontend and backend sockets.</param>
        /// <returns>A thread-safe <see cref="ThreadDevice{TFrontend,TBackend}"/> object implementing the Queue pattern.</returns>
        public static ThreadDevice<IDuplexSocket, IDuplexSocket> CreateThreaded(IZmqContext context)
        {
            return ThreadDevice<IDuplexSocket, IDuplexSocket>.Create(context.CreateReplyExtSocket(), context.CreateRequestExtSocket());
        }
    }
}
