namespace ZeroMQ
{
    /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="SubscribeSocket"]/*'/>
    public interface ISubscribeSocket : IReceiveSocket
    {
        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="SubscribeAll"]/*'/>
        void SubscribeAll();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Subscribe"]/*'/>
        void Subscribe(byte[] prefix);

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="UnsubscribeAll"]/*'/>
        void UnsubscribeAll();

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Unsubscribe"]/*'/>
        void Unsubscribe(byte[] prefix);
    }
}