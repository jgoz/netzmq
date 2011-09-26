namespace ZeroMQ.Proxy
{
    using System;

    internal interface IPollerProxy : IDisposable
    {
        int Poll(IPollItem[] items, int timeoutMilliseconds);
    }
}
