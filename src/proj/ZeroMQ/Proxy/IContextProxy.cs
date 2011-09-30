namespace ZeroMQ.Proxy
{
    using System;

    internal interface IContextProxy
    {
        IntPtr CreateSocket(int socketType);

        void Terminate();
    }
}
