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
        /// Creates a new <see cref="IQueueDevice"/> that will run in the current thread.
        /// </summary>
        /// <param name="context">An <see cref="IZmqContext"/> for creating the frontend and backend sockets.</param>
        /// <returns>A thread-safe <see cref="IQueueDevice"/> object implementing the Queue pattern.</returns>
        public static IQueueDevice Create(IZmqContext context)
        {
            return ZmqDevice<IDuplexSocket, IDuplexSocket>.Create(context.CreateReplyExtSocket(), context.CreateRequestExtSocket(), (f, b) => new Queue(f, b));
        }

        /// <summary>
        /// Creates a new <see cref="IQueueDevice"/> that will run in a separate thread.
        /// </summary>
        /// <param name="context">An <see cref="IZmqContext"/> for creating the frontend and backend sockets.</param>
        /// <returns>A thread-safe <see cref="IQueueDevice"/> object implementing the Queue pattern.</returns>
        public static IQueueDevice CreateThreaded(IZmqContext context)
        {
            return ThreadDevice<IDuplexSocket, IDuplexSocket>.Create(context.CreateReplyExtSocket(), context.CreateRequestExtSocket(), (f, b) => new ThreadQueue(f, b));
        }

        internal class Queue : ZmqDevice<IDuplexSocket, IDuplexSocket>, IQueueDevice
        {
            internal Queue(IDuplexSocket frontend, IDuplexSocket backend)
                : base(frontend, backend)
            {
            }
        }

        internal class ThreadQueue : ThreadDevice<IDuplexSocket, IDuplexSocket>, IQueueDevice
        {
            internal ThreadQueue(IDuplexSocket frontend, IDuplexSocket backend)
                : base(frontend, backend)
            {
            }
        }
    }
}
