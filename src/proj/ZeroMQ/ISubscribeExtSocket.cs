namespace ZeroMQ
{
    /// <summary>
    /// A socket that can subscribe to and receive data distributed by a remote publisher.
    /// Extends <see cref="ISubscribeSocket"/> by allowing outgoing subscription messages to be sent
    /// </summary>
    public interface ISubscribeExtSocket : ISubscribeSocket, IDuplexSocket
    {
    }
}
