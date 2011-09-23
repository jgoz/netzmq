namespace ZeroMQ.Sockets
{
    using ZeroMQ.Proxy;

    /// <summary>
    /// ZMQ_XREQ socket. Extends the Request socket by load-balancing outgoing messages and
    /// fair-queuing incoming messages.
    /// </summary>
    public sealed class RequestExtSocket : ZmqSocket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestExtSocket"/> class.
        /// </summary>
        /// <param name="context"><see cref="IZmqContext"/> to use when initializing the socket.</param>
        public RequestExtSocket(ZmqContext context)
            : base(context, SocketType.Xreq)
        {
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
