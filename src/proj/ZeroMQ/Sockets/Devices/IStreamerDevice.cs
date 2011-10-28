namespace ZeroMQ.Sockets.Devices
{
    /// <summary>
    /// Collects tasks from a set of pushers and forwards these to a set of pullers.
    /// </summary>
    public interface IStreamerDevice : IDevice<IReceiveSocket, ISendSocket>
    {
    }
}
