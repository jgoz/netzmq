namespace ZeroMQ
{
    using System;

    /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="ReceiveSocket"]/*'/>
    public interface IReceiveSocket : ISocket
    {
        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="ReceiveReady"]/*'/>
        event EventHandler<ReceiveReadyEventArgs> ReceiveReady;

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="ReceiveStatus"]/*'/>
        ReceiveResult ReceiveStatus { get; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Receive1"]/node()[name()!="exception"]'/>
        byte[] Receive();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Receive2"]/node()[name()!="exception"]'/>
        byte[] Receive(TimeSpan timeout);
    }
}
