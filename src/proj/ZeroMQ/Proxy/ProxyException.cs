namespace ZeroMQ.Proxy
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    internal class ProxyException : Exception
    {
        public ProxyException(int errorCode)
        {
            this.ErrorCode = errorCode;
        }

        public ProxyException(int errorCode, string message)
            : base(message)
        {
            this.ErrorCode = errorCode;
        }

        public ProxyException(int errorCode, string message, Exception inner)
            : base(message, inner)
        {
            this.ErrorCode = errorCode;
        }

        protected ProxyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public int ErrorCode { get; private set; }
    }
}
