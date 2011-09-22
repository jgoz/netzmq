namespace ZeroMQ.Sockets
{
    using ZeroMQ.Proxy;

    /// <summary>
    /// ZMQ_PAIR socket. Unrestricted and unfiltered communication with a single remote endpoint.
    /// </summary>
    public sealed class PairSocket : ZmqSocket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PairSocket"/> class.
        /// </summary>
        /// <param name="context"><see cref="IZmqContext"/> to use when initializing the socket.</param>
        public PairSocket(ZmqContext context)
            : base(context, SocketType.Pair)
        {
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Bind"]/*'/>
        public new void Bind(string endpoint)
        {
            base.Bind(endpoint);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Receive1"]/*'/>
        public new ReceivedMessage Receive(SocketFlags socketFlags)
        {
            return base.Receive(socketFlags);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Send1"]/*'/>
        public new SendResult Send(byte[] buffer, SocketFlags socketFlags)
        {
            return base.Send(buffer, socketFlags);
        }
    }
}
