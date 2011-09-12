namespace ZeroMQ
{
    using System;

    /// <summary>
    /// Specifies socket send and receive behaviors.
    /// </summary>
    [Flags]
    public enum SocketFlags
    {
        /// <summary>
        /// Use no flags for this call.
        /// </summary>
        None = 0,

        /// <summary>
        /// Send or receive in non-blocking mode. 
        /// </summary>
        DontWait = 0x1,

        /// <summary>
        /// Indicates that the message being sent is a multi-part message,
        /// and that further message parts are to follow.
        /// </summary>
        SendMore = 0x2,
    }
}
