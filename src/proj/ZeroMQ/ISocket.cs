namespace ZeroMQ
{
    using System;

    /// <summary>
    /// A ZeroMQ socket that can bind to local interfaces and connect to remote endpoints.
    /// </summary>
    public interface ISocket : IDisposable
    {
        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Affinity"]/*'/>
        ulong Affinity { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Backlog"]/*'/>
        int Backlog { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Identity"]/*'/>
        byte[] Identity { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Linger"]/*'/>
        TimeSpan Linger { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="MaxMessageSize"]/*'/>
        long MaxMessageSize { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="MulticastHops"]/*'/>
        int MulticastHops { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="MulticastRate"]/*'/>
        int MulticastRate { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="MulticastRecoveryInterval"]/*'/>
        TimeSpan MulticastRecoveryInterval { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="ReceiveBufferSize"]/*'/>
        int ReceiveBufferSize { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="ReceiveHighWatermark"]/*'/>
        int ReceiveHighWatermark { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="ReceiveMore"]/*'/>
        bool ReceiveMore { get; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="ReceiveTimeout"]/*'/>
        TimeSpan ReceiveTimeout { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="ReconnectInterval"]/*'/>
        TimeSpan ReconnectInterval { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="ReconnectIntervalMax"]/*'/>
        TimeSpan ReconnectIntervalMax { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="SendBufferSize"]/*'/>
        int SendBufferSize { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="SendHighWatermark"]/*'/>
        int SendHighWatermark { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="SendTimeout"]/*'/>
        TimeSpan SendTimeout { get; set; }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Bind"]/node()[name()!="exception"]'/>
        void Bind(string endpoint);

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Connect"]/node()[name()!="exception"]'/>
        void Connect(string endpoint);
    }
}
