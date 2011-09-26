namespace ZeroMQ
{
    using System;

    /// <summary>
    /// Multiplexes input/output events in a level-triggered fashion over a set of sockets.
    /// </summary>
    public interface IPoller : IDisposable
    {
        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Poll1"]/node()[name()!="exception"]'/>
        void Poll();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Poll2"]/node()[name()!="exception"]'/>
        void Poll(TimeSpan timeout);
    }
}
