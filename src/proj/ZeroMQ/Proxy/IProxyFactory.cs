namespace ZeroMQ.Proxy
{
    using System;

    internal interface IProxyFactory
    {
        IErrorProviderProxy ErrorProvider { get; }

        IContextProxy CreateContext(int threadPoolSize);
        ISocketProxy CreateSocket(IntPtr socket);
        IPollSetProxy CreatePollSet(int socketCount);
        IDeviceProxy CreateDevice(ISocketProxy inSocket, ISocketProxy outSocket);
    }
}