namespace ZeroMQ.Proxy
{
    using System;

    internal interface IContextProxy : IDisposable
    {
        IntPtr Handle { get; }
    }
}
