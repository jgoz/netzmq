namespace ZeroMQ.Sockets
{
    using System;

    using ZeroMQ.Proxy;

    /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="SubscribeExtSocket"]/*'/>
    public sealed class SubscribeExtSocket : ZmqSocket, ISubscribeSocket
    {
        /// <summary>
        /// The byte value prefixed to outgoing subscription messages.
        /// </summary>
        public const byte SubscribePrefix = 1;

        /// <summary>
        /// The byte value prefixed to outgoing unsubscription messages.
        /// </summary>
        public const byte UnsubscribePrefix = 0;

        internal SubscribeExtSocket(ISocketProxy proxy, IErrorProviderProxy errorProviderProxy)
            : base(proxy, errorProviderProxy)
        {
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="ReceiveReady"]/*'/>
        public new event EventHandler<ReceiveReadyEventArgs> ReceiveReady
        {
            add { base.ReceiveReady += value; }
            remove { base.ReceiveReady -= value; }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="SendReady"]/*'/>
        public new event EventHandler<SendReadyEventArgs> SendReady
        {
            add { base.SendReady += value; }
            remove { base.SendReady -= value; }
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
            if (prefix == null)
            {
                throw new ArgumentNullException("prefix");
            }

            var buffer = new byte[prefix.Length + 1];

            buffer[0] = SubscribePrefix;
            prefix.CopyTo(buffer, 1);

            this.Send(buffer, SocketFlags.None);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="UnsubscribeAll"]/*'/>
        public void UnsubscribeAll()
        {
            this.Unsubscribe(new byte[0]);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Unsubscribe"]/*'/>
        public new void Unsubscribe(byte[] prefix)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException("prefix");
            }

            var buffer = new byte[prefix.Length + 1];

            buffer[0] = UnsubscribePrefix;
            prefix.CopyTo(buffer, 1);

            this.Send(buffer, SocketFlags.None);
        }
    }
}
