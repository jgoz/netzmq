namespace ZeroMQ.Sockets.Devices
{
    /// <summary>
    /// Collects messages from a set of publishers and forwards these to a set of subscribers.
    /// </summary>
    public interface IForwarderDevice : IDevice<ISubscribeSocket, ISendSocket>
    {
    }
}
