﻿namespace ZeroMQ
{
    using System;

    /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="ZmqPoller"]/*'/>
    public interface IPoller : IDisposable
    {
        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Poll1"]/node()[name()!="exception"]'/>
        void Poll();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Poll2"]/node()[name()!="exception"]'/>
        void Poll(TimeSpan timeout);
    }
}
