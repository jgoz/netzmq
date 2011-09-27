namespace ZeroMQ.Sockets
{
    using System;

    using ZeroMQ.Proxy;

    internal class SendSocket : ZmqSocket, ISendSocket
    {
        public SendSocket(ISocketProxy proxy)
            : base(proxy)
        {
        }

        public new event EventHandler<SendReadyEventArgs> SendReady
        {
            add { base.SendReady += value; }
            remove { base.SendReady -= value; }
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
