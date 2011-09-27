namespace ZeroMQ
{
    using System;

    internal interface IPollItem
    {
        IntPtr Socket { get; }
        PollFlags Events { get; set; }
        PollFlags REvents { get; set; }
    }
}
