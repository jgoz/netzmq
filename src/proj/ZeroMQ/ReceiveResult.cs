namespace ZeroMQ
{
    /// <summary>
    /// Specifies possible results for socket receive operations.
    /// </summary>
    public enum ReceiveResult
    {
        /// <summary>
        /// The receive operation returned a message that contains data.
        /// </summary>
        Received,

        /// <summary>
        /// The receive buffer was not large enough to retrieve the entire message, so
        /// the resulting message is incomplete.
        /// </summary>
        Truncated,

        /// <summary>
        /// Non-blocking mode was requested and no messages are available at the moment.
        /// </summary>
        TryAgain
    }
}
