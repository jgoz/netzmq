﻿namespace ZeroMQ
{
    using System;

    /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="SendSocket"]/*'/>
    public interface ISendSocket : ISocket
    {
        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="SendReady"]/*'/>
        event EventHandler<SendReadyEventArgs> SendReady;

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Send1"]/node()[name()!="exception"]'/>
        SendResult Send(byte[] buffer);

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Send2"]/node()[name()!="exception"]'/>
        SendResult Send(byte[] buffer, TimeSpan timeout);

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="SendPart1"]/node()[name()!="exception"]'/>
        SendResult SendPart(byte[] buffer);

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="SendPart2"]/node()[name()!="exception"]'/>
        SendResult SendPart(byte[] buffer, TimeSpan timeout);
    }
}
