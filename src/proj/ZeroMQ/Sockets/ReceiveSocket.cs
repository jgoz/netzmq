namespace ZeroMQ.Sockets
{
    using System;

    using ZeroMQ.Proxy;

    internal class ReceiveSocket : ZmqSocket, IReceiveSocket
    {
        public ReceiveSocket(ISocketProxy proxy)
            : base(proxy)
        {
        }

        public new event EventHandler<ReceiveReadyEventArgs> ReceiveReady
        {
            add { base.ReceiveReady += value; }
            remove { base.ReceiveReady -= value; }
        }

        public ReceivedMessage Receive()
        {
            return this.Receive(SocketFlags.None);
        }

        public new ReceivedMessage Receive(TimeSpan timeout)
        {
            return base.Receive(timeout);
        }
    }
}
