namespace ZeroMQ
{
    using ZeroMQ.Options;

    /// <summary>
    /// ZMQ_REP socket. Receive requests from and sends replies to a client.
    /// </summary>
    public class RepSocket : Socket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepSocket"/> class.
        /// </summary>
        /// <param name="context"><see cref="ISocketContext"/> to use when initializing the socket.</param>
        public RepSocket(ISocketContext context)
            : base(context, SocketType.Rep)
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
        /// Receive a message from a remote socket.
        /// </summary>
        /// <param name="socketFlags">A bitwise combination of <see cref="SocketFlags"/> values.</param>
        /// <returns>A <see cref="ReceivedMessage"/> object containing the data recieved and the operation outcome.</returns>
        /// <exception cref="ZmqLibException">An error occured during the execution of a native procedure.</exception>
        public new ReceivedMessage Receive(SocketFlags socketFlags)
        {
            return base.Receive(socketFlags);
        }

        /// <summary>
        /// Queue a message to be sent by the socket.
        /// </summary>
        /// <param name="buffer">An array of type <see cref="byte"/> that contains the message to be sent.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="SocketFlags"/> values.</param>
        /// <returns>A <see cref="SendResult"/> value indicating the send operation outcome.</returns>
        /// <exception cref="ZmqLibException">An error occured during the execution of a native procedure.</exception>
        public new SendResult Send(byte[] buffer, SocketFlags socketFlags)
        {
            return base.Send(buffer, socketFlags);
        }
    }
}
