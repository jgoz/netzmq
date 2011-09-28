namespace ZeroMQ.Proxy
{
    using System;

    internal interface IContextProxy
    {
        IntPtr Handle { get; }

        void Terminate();
    }
}
