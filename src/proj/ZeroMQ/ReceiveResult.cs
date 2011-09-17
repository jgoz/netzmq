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
        /// Non-blocking mode was requested and no messages are available at the moment.
        /// </summary>
        TryAgain
    }
}
