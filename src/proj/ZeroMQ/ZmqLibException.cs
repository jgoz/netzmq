namespace ZeroMQ
{
    using System;
    using System.Runtime.Serialization;

    using ZeroMQ.Proxy;

    /// <summary>
    /// An exception thrown by the result of a ZeroMQ library call.
    /// </summary>
    [Serializable]
    public class ZmqLibException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZmqLibException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code returned by the ZeroMQ library call.</param>
        public ZmqLibException(int errorCode)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZmqLibException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code returned by the ZeroMQ library call.</param>
        /// <param name="message">The message that describes the error</param>
        public ZmqLibException(int errorCode, string message)
            : base(message)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZmqLibException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code returned by the ZeroMQ library call.</param>
        /// <param name="message">The message that describes the error</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ZmqLibException(int errorCode, string message, Exception inner)
            : base(message, inner)
        {
            this.ErrorCode = errorCode;
        }

        internal ZmqLibException(ProxyException proxyException)
            : this(proxyException.ErrorCode, proxyException.Message, proxyException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZmqLibException"/> class.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context"><see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected ZmqLibException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the error code returned by the ZeroMQ library call.
        /// </summary>
        public int ErrorCode { get; private set; }

        internal static ZmqLibException GetLastError()
        {
            /*
            Proxy.ZmqException lastError = Proxy.ZmqException.GetLastError();

            return new ZmqLibException(lastError.ErrorCode, lastError.Message);
             */

            // TODO: Get from proxy
            return new ZmqLibException(1);
        }

        internal static Proxy.ErrorCode GetErrorCode()
        {
            // return (Proxy.ErrorCode)Proxy.ZmqException.GetErrorCode();
            return 0; // TODO: get from proxy
        }
    }
}
