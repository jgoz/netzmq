namespace ZeroMQ
{
    using System;

    using ZeroMQ.Sockets;

    /// <summary>
    /// Stores retrieved message data and the outcome of a <see cref="ZmqSocket.Receive"/> operation.
    /// </summary>
    public class ReceivedMessage
    {
        private static readonly ReceivedMessage TryAgainResult = new ReceivedMessage(0, ReceiveResult.TryAgain);

        internal ReceivedMessage(int bytesReceived, ReceiveResult result)
        {
            if (bytesReceived < 0)
            {
                throw new ArgumentOutOfRangeException("bytesReceived", bytesReceived, "Expected non-negative value.");
            }

            this.Data = new byte[bytesReceived];
            this.Result = result;
        }

        internal ReceivedMessage(byte[] data, ReceiveResult result)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            this.Data = data;
            this.Result = result;
        }

        /// <summary>
        /// Gets the data retrieved from a socket receive operation.
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Gets the result of a socket receive operation.
        /// </summary>
        public ReceiveResult Result { get; private set; }

        internal static ReceivedMessage TryAgain
        {
            get { return TryAgainResult; }
        }
    }
}
