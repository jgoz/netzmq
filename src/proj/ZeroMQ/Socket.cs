namespace ZeroMQ
{
    using System;
    using System.Runtime.InteropServices;

    using ZeroMQ.Interop;

    /// <summary>
    /// Sends and receives messages across various transports, synchronously or asynchronously.
    /// </summary>
    /// <remarks>
    /// The <see cref="Socket"/> class defines the common behavior for derived Socket types. 
    /// </remarks>
    public abstract class Socket
    {
        private const int VerySmallMessageSize = 32;

        private readonly ISocketContext context;
        private readonly IntPtr socket;
        private readonly SocketType socketType;

        private readonly IntPtr sendBuffer;
        private readonly IntPtr receiveBuffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Socket"/> class.
        /// </summary>
        /// <param name="context"><see cref="ISocketContext"/> to use when initializing the socket.</param>
        /// <param name="socketType">Socket type for the current socket.</param>
        protected Socket(ISocketContext context, SocketType socketType)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.context = context;
            this.socketType = socketType;
            this.socket = LibZmq.Socket(this.context.Handle, (int)this.socketType);

            if (this.socket == IntPtr.Zero)
            {
                throw ZmqLibException.GetLastError();
            }

            this.sendBuffer = Marshal.AllocHGlobal(VerySmallMessageSize);
            this.receiveBuffer = Marshal.AllocHGlobal(VerySmallMessageSize);
        }

        /// <summary>
        /// Queue a message to be sent by the socket.
        /// </summary>
        /// <param name="buffer">An array of type <see cref="Byte"/> that contains the message to be sent.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="SocketFlags"/> values.</param>
        /// <returns>A <see cref="SendResult"/> value indicating the send operation's outcome.</returns>
        protected SendResult Send(byte[] buffer, SocketFlags socketFlags)
        {
            if (LibZmq.MsgInitSize(this.sendBuffer, buffer.Length) != 0)
            {
                throw ZmqLibException.GetLastError();
            }

            Marshal.Copy(buffer, 0, LibZmq.MsgData(this.sendBuffer), buffer.Length);

            int result = LibZmq.Send(this.socket, this.sendBuffer, (int)socketFlags);

            if (LibZmq.MsgClose(this.sendBuffer) != 0)
            {
                throw ZmqLibException.GetLastError();
            }

            if (result == 0)
            {
                return SendResult.Sent;
            }

            if (LibZmq.Errno() == (int)SystemError.EAgain)
            {
                return SendResult.TryAgain;
            }

            throw ZmqLibException.GetLastError();
        }
    }
}
