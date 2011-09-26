namespace ZeroMQ.Sockets
{
    using System;

    /// <summary>
    /// ZMQ_XSUB socket. Extends Subscribe socket by allowing outgoing subscription messages to be sent.
    /// Set a subscription filter via <see cref="Subscribe(byte[])"/> or <see cref="SubscribeAll"/> before
    /// connecting to a publisher.
    /// </summary>
    public sealed class SubscribeExtSocket : ZmqSocket, IDuplexSocket, ISubscribeSocket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscribeExtSocket"/> class.
        /// </summary>
        /// <param name="context"><see cref="ZmqContext"/> to use when initializing the socket.</param>
        public SubscribeExtSocket(ZmqContext context)
            : base(context, SocketType.Xsub)
        {
        }

        /// <include file='../CommonDoc.xml' path='ZeroMQ/Members[@name="ReceiveReady"]/*'/>
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
        public ReceivedMessage Receive()
        {
            return this.Receive(SocketFlags.None);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Receive2"]/*'/>
        public new ReceivedMessage Receive(TimeSpan timeout)
        {
            return base.Receive(timeout);
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
