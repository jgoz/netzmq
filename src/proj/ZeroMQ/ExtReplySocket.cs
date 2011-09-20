﻿namespace ZeroMQ
{
    using ZeroMQ.Proxy;

    /// <summary>
    /// ZMQ_XREP socket. Extends the Reply socket by identity-stamping incoming messages so that
    /// outgoing messages can be correctly routed.
    /// </summary>
    public sealed class ExtReplySocket : Socket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtReplySocket"/> class.
        /// </summary>
        /// <param name="context"><see cref="ISocketContext"/> to use when initializing the socket.</param>
        public ExtReplySocket(SocketContext context)
            : base(context, SocketType.Xrep)
        {
        }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Bind"]/*'/>
        public new void Bind(string endpoint)
        {
            base.Bind(endpoint);
        }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Receive1"]/*'/>
        public new ReceivedMessage Receive(SocketFlags socketFlags)
        {
            return base.Receive(socketFlags);
        }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Send1"]/*'/>
        public new SendResult Send(byte[] buffer, SocketFlags socketFlags)
        {
            return base.Send(buffer, socketFlags);
        }
    }
}
