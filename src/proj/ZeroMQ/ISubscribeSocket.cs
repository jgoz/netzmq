namespace ZeroMQ
{
    /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="SubscribeSocket"]/*'/>
    public interface ISubscribeSocket : IReceiveSocket
    {
        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="SubscribeAll"]/node()[name()!="exception"]'/>
        void SubscribeAll();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Subscribe"]/node()[name()!="exception"]'/>
        void Subscribe(byte[] prefix);

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="UnsubscribeAll"]/node()[name()!="exception"]'/>
        void UnsubscribeAll();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Unsubscribe"]/node()[name()!="exception"]'/>
        void Unsubscribe(byte[] prefix);
    }
}