namespace ZeroMQ.Sockets
{
    using System;

    /// <summary>
    /// ZMQ_REQ socket. Used by a client to send requests to and receive replies from a service.
    /// </summary>
    public sealed class RequestSocket : ZmqSocket, IDuplexSocket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestSocket"/> class.
        /// </summary>
        /// <param name="context"><see cref="IZmqContext"/> to use when initializing the socket.</param>
        public RequestSocket(ZmqContext context)
            : base(context, SocketType.Req)
        {
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
