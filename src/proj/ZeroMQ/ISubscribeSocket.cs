namespace ZeroMQ
{
    /// <summary>
    /// A socket that can subscribe to and receive data distributed by a remote publisher.
    /// </summary>
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