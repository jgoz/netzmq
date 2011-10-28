namespace ZeroMQ.Sockets.Devices
{
    /// <summary>
    /// A shared queue that collects requests from a set of clients and distributes
    /// these fairly among a set of services.
    /// </summary>
    public interface IQueueDevice : IDevice<IDuplexSocket, IDuplexSocket>
    {
    }
}
