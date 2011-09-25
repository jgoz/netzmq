namespace ZeroMQ
{
    /// <summary>
    /// A socket that is capable of both sending and receiving messages to and from remote endpoints.
    /// </summary>
    public interface IDuplexSocket : IReceiveSocket, ISendSocket
    {
    }
}
