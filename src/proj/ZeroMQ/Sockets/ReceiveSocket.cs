namespace ZeroMQ.Sockets
{
    using System;

    using ZeroMQ.Proxy;

    /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="ReceiveSocket"]/*'/>
    public sealed class ReceiveSocket : ZmqSocket, IReceiveSocket
    {
        internal ReceiveSocket(ISocketProxy proxy, IErrorProviderProxy errorProviderProxy)
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
    }
}
