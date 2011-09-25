namespace ZeroMQ
{
    using System;

    /// <summary>
    /// Provides data for <see cref="ISendSocket.SendReady"/> events.
    /// </summary>
    public class SendReadyEventArgs : EventArgs
    {
        internal SendReadyEventArgs(ISendSocket socket)
        {
            this.Socket = socket;
        }

        /// <summary>
        /// Gets the socket that may be used to send at least one message without blocking.
        /// </summary>
        public ISendSocket Socket { get; private set; }
    }
}
