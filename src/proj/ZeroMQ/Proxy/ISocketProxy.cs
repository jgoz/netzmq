namespace ZeroMQ.Proxy
{
    using System;

    internal interface ISocketProxy
    {
        IntPtr Handle { get; }

        int Bind(string endpoint);
        int Connect(string endpoint);

        int Close();

        int SetSocketOption(SocketOption option, int value);
        int SetSocketOption(SocketOption option, long value);
        int SetSocketOption(SocketOption option, ulong value);
        int SetSocketOption(SocketOption option, byte[] value);

        int GetSocketOption(SocketOption option, out int value);
        int GetSocketOption(SocketOption option, out long value);
        int GetSocketOption(SocketOption option, out ulong value);
        int GetSocketOption(SocketOption option, out byte[] value);

        int Receive(int socketFlags, out byte[] buffer);
        int Send(int socketFlags, byte[] buffer);
    }
}
