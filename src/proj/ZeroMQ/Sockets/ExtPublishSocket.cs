namespace ZeroMQ.Sockets
{
    using ZeroMQ.Proxy;

    /// <summary>
    /// ZMQ_XPUB socket. Extends Publish socket by allowing incoming subscription messages.
    /// </summary>
    public sealed class ExtPublishSocket : ZmqSocket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtPublishSocket"/> class.
        /// </summary>
        /// <param name="context"><see cref="IZmqContext"/> to use when initializing the socket.</param>
        public ExtPublishSocket(ZmqContext context)
            : base(context, SocketType.Xpub)
        {
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Bind"]/*'/>
        public new void Bind(string endpoint)
        {
            base.Bind(endpoint);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Receive1"]/*'/>
        public new ReceivedMessage Receive(SocketFlags socketFlags)
        {
            return base.Receive(socketFlags);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Send1"]/*'/>
        public new SendResult Send(byte[] buffer, SocketFlags socketFlags)
        {
            return base.Send(buffer, socketFlags);
        }
    }
}
