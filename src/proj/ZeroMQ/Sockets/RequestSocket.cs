namespace ZeroMQ.Sockets
{
    using ZeroMQ.Proxy;

    /// <summary>
    /// ZMQ_REQ socket. Used by a client to send requests to and receive replies from a service.
    /// </summary>
    public sealed class RequestSocket : ZmqSocket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestSocket"/> class.
        /// </summary>
        /// <param name="context"><see cref="IZmqContext"/> to use when initializing the socket.</param>
        public RequestSocket(ZmqContext context)
            : base(context, SocketType.Req)
        {
        }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Connect"]/*'/>
        public new void Connect(string endpoint)
        {
            base.Connect(endpoint);
        }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Receive1"]/*'/>
        public new ReceivedMessage Receive(SocketFlags socketFlags)
        {
            return base.Receive(socketFlags);
        }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Send1"]/*'/>
        public new SendResult Send(byte[] buffer, SocketFlags socketFlags)
        {
            return base.Send(buffer, socketFlags);
        }
    }
}
