namespace ZeroMQ
{
    using System;

    /// <summary>
    /// A socket that is capable of receiving messages from remote endpoints.
    /// </summary>
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
