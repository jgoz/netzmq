namespace ZeroMQ
{
    using System;

    /// <summary>
    /// A ZeroMQ socket that can bind to local interfaces and connect to remote endpoints.
    /// </summary>
    public interface ISocket : IDisposable
    {
        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Affinity"]/node()[name()!="exception"]'/>
        ulong Affinity { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Backlog"]/node()[name()!="exception"]'/>
        int Backlog { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Identity"]/node()[name()!="exception"]'/>
        byte[] Identity { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Linger"]/node()[name()!="exception"]'/>
        TimeSpan Linger { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="MaxMessageSize"]/node()[name()!="exception"]'/>
        long MaxMessageSize { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="MulticastHops"]/node()[name()!="exception"]'/>
        int MulticastHops { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="MulticastRate"]/node()[name()!="exception"]'/>
        int MulticastRate { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="MulticastRecoveryInterval"]/node()[name()!="exception"]'/>
        TimeSpan MulticastRecoveryInterval { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="ReceiveBufferSize"]/node()[name()!="exception"]'/>
        int ReceiveBufferSize { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="ReceiveHighWatermark"]/node()[name()!="exception"]'/>
        int ReceiveHighWatermark { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="ReceiveMore"]/node()[name()!="exception"]'/>
        bool ReceiveMore { get; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="ReceiveTimeout"]/node()[name()!="exception"]'/>
        TimeSpan ReceiveTimeout { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="ReconnectInterval"]/node()[name()!="exception"]'/>
        TimeSpan ReconnectInterval { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="ReconnectIntervalMax"]/node()[name()!="exception"]'/>
        TimeSpan ReconnectIntervalMax { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="SendBufferSize"]/node()[name()!="exception"]'/>
        int SendBufferSize { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="SendHighWatermark"]/node()[name()!="exception"]'/>
        int SendHighWatermark { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="SendTimeout"]/node()[name()!="exception"]'/>
        TimeSpan SendTimeout { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Bind"]/node()[name()!="exception"]'/>
        void Bind(string endpoint);

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Connect"]/node()[name()!="exception"]'/>
        void Connect(string endpoint);
    }
}
