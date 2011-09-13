namespace ZeroMQ
{
    using System;

    /// <summary>
    /// Stores retrieved message data and the outcome of a <see cref="Socket.Receive"/> operation.
    /// </summary>
    public class ReceivedMessage
    {
        internal ReceivedMessage(int bytesReceived)
        {
            if (bytesReceived < 0)
            {
                throw new ArgumentOutOfRangeException("bytesReceived", bytesReceived, "Expected non-negative value.");
            }

            this.Data = new byte[bytesReceived];
            this.Result = ReceiveResult.Received;
        }

        /// <summary>
        /// Gets the data retrieved from a socket receive operation.
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Gets the result of a socket receive operation.
        /// </summary>
        public ReceiveResult Result { get; private set; }

        internal static ReceivedMessage TryAgain()
        {
            return new ReceivedMessage(0) { Result = ReceiveResult.TryAgain };
        }
    }
}
