namespace ZeroMQ
{
    using ZeroMQ.Options;

    /// <summary>
    /// ZMQ_REQ socket. Used by a client to send requests to and receive replies from a service.
    /// </summary>
    public class ReqSocket : Socket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReqSocket"/> class.
        /// </summary>
        /// <param name="context"><see cref="ISocketContext"/> to use when initializing the socket.</param>
        public ReqSocket(ISocketContext context)
            : base(context, SocketType.Req)
        {
        }

        /// <summary>
        /// Connect the current socket to the specified endpoint.
        /// </summary>
        /// <summary>
        /// Connect the current socket to the specified endpoint.
        /// </summary>
        /// <param name="endpoint">
        /// A string consisting of a <em>transport</em> and an <em>address</em>, formatted as
        /// <c><em>transport</em>://<em>address</em></c>.
        /// </param>
        /// <exception cref="ZmqLibException">An error occured during the execution of a native procedure.</exception>
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
