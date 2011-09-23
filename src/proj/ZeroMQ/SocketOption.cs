namespace ZeroMQ
{
    internal enum SocketOption
    {
        Affinity = 4,
        Identity = 5,
        Subscribe = 6,
        Unsubscribe = 7,
        Rate = 8,
        RecoveryIvl = 9,
        SndBuf = 11,
        RcvBuf = 12,
        RcvMore = 13,
        Fd = 14,
        Events = 15,
        Type = 16,
        Linger = 17,
        ReconnectIvl = 18,
        Backlog = 19,
        ReconnectIvlMax = 21,
        MaxMsgSize = 22,
        SndHwm = 23,
        RcvHwm = 24,
        MulticastHops = 25,
        RcvTimeo = 27,
        SndTimeo = 28,
        RcvLabel = 29,
    }
}