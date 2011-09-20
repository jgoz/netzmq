namespace ZeroMQ
{
    using ZeroMQ.Proxy;

    /// <summary>
    /// ZMQ_PUSH socket. Used by a pipeline node to send messages to downstream pipeline nodes.
    /// </summary>
    public sealed class PullSocket : Socket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PullSocket"/> class.
        /// </summary>
        /// <param name="context"><see cref="ISocketContext"/> to use when initializing the socket.</param>
        public PullSocket(SocketContext context)
            : base(context, SocketType.Pull)
        {
        }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Bind"]/*'/>
        public new void Bind(string endpoint)
        {
            base.Bind(endpoint);
        }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Connect"]/*'/>
        public new void Connect(string endpoint)
        {
            base.Connect(endpoint);
        }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Receive1"]/*'/>
        public new ReceivedMessage Receive(SocketFlags socketFlags)
        {
            return base.Receive(socketFlags);
        }
    }
}
