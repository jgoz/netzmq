namespace ZeroMQ.Sockets
{
    using System;

    using ZeroMQ.Proxy;

    /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="SubscribeSocket"]/*'/>
    public sealed class SubscribeSocket : ZmqSocket, ISubscribeSocket
    {
        internal SubscribeSocket(ISocketProxy proxy, IErrorProviderProxy errorProviderProxy)
            : base(proxy, errorProviderProxy)
        {
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="ReceiveReady"]/*'/>
        public new event EventHandler<ReceiveReadyEventArgs> ReceiveReady
        {
            add { base.ReceiveReady += value; }
            remove { base.ReceiveReady -= value; }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Receive1"]/*'/>
        public byte[] Receive()
        {
            return this.Receive(SocketFlags.None);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Receive2"]/*'/>
        public new byte[] Receive(TimeSpan timeout)
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
