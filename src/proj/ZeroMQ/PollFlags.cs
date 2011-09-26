namespace ZeroMQ
{
    using System;

    [Flags]
    internal enum PollFlags : short
    {
        PollIn = 0x1,
        PollOut = 0x2,
        PollErr = 0x4
    }
}
