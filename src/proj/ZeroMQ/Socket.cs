namespace ZeroMQ
{
    using System;
    using System.Text;

    /// <summary>
    /// Sends and receives messages across various transports, synchronously or asynchronously.
    /// </summary>
    /// <remarks>
    /// The <see cref="Socket"/> class defines the common behavior for derived Socket types. 
    /// </remarks>
    public class Socket : IDisposable
    {
        private readonly Proxy.Socket socket;

        private static Encoding defaultEncoding = Encoding.UTF8;

        /// <summary>
        /// Initializes a new instance of the <see cref="Socket"/> class.
        /// </summary>
        /// <param name="context"><see cref="ISocketContext"/> to use when initializing the socket.</param>
        /// <param name="socketType">Socket type for the current socket.</param>
        protected Socket(ISocketContext context, Proxy.SocketType socketType)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            try
            {
                this.socket = new Proxy.Socket(context.Context, socketType);
            }
            catch (Proxy.ZmqException ex)
            {
                throw new ZmqLibException(ex);
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Socket"/> class.
        /// </summary>
        ~Socket()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets or sets the default encoding for all sockets in the current process
        /// </summary>
        public static Encoding DefaultEncoding
        {
            get { return defaultEncoding; }
            set { defaultEncoding = value; }
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
                this.socket.Dispose();
            }
        }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Bind"]/*'/>
        protected void Bind(string endpoint)
        {
            if (this.socket.Bind(endpoint) == -1)
            {
                throw ZmqLibException.GetLastError();
            }
        }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Connect"]/*'/>
        protected void Connect(string endpoint)
        {
            if (this.socket.Connect(endpoint) == -1)
            {
                throw ZmqLibException.GetLastError();
            }
        }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Receive1"]/*'/>
        protected ReceivedMessage Receive(SocketFlags socketFlags)
        {
            byte[] buffer;

            int bytesReceived = this.socket.Receive((int)socketFlags, out buffer);

            if (bytesReceived >= 0)
            {
                return new ReceivedMessage(buffer, ReceiveResult.Received);
            }

            if (ZmqLibException.GetErrorCode() == Proxy.ErrorCode.Eagain)
            {
                return ReceivedMessage.TryAgain;
            }

            throw ZmqLibException.GetLastError();
        }

        /// <include file='CommonDoc.xml' path='ZeroMQ/Members[@name="Send1"]/*'/>
        protected SendResult Send(byte[] buffer, SocketFlags socketFlags)
        {
            int bytesSent = this.socket.Send((int)socketFlags, buffer);

            if (bytesSent >= 0)
            {
                return SendResult.Sent;
            }

            if (ZmqLibException.GetErrorCode() == Proxy.ErrorCode.Eagain)
            {
                return SendResult.TryAgain;
            }

            throw ZmqLibException.GetLastError();
        }

        /// <summary>
        /// Sets an option on the current socket to an integer value.
        /// </summary>
        /// <param name="option">The <see cref="Proxy.SocketOption"/> to set.</param>
        /// <param name="value">The <see cref="int"/> value to set.</param>
        protected void SetSocketOption(Proxy.SocketOption option, int value)
        {
            if (this.socket.SetSocketOption(option, value) == -1)
            {
                throw ZmqLibException.GetLastError();
            }
        }

        /// <summary>
        /// Sets an option on the current socket to an unsigned long value.
        /// </summary>
        /// <param name="option">The <see cref="Proxy.SocketOption"/> to set.</param>
        /// <param name="value">The <see cref="ulong"/> value to set.</param>
        protected void SetSocketOption(Proxy.SocketOption option, ulong value)
        {
            if (this.socket.SetSocketOption(option, value) == -1)
            {
                throw ZmqLibException.GetLastError();
            }
        }

        /// <summary>
        /// Sets an option on the current socket to a string value.
        /// </summary>
        /// <param name="option">The <see cref="Proxy.SocketOption"/> to set.</param>
        /// <param name="value">The <see cref="string"/> value to set.</param>
        protected void SetSocketOption(Proxy.SocketOption option, string value)
        {
            this.SetSocketOption(option, defaultEncoding.GetBytes(value));
        }

        /// <summary>
        /// Sets an option on the current socket to a byte array value.
        /// </summary>
        /// <param name="option">The <see cref="Proxy.SocketOption"/> to set.</param>
        /// <param name="value">The <see cref="byte"/> array value to set.</param>
        protected void SetSocketOption(Proxy.SocketOption option, byte[] value)
        {
            if (this.socket.SetSocketOption(option, value) == -1)
            {
                throw ZmqLibException.GetLastError();
            }
        }

        /// <summary>
        /// Gets an option of the current socket as an integer.
        /// </summary>
        /// <param name="option">The <see cref="Proxy.SocketOption"/> to get.</param>
        /// <returns>The <see cref="int"/> value of the specified option.</returns>
        protected int GetSocketOptionInt32(Proxy.SocketOption option)
        {
            int value;

            if (this.socket.GetSocketOption(option, out value) == -1)
            {
                throw ZmqLibException.GetLastError();
            }

            return value;
        }

        /// <summary>
        /// Gets an option of the current socket as an unsigned long.
        /// </summary>
        /// <param name="option">The <see cref="Proxy.SocketOption"/> to get.</param>
        /// <returns>The <see cref="ulong"/> value of the specified option.</returns>
        protected ulong GetSocketOptionUInt64(Proxy.SocketOption option)
        {
            ulong value;

            if (this.socket.GetSocketOption(option, out value) == -1)
            {
                throw ZmqLibException.GetLastError();
            }

            return value;
        }

        /// <summary>
        /// Gets an option of the current socket as a string.
        /// </summary>
        /// <param name="option">The <see cref="Proxy.SocketOption"/> to get.</param>
        /// <returns>The <see cref="string"/> value of the specified option.</returns>
        protected string GetSocketOptionString(Proxy.SocketOption option)
        {
            return DefaultEncoding.GetString(this.GetSocketOptionBytes(option));
        }

        /// <summary>
        /// Gets an option of the current socket as a byte array.
        /// </summary>
        /// <param name="option">The <see cref="Proxy.SocketOption"/> to get.</param>
        /// <returns>The <see cref="byte"/> array value of the specified option.</returns>
        protected byte[] GetSocketOptionBytes(Proxy.SocketOption option)
        {
            byte[] value;

            if (this.socket.GetSocketOption(option, out value) == -1)
            {
                throw ZmqLibException.GetLastError();
            }

            return value;
        }
    }
}
