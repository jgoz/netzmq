namespace ZeroMQ.Proxy
{
    internal interface IDeviceProxy
    {
        ISocketProxy Frontend { get; }
        ISocketProxy Backend { get; }
        bool IsRunning { get; set; }

        int Run();
    }
}
