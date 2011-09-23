namespace ZeroMQ.Sockets
{
    using ZeroMQ.Proxy;

    /// <summary>
    /// ZMQ_PUSH socket. Used by a pipeline node to send messages to downstream pipeline nodes.
    /// </summary>
    public sealed class PushSocket : ZmqSocket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PushSocket"/> class.
        /// </summary>
        /// <param name="context"><see cref="IZmqContext"/> to use when initializing the socket.</param>
        public PushSocket(ZmqContext context)
            : base(context, SocketType.Push)
        {
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Send1"]/*'/>
        public new SendResult Send(byte[] buffer, SocketFlags socketFlags)
        {
            return base.Send(buffer, socketFlags);
        }
    }
}
