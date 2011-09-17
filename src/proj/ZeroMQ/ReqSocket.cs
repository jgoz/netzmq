namespace ZeroMQ
{
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
            : base(context, Proxy.SocketType.Req)
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
