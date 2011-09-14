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
    public abstract class Socket : IDisposable
    {
        private readonly ISocketContext context;
        private readonly IntPtr socket;
        private readonly SocketType socketType;

        // Empirically derived size for best performance by platform
        private static int bufferSize = LibZmq.Is64BitProcess() ? 8192 : 2048;

        private IntPtr sendBuffer;
        private IntPtr receiveBuffer;

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

            this.sendBuffer = Marshal.AllocHGlobal(bufferSize);
            this.receiveBuffer = Marshal.AllocHGlobal(bufferSize);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Socket"/> class.
        /// </summary>
        ~Socket()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="Socket"/> class.
        /// </summary>
        public virtual void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="Socket"/>, and optionally disposes of the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }

            Marshal.FreeHGlobal(this.sendBuffer);
            Marshal.FreeHGlobal(this.receiveBuffer);

            if (LibZmq.Close(this.socket) == -1)
            {
                throw ZmqLibException.GetLastError();
            }
        }

        /// <summary>
        /// Receive a message from a remote socket.
        /// </summary>
        /// <param name="socketFlags">A bitwise combination of <see cref="SocketFlags"/> values.</param>
        /// <returns>A <see cref="ReceivedMessage"/> object containing the data recieved and the operation outcome.</returns>
        /// <exception cref="ZmqLibException">An error occured during the execution of a native procedure.</exception>
        protected ReceivedMessage Receive(SocketFlags socketFlags)
        {
            int bytesReceived = LibZmq.Recv(this.socket, this.receiveBuffer, (UIntPtr)bufferSize, (int)socketFlags);

            if (bytesReceived == -1)
            {
                if (LibZmq.Errno() == (int)SystemError.EAgain)
                {
                    return ReceivedMessage.TryAgain;
                }

                throw ZmqLibException.GetLastError();
            }

            var result = new ReceivedMessage(
                bytesReceived,
                bytesReceived <= bufferSize ? ReceiveResult.Received : ReceiveResult.Truncated);

            this.EnsureBufferCapacity(bytesReceived);

            Marshal.Copy(this.receiveBuffer, result.Data, 0, bytesReceived);

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
            this.EnsureBufferCapacity(buffer.Length);

            Marshal.Copy(buffer, 0, LibZmq.MsgData(this.sendBuffer), buffer.Length);

            int result = LibZmq.Send(this.socket, this.sendBuffer, (UIntPtr)buffer.Length, (int)socketFlags);

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

        private void EnsureBufferCapacity(int minLength)
        {
            if (bufferSize >= minLength)
            {
                return;
            }

            while (bufferSize < minLength)
            {
                bufferSize *= 2;
            }

            this.sendBuffer = Marshal.ReAllocHGlobal(this.sendBuffer, (IntPtr)bufferSize);
            this.receiveBuffer = Marshal.ReAllocHGlobal(this.receiveBuffer, (IntPtr)bufferSize);
        }
    }
}
