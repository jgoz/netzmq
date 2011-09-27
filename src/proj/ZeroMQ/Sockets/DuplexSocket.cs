namespace ZeroMQ.Sockets
{
    using System;

    using ZeroMQ.Proxy;

    internal class DuplexSocket : ZmqSocket, IDuplexSocket
    {
        public DuplexSocket(ISocketProxy proxy)
            : base(proxy)
        {
        }

        public new event EventHandler<ReceiveReadyEventArgs> ReceiveReady
        {
            add { base.ReceiveReady += value; }
            remove { base.ReceiveReady -= value; }
        }

        public new event EventHandler<SendReadyEventArgs> SendReady
        {
            add { base.SendReady += value; }
            remove { base.SendReady -= value; }
        }

        public ReceivedMessage Receive()
        {
            return this.Receive(SocketFlags.None);
        }

        public new ReceivedMessage Receive(TimeSpan timeout)
        {
            return base.Receive(timeout);
        }

        public SendResult Send(byte[] buffer)
        {
            return this.Send(buffer, SocketFlags.None);
        }

        public SendResult Send(byte[] buffer, TimeSpan timeout)
        {
            return this.Send(buffer, SocketFlags.DontWait, timeout);
        }

        public SendResult SendPart(byte[] buffer)
        {
            return this.Send(buffer, SocketFlags.SendMore);
        }

        public SendResult SendPart(byte[] buffer, TimeSpan timeout)
        {
            return this.Send(buffer, SocketFlags.SendMore | SocketFlags.DontWait, timeout);
        }
    }
}
