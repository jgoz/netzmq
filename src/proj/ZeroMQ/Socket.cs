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

        private IntPtr sendBuffer;
        private IntPtr receiveBuffer;
        private int bufferSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="Socket"/> class.
        /// </summary>
        /// <param name="context"><see cref="ISocketContext"/> to use when initializing the socket.</param>
        /// <param name="socketType">Socket type for the current socket.</param>
        /// <remarks>
        /// This constructer uses an empirically derived buffer size for the best Marshal.Copy performance
        /// by processor architecture. On x86 systems, this is 2048 bytes. On x64 systems this is 8192 bytes.
        /// If message sizes are expected to reach or exceed these values, use the <see cref="Socket(ISocketContext, SocketType, int)"/> 
        /// constructor and specify a larger buffer size. Otherwise, there is a chance that received messages
        /// could be truncated.
        /// </remarks>
        protected Socket(ISocketContext context, SocketType socketType)
            : this(context, socketType, LibZmq.Is64BitProcess() ? 8192 : 2048)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Socket"/> class.
        /// </summary>
        /// <param name="context"><see cref="ISocketContext"/> to use when initializing the socket.</param>
        /// <param name="socketType">Socket type for the current socket.</param>
        /// <param name="bufferSize">The initial size for send and receive buffers. Buffers will expand if necessary.</param>
        /// <remarks>
        /// <paramref name="bufferSize"/> should be set to an appropriately large value for the implementing
        /// application. Otherwise, received messages may be truncated.
        /// </remarks>
        protected Socket(ISocketContext context, SocketType socketType, int bufferSize)
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

            this.bufferSize = bufferSize;
            this.sendBuffer = Marshal.AllocHGlobal(this.bufferSize);
            this.receiveBuffer = Marshal.AllocHGlobal(this.bufferSize);
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
        /// Create an endpoint for accepting connections and bind it to the current socket.
        /// </summary>
        /// <param name="endpoint">
        /// A string consisting of a <em>transport</em> and an <em>address</em>, formatted as
        /// <c><em>transport</em>://<em>address</em></c>.
        /// </param>
        /// <exception cref="ZmqLibException">An error occured during the execution of a native procedure.</exception>
        protected void Bind(string endpoint)
        {
            if (LibZmq.Bind(this.socket, endpoint) == -1)
            {
                throw ZmqLibException.GetLastError();
            }
        }

        /// <summary>
        /// Connect the current socket to the specified endpoint.
        /// </summary>
        /// <param name="uri">A <see cref="Uri"/> that represents the remote endpoint.</param>
        /// <exception cref="ZmqLibException">An error occured during the execution of a native procedure.</exception>
        protected void Connect(Uri uri)
        {
            if (LibZmq.Connect(this.socket, uri.ToZeroMQEndpoint()) == -1)
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
            int bytesReceived = LibZmq.Recv(this.socket, this.receiveBuffer, (UIntPtr)this.bufferSize, (int)socketFlags);

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
                bytesReceived <= this.bufferSize ? ReceiveResult.Received : ReceiveResult.Truncated);

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
            if (this.bufferSize >= minLength)
            {
                return;
            }

            while (this.bufferSize < minLength)
            {
                this.bufferSize *= 2;
            }

            this.sendBuffer = Marshal.ReAllocHGlobal(this.sendBuffer, (IntPtr)this.bufferSize);
            this.receiveBuffer = Marshal.ReAllocHGlobal(this.receiveBuffer, (IntPtr)this.bufferSize);
        }
    }
}
