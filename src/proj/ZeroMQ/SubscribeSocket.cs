namespace ZeroMQ
{
    using ZeroMQ.Proxy;

    /// <summary>
    /// ZMQ_SUB socket. Subscribe to data distributed by a publisher. Set a subscription filter
    /// via <see cref="Subscribe(byte[])"/> or one of its overloads before connecting to a publisher.
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

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="SubscribeAll"]/*'/>
        public void SubscribeAll()
        {
            this.Subscribe(new byte[0]);
        }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Subscribe"]/*'/>
        public void Subscribe(string prefix)
        {
            this.Subscribe(DefaultEncoding.GetBytes(prefix));
        }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Subscribe"]/*'/>
        public new void Subscribe(byte[] prefix)
        {
            base.Subscribe(prefix);
        }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Unsubscribe"]/*'/>
        public void UnsubscribeAll()
        {
            this.Unsubscribe(new byte[0]);
        }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Unsubscribe"]/*'/>
        public void Unsubscribe(string prefix)
        {
            this.Unsubscribe(DefaultEncoding.GetBytes(prefix));
        }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Unsubscribe"]/*'/>
        public new void Unsubscribe(byte[] prefix)
        {
            base.Unsubscribe(prefix);
        }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Receive1"]/*'/>
        public new ReceivedMessage Receive(SocketFlags socketFlags)
        {
            return base.Receive(socketFlags);
        }
    }
}
