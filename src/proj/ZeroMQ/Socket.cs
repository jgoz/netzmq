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

        private readonly IntPtr sendMsg;
        private readonly IntPtr receiveMsg;

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

            this.sendMsg = Marshal.AllocHGlobal(VerySmallMessageSize);
            this.receiveMsg = Marshal.AllocHGlobal(VerySmallMessageSize);
        }

        /// <summary>
        /// Receive a message from a remote socket.
        /// </summary>
        /// <param name="socketFlags">A bitwise combination of <see cref="SocketFlags"/> values.</param>
        /// <returns>A <see cref="ReceivedMessage"/> object containing the data recieved and the operation outcome.</returns>
        /// <exception cref="ZmqLibException">An error occured during the execution of a native procedure.</exception>
        protected ReceivedMessage Receive(SocketFlags socketFlags)
        {
            if (LibZmq.MsgInit(this.receiveMsg) != 0)
            {
                throw ZmqLibException.GetLastError();
            }

            int bytesReceived = LibZmq.RecvMsg(this.socket, this.receiveMsg, (int)socketFlags);

            if (bytesReceived == -1)
            {
                if (LibZmq.Errno() == (int)SystemError.EAgain)
                {
                    return ReceivedMessage.TryAgain();
                }

                throw ZmqLibException.GetLastError();
            }

            var result = new ReceivedMessage(bytesReceived);

            IntPtr msgData = LibZmq.MsgData(this.receiveMsg);
            Marshal.Copy(msgData, result.Data, 0, bytesReceived);

            if (msgData == IntPtr.Zero)
            {
                throw ZmqLibException.GetLastError();
            }

            if (LibZmq.MsgClose(this.receiveMsg) == -1)
            {
                throw ZmqLibException.GetLastError();
            }

            return result;
        }

        /// <summary>
        /// Queue a message to be sent by the socket.
        /// </summary>
        /// <param name="buffer">An array of type <see cref="Byte"/> that contains the message to be sent.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="SocketFlags"/> values.</param>
        /// <returns>A <see cref="SendResult"/> value indicating the send operation outcome.</returns>
        /// <exception cref="ZmqLibException">An error occured during the execution of a native procedure.</exception>
        protected SendResult Send(byte[] buffer, SocketFlags socketFlags)
        {
            if (LibZmq.MsgInitSize(this.sendMsg, buffer.Length) != 0)
            {
                throw ZmqLibException.GetLastError();
            }

            Marshal.Copy(buffer, 0, LibZmq.MsgData(this.sendMsg), buffer.Length);

            int result = LibZmq.SendMsg(this.socket, this.sendMsg, (int)socketFlags);

            if (LibZmq.MsgClose(this.sendMsg) == -1)
            {
                throw ZmqLibException.GetLastError();
            }

            if (result == -1)
            {
                if (LibZmq.Errno() == (int)SystemError.EAgain)
                {
                    return SendResult.TryAgain;
                }

                throw ZmqLibException.GetLastError();
            }

            return SendResult.Sent;
        }
    }
}
