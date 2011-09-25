namespace ZeroMQ.Sockets
{
    using System;

    /// <summary>
    /// ZMQ_SUB socket. Subscribe to data distributed by a publisher. Set a subscription filter
    /// via <see cref="Subscribe(byte[])"/> or <see cref="SubscribeAll"/> before connecting to a publisher.
    /// </summary>
    public sealed class SubscribeSocket : ZmqSocket, ISubscribeSocket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscribeSocket"/> class.
        /// </summary>
        /// <param name="context"><see cref="ZmqContext"/> to use when initializing the socket.</param>
        public SubscribeSocket(ZmqContext context)
            : base(context, SocketType.Sub)
        {
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Receive1"]/*'/>
        public ReceivedMessage Receive()
        {
            return this.Receive(SocketFlags.None);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Receive2"]/*'/>
        public new ReceivedMessage Receive(TimeSpan timeout)
        {
            return base.Receive(timeout);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="SubscribeAll"]/*'/>
        public void SubscribeAll()
        {
            this.Subscribe(new byte[0]);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Subscribe"]/*'/>
        public new void Subscribe(byte[] prefix)
        {
            base.Subscribe(prefix);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="UnsubscribeAll"]/*'/>
        public void UnsubscribeAll()
        {
            this.Unsubscribe(new byte[0]);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Unsubscribe"]/*'/>
        public new void Unsubscribe(byte[] prefix)
        {
            base.Unsubscribe(prefix);
        }
    }
}
