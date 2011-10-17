namespace ZeroMQ
{
    using System;

    /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="ReceiveSocket"]/*'/>
    public interface IReceiveSocket : ISocket
    {
        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="ReceiveReady"]/*'/>
        event EventHandler<ReceiveReadyEventArgs> ReceiveReady;

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Receive1"]/node()[name()!="exception"]'/>
        ReceivedMessage Receive();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Receive2"]/node()[name()!="exception"]'/>
        ReceivedMessage Receive(TimeSpan timeout);
    }
}
