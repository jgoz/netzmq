namespace ZeroMQ
{
    using ZeroMQ.Proxy;

    /// <summary>
    /// ZMQ_XREQ socket. Extends the Request socket by load-balancing outgoing messages and
    /// fair-queuing incoming messages.
    /// </summary>
    public sealed class ExtRequestSocket : Socket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtRequestSocket"/> class.
        /// </summary>
        /// <param name="context"><see cref="ISocketContext"/> to use when initializing the socket.</param>
        public ExtRequestSocket(SocketContext context)
            : base(context, SocketType.Xreq)
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
