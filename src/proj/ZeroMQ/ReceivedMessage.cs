namespace ZeroMQ
{
    using System;

    /// <summary>
    /// Stores retrieved message data and the outcome of a Receive operation.
    /// </summary>
    public class ReceivedMessage
    {
        internal static readonly ReceivedMessage TryAgain = new ReceivedMessage(ReceiveResult.TryAgain);
        internal static readonly ReceivedMessage Interrupted = new ReceivedMessage(ReceiveResult.Interrupted);

        internal ReceivedMessage(byte[] data, ReceiveResult result, bool hasMoreParts)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            this.Data = data;
            this.Result = result;
            this.HasMoreParts = hasMoreParts;
        }

        private ReceivedMessage(ReceiveResult result)
            : this(new byte[0], result, false)
        {
        }

        /// <summary>
        /// Gets the data retrieved from a socket receive operation.
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Gets the result of a socket receive operation.
        /// </summary>
        public ReceiveResult Result { get; private set; }

        /// <summary>
        /// Gets a value indicating whether more message parts will follow this message.
        /// </summary>
        public bool HasMoreParts { get; private set; }
    }
}
