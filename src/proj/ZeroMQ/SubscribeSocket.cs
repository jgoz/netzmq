namespace ZeroMQ
{
    using ZeroMQ.Proxy;

    /// <summary>
    /// ZMQ_SUB socket. Subscribe to data distributed by a publisher.
    /// </summary>
    public sealed class SubscribeSocket : Socket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscribeSocket"/> class.
        /// </summary>
        /// <param name="context"><see cref="ISocketContext"/> to use when initializing the socket.</param>
        public SubscribeSocket(SocketContext context)
            : base(context, SocketType.Sub)
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
    }
}
