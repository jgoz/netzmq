namespace ZeroMQ
{
    using ZeroMQ.Proxy;

    /// <summary>
    /// ZMQ_XSUB socket. Extends Subscribe socket by allowing outgoing subscription messages.
    /// </summary>
    public sealed class ExtSubscribeSocket : Socket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtSubscribeSocket"/> class.
        /// </summary>
        /// <param name="context"><see cref="ISocketContext"/> to use when initializing the socket.</param>
        public ExtSubscribeSocket(SocketContext context)
            : base(context, SocketType.Xsub)
        {
        }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Bind"]/*'/>
        public new void Bind(string endpoint)
        {
            base.Bind(endpoint);
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
