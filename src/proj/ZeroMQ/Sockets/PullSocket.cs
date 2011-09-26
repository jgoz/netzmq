namespace ZeroMQ.Sockets
{
    using System;

    /// <summary>
    /// ZMQ_PUSH socket. Used by a pipeline node to send messages to downstream pipeline nodes.
    /// </summary>
    public sealed class PullSocket : ZmqSocket, IReceiveSocket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PullSocket"/> class.
        /// </summary>
        /// <param name="context"><see cref="ZmqContext"/> to use when initializing the socket.</param>
        public PullSocket(ZmqContext context)
            : base(context, SocketType.Pull)
        {
        }

        /// <include file='../CommonDoc.xml' path='ZeroMQ/Members[@name="ReceiveReady"]/*'/>
        public new event EventHandler<ReceiveReadyEventArgs> ReceiveReady
        {
            add { base.ReceiveReady += value; }
            remove { base.ReceiveReady -= value; }
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
    }
}
