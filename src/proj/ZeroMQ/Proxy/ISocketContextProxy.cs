namespace ZeroMQ.Proxy
{
    using System;

    internal interface ISocketContextProxy : IDisposable
    {
        IntPtr Handle { get; }
    }
}
