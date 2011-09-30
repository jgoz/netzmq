namespace ZeroMQ.Proxy
{
    using System;

    internal interface ISocketProxy
    {
        IntPtr Handle { get; }

        int Bind(string endpoint);
        int Connect(string endpoint);
        int Close();

        int SetSocketOption(int option, int value);
        int SetSocketOption(int option, long value);
        int SetSocketOption(int option, ulong value);
        int SetSocketOption(int option, byte[] value);
        int GetSocketOption(int option, out int value);
        int GetSocketOption(int option, out long value);
        int GetSocketOption(int option, out ulong value);
        int GetSocketOption(int option, out byte[] value);

        int Receive(int socketFlags, out byte[] buffer);
        int Send(int socketFlags, byte[] buffer);
    }
}
