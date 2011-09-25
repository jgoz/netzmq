namespace ZeroMQ
{
    using System;

    /// <summary>
    /// Provides data for <see cref="IReceiveSocket.ReceiveReady"/> events.
    /// </summary>
    public class ReceiveReadyEventArgs : EventArgs
    {
        internal ReceiveReadyEventArgs(IReceiveSocket socket)
        {
            this.Socket = socket;
        }

        /// <summary>
        /// Gets the socket that may be used to receive at least one message without blocking.
        /// </summary>
        public IReceiveSocket Socket { get; private set; }
    }
}
