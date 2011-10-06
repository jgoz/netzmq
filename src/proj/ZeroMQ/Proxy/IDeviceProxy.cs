namespace ZeroMQ.Proxy
{
    internal interface IDeviceProxy
    {
        bool IsRunning { get; set; }

        int Run();
    }
}
