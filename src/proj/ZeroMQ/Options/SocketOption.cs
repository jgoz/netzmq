namespace ZeroMQ.Options
{
    /// <summary>
    /// Specifies available socket options.
    /// </summary>
    public enum SocketOption
    {
        /// <summary>
        /// The I/O thread affinity for newly created connections.
        /// </summary>
        Affinity = 4,

        /// <summary>
        /// The identity of the specified socket.
        /// </summary>
        Identity = 5,

        /// <summary>
        /// Set a new message filter on a Subscribe socket. 
        /// </summary>
        Subscribe = 6,

        /// <summary>
        /// Remove a message filter from a Subscribe socket.
        /// </summary>
        Unsubscribe = 7,

        /// <summary>
        /// The maximum send or receive data rate for multicast transports.
        /// </summary>
        Rate = 8,

        /// <summary>
        /// The recovery interval for multicast transports.
        /// </summary>
        RecoveryInterval = 9,

        /// <summary>
        /// The underlying kernel transmit buffer size for the socket to the specified size in bytes.
        /// </summary>
        SndBuf = 11,

        /// <summary>
        /// The underlying kernel receive buffer size for the socket to the specified size in bytes.
        /// </summary>
        RcvBuf = 12,

        /// <summary>
        /// Indicate if the multi-part message currently being read has more message parts to follow.
        /// </summary>
        Rcvmore = 13,

        /// <summary>
        /// Get the file descriptor for the current socket.
        /// </summary>
        Fd = 14,

        /// <summary>
        /// Get the event state.
        /// </summary>
        Events = 15,

        /// <summary>
        /// Get the socket type.
        /// </summary>
        Type = 16,

        /// <summary>
        /// The linger period for socket shutdown.
        /// </summary>
        Linger = 17,

        /// <summary>
        /// The initial reconnection interval.
        /// </summary>
        ReconnectInterval = 18,

        /// <summary>
        /// The maximum length of the queue of outstanding peer connections.
        /// </summary>
        Backlog = 19,

        /// <summary>
        /// The maximum reconnection interval.
        /// </summary>
        ReconnectIntervalMax = 21,

        /// <summary>
        /// Limits the size of the inbound message.
        /// </summary>
        MaxMsgSize = 22,

        /// <summary>
        /// The high water mark for outbound messages.
        /// </summary>
        SndHwm = 23,

        /// <summary>
        /// The high water mark for inbound messages.
        /// </summary>
        RcvHwm = 24,

        /// <summary>
        /// The time-to-live field in every multicast packet sent from this socket.
        /// </summary>
        MulticastHops = 25,

        /// <summary>
        /// The timeout for receive operations.
        /// </summary>
        RcvTimeout = 27,

        /// <summary>
        /// The timeout for send operations on the socket.
        /// </summary>
        SndTimeout = 28,

        /// <summary>
        /// Indicate whether the last message part received was a label.
        /// </summary>
        RcvLabel = 29,
    }
}
