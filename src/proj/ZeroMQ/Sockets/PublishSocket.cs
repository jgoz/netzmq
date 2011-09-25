namespace ZeroMQ.Sockets
{
    using System;

    /// <summary>
    /// ZMQ_PUB socket. Publish messages to connected peers in a fan-out model.
    /// </summary>
    public sealed class PublishSocket : ZmqSocket, ISendSocket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PublishSocket"/> class.
        /// </summary>
        /// <param name="context"><see cref="ZmqContext"/> to use when initializing the socket.</param>
        public PublishSocket(ZmqContext context)
            : base(context, SocketType.Pub)
        {
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="SendReady"]/*'/>
        public new event EventHandler<SendReadyEventArgs> SendReady
        {
            add { base.SendReady += value; }
            remove { base.SendReady -= value; }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Send1"]/*'/>
        public SendResult Send(byte[] buffer)
        {
            return this.Send(buffer, SocketFlags.None);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Send2"]/*'/>
        public SendResult Send(byte[] buffer, TimeSpan timeout)
        {
            return this.Send(buffer, SocketFlags.DontWait, timeout);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="SendPart1"]/*'/>
        public SendResult SendPart(byte[] buffer)
        {
            return this.Send(buffer, SocketFlags.SendMore);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="SendPart2"]/*'/>
        public SendResult SendPart(byte[] buffer, TimeSpan timeout)
        {
            return this.Send(buffer, SocketFlags.SendMore | SocketFlags.DontWait, timeout);
        }
    }
}
