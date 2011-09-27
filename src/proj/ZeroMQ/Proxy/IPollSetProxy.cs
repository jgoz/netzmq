namespace ZeroMQ.Proxy
{
    using System;

    internal interface IPollSetProxy : IDisposable
    {
        int Poll(IPollItem[] items, int timeoutMilliseconds);
    }
}
