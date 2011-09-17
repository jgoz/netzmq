namespace ZeroMQ
{
    using ZeroMQ.Options;

    /// <summary>
    /// ZMQ_PUSH socket. Used by a pipeline node to send messages to downstream pipeline nodes.
    /// </summary>
    public class PullSocket : Socket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PullSocket"/> class.
        /// </summary>
        /// <param name="context"><see cref="ISocketContext"/> to use when initializing the socket.</param>
        public PullSocket(ISocketContext context)
            : base(context, SocketType.Pull)
        {
        }

        /// <summary>
        /// Create an endpoint for accepting connections and bind it to the current socket.
        /// </summary>
        /// <param name="endpoint">
        /// A string consisting of a <em>transport</em> and an <em>address</em>, formatted as
        /// <c><em>transport</em>://<em>address</em></c>.
        /// </param>
        /// <exception cref="ZmqLibException">An error occured during the execution of a native procedure.</exception>
        public new void Bind(string endpoint)
        {
            base.Bind(endpoint);
        }

        /// <summary>
        /// Connect the current socket to the specified endpoint.
        /// </summary>
        /// <param name="endpoint">
        /// A string consisting of a <em>transport</em> and an <em>address</em>, formatted as
        /// <c><em>transport</em>://<em>address</em></c>.
        /// </param>
        /// <exception cref="ZmqLibException">An error occured during the execution of a native procedure.</exception>
        public new void Connect(string endpoint)
        {
            base.Connect(endpoint);
        }

        /// <summary>
        /// Receive a message from a remote socket.
        /// </summary>
        /// <param name="socketFlags">A bitwise combination of <see cref="SocketFlags"/> values.</param>
        /// <returns>A <see cref="ReceivedMessage"/> object containing the data recieved and the operation outcome.</returns>
        /// <exception cref="ZmqLibException">An error occured during the execution of a native procedure.</exception>
        public new ReceivedMessage Receive(SocketFlags socketFlags)
        {
            return base.Receive(socketFlags);
        }
    }
}
