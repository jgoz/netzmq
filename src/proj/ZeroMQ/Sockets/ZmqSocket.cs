namespace ZeroMQ.Sockets
{
    using System;
    using System.Diagnostics;

    using ZeroMQ.Proxy;

    /// <summary>
    /// Sends and receives messages across various transports, synchronously or asynchronously.
    /// </summary>
    /// <remarks>
    /// The <see cref="ZmqSocket"/> class defines the common behavior for derived socket types. 
    /// </remarks>
    public abstract class ZmqSocket : IDisposable
    {
        private readonly ISocketProxy socket;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZmqSocket"/> class.
        /// </summary>
        /// <param name="context"><see cref="IZmqContext"/> to use when initializing the socket.</param>
        /// <param name="socketType">Socket type for the current socket.</param>
        /// <exception cref="ZmqLibException">An error occured while initializing the underlying socket.</exception>
        internal ZmqSocket(ZmqContext context, SocketType socketType)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            try
            {
                this.socket = ProxyFactory.CreateSocket(context.Handle, socketType);
            }
            catch (ProxyException ex)
            {
                throw new ZmqLibException(ex);
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ZmqSocket"/> class.
        /// </summary>
        ~ZmqSocket()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets or sets the I/O thread affinity for newly created connections on this socket.
        /// </summary>
        public ulong Affinity
        {
            get { return this.GetSocketOptionUInt64(SocketOption.Affinity); }
            set { this.SetSocketOption(SocketOption.Affinity, value); }
        }

        /// <summary>
        /// Gets or sets the maximum length of the queue of outstanding peer connections. (Default = 100 connections).
        /// </summary>
        public int Backlog
        {
            get { return this.GetSocketOptionInt32(SocketOption.Backlog); }
            set { this.SetSocketOption(SocketOption.Backlog, value); }
        }

        /// <summary>
        /// Gets or sets the identity of the current socket.
        /// </summary>
        public byte[] Identity
        {
            get { return this.GetSocketOptionBytes(SocketOption.Identity); }
            set { this.SetSocketOption(SocketOption.Identity, value); }
        }

        /// <summary>
        /// Gets or sets the identity of the current socket as a string using <see cref="ZmqContext.DefaultEncoding"/>.
        /// </summary>
        public string IdentityString
        {
            get { return this.GetSocketOptionString(SocketOption.Identity); }
            set { this.SetSocketOption(SocketOption.Identity, value); }
        }

        /// <summary>
        /// Gets or sets the linger period for socket shutdown. (Default = <see cref="TimeSpan.MaxValue"/>, infinite).
        /// </summary>
        public TimeSpan Linger
        {
            get { return GetTimeSpan(this.GetSocketOptionInt32(SocketOption.Linger)); }
            set { this.SetSocketOption(SocketOption.Linger, GetMilliseconds(value)); }
        }

        /// <summary>
        /// Gets or sets the maximum size for inbound messages (bytes). (Default = -1, no limit).
        /// </summary>
        public long MaxMessageSize
        {
            get { return this.GetSocketOptionInt64(SocketOption.MaxMsgSize); }
            set { this.SetSocketOption(SocketOption.MaxMsgSize, value); }
        }

        /// <summary>
        /// Gets or sets the time-to-live field in every multicast packet sent from this socket (network hops). (Default = 1 hop).
        /// </summary>
        public int MulticastHops
        {
            get { return this.GetSocketOptionInt32(SocketOption.MulticastHops); }
            set { this.SetSocketOption(SocketOption.MulticastHops, value); }
        }

        /// <summary>
        /// Gets or sets the maximum send or receive data rate for multicast transports (kbps). (Default = 100 kbps).
        /// </summary>
        public int MulticastRate
        {
            get { return this.GetSocketOptionInt32(SocketOption.Rate); }
            set { this.SetSocketOption(SocketOption.Rate, value); }
        }

        /// <summary>
        /// Gets or sets the recovery interval for multicast transports. (Default = 10 seconds).
        /// </summary>
        public TimeSpan MulticastRecoveryInterval
        {
            get { return GetTimeSpan(this.GetSocketOptionInt32(SocketOption.RecoveryIvl)); }
            set { this.SetSocketOption(SocketOption.RecoveryIvl, GetMilliseconds(value)); }
        }

        /// <summary>
        /// Gets or sets the underlying kernel receive buffer size for the current socket (bytes). (Default = 0, OS default).
        /// </summary>
        public int ReceiveBufferSize
        {
            get { return this.GetSocketOptionInt32(SocketOption.RcvBuf); }
            set { this.SetSocketOption(SocketOption.RcvBuf, value); }
        }

        /// <summary>
        /// Gets or sets the high water mark for inbound messages (number of messages). (Default = 0, no limit).
        /// </summary>
        public int ReceiveHighWatermark
        {
            get { return this.GetSocketOptionInt32(SocketOption.RcvHwm); }
            set { this.SetSocketOption(SocketOption.RcvHwm, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the multi-part message currently being read has more message parts to follow.
        /// </summary>
        public bool ReceiveMore
        {
            get { return this.GetSocketOptionInt32(SocketOption.RcvMore) == 1; }
        }

        /// <summary>
        /// Gets or sets the timeout for receive operations. (Default = <see cref="TimeSpan.MaxValue"/>, infinite).
        /// </summary>
        public TimeSpan ReceiveTimeout
        {
            get { return GetTimeSpan(this.GetSocketOptionInt32(SocketOption.RcvTimeo)); }
            set { this.SetSocketOption(SocketOption.RcvTimeo, GetMilliseconds(value)); }
        }

        /// <summary>
        /// Gets or sets the initial reconnection interval. (Default = 100 milliseconds).
        /// </summary>
        public TimeSpan ReconnectInterval
        {
            get { return GetTimeSpan(this.GetSocketOptionInt32(SocketOption.ReconnectIvl)); }
            set { this.SetSocketOption(SocketOption.ReconnectIvl, GetMilliseconds(value)); }
        }

        /// <summary>
        /// Gets or sets the maximum reconnection interval. (Default = 0, only use <see cref="ReconnectInterval"/>).
        /// </summary>
        public TimeSpan ReconnectIntervalMax
        {
            get { return GetTimeSpan(this.GetSocketOptionInt32(SocketOption.ReconnectIvlMax)); }
            set { this.SetSocketOption(SocketOption.ReconnectIvlMax, GetMilliseconds(value)); }
        }

        /// <summary>
        /// Gets or sets the underlying kernel transmit buffer size for the current socket (bytes). (Default = 0, OS default).
        /// </summary>
        public int SendBufferSize
        {
            get { return this.GetSocketOptionInt32(SocketOption.SndBuf); }
            set { this.SetSocketOption(SocketOption.SndBuf, value); }
        }

        /// <summary>
        /// Gets or sets the high water mark for outbound messages (number of messages). (Default = 0, no limit).
        /// </summary>
        public int SendHighWatermark
        {
            get { return this.GetSocketOptionInt32(SocketOption.SndHwm); }
            set { this.SetSocketOption(SocketOption.SndHwm, value); }
        }

        /// <summary>
        /// Gets or sets the timeout for send operations. (Default = <see cref="TimeSpan.MaxValue"/>, infinite).
        /// </summary>
        public TimeSpan SendTimeout
        {
            get { return GetTimeSpan(this.GetSocketOptionInt32(SocketOption.SndTimeo)); }
            set { this.SetSocketOption(SocketOption.SndTimeo, GetMilliseconds(value)); }
        }

        /// <summary>
        /// Create an endpoint for accepting connections and bind it to the current socket.
        /// </summary>
        /// <param name="endpoint">
        /// A string consisting of a <em>transport</em> and an <em>address</em>, formatted as <c><em>transport</em>://<em>address</em></c>.
        /// </param>
        /// <exception cref="ZmqLibException">An error occured during the execution of a native procedure.</exception>
        public void Bind(string endpoint)
        {
            if (this.socket.Bind(endpoint) == -1)
            {
                throw ZmqLibException.GetLastError();
            }
        }

        /// <summary>
        /// Connect the current socket to the specified endpoint.
        /// </summary>
        /// <param name="endpoint">
        /// A string consisting of a <em>transport</em> and an <em>address</em>, formatted as <c><em>transport</em>://<em>address</em></c>.
        /// </param>
        /// <exception cref="ZmqLibException">An error occured during the execution of a native procedure.</exception>
        public void Connect(string endpoint)
        {
            if (this.socket.Connect(endpoint) == -1)
            {
                throw ZmqLibException.GetLastError();
            }
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="ZmqSocket"/> class.
        /// </summary>
        public virtual void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Sets an option on the current socket to an integer value.
        /// </summary>
        /// <param name="option">The <see cref="SocketOption"/> to set.</param>
        /// <param name="value">The <see cref="int"/> value to set.</param>
        internal void SetSocketOption(SocketOption option, int value)
        {
            if (this.socket.SetSocketOption(option, value) == -1)
            {
                throw ZmqLibException.GetLastError();
            }
        }

        /// <summary>
        /// Sets an option on the current socket to a long value.
        /// </summary>
        /// <param name="option">The <see cref="SocketOption"/> to set.</param>
        /// <param name="value">The <see cref="long"/> value to set.</param>
        internal void SetSocketOption(SocketOption option, long value)
        {
            if (this.socket.SetSocketOption(option, value) == -1)
            {
                throw ZmqLibException.GetLastError();
            }
        }

        /// <summary>
        /// Sets an option on the current socket to an unsigned long value.
        /// </summary>
        /// <param name="option">The <see cref="SocketOption"/> to set.</param>
        /// <param name="value">The <see cref="ulong"/> value to set.</param>
        internal void SetSocketOption(SocketOption option, ulong value)
        {
            if (this.socket.SetSocketOption(option, value) == -1)
            {
                throw ZmqLibException.GetLastError();
            }
        }

        /// <summary>
        /// Sets an option on the current socket to a string value.
        /// </summary>
        /// <param name="option">The <see cref="SocketOption"/> to set.</param>
        /// <param name="value">The <see cref="string"/> value to set.</param>
        internal void SetSocketOption(SocketOption option, string value)
        {
            this.SetSocketOption(option, value.ToZmqBuffer());
        }

        /// <summary>
        /// Sets an option on the current socket to a byte array value.
        /// </summary>
        /// <param name="option">The <see cref="SocketOption"/> to set.</param>
        /// <param name="value">The <see cref="byte"/> array value to set.</param>
        internal void SetSocketOption(SocketOption option, byte[] value)
        {
            if (this.socket.SetSocketOption(option, value) == -1)
            {
                throw ZmqLibException.GetLastError();
            }
        }

        /// <summary>
        /// Gets an option of the current socket as an integer.
        /// </summary>
        /// <param name="option">The <see cref="SocketOption"/> to get.</param>
        /// <returns>The <see cref="int"/> value of the specified option.</returns>
        internal int GetSocketOptionInt32(SocketOption option)
        {
            int value;

            if (this.socket.GetSocketOption(option, out value) == -1)
            {
                throw ZmqLibException.GetLastError();
            }

            return value;
        }

        /// <summary>
        /// Gets an option of the current socket as a long.
        /// </summary>
        /// <param name="option">The <see cref="SocketOption"/> to get.</param>
        /// <returns>The <see cref="long"/> value of the specified option.</returns>
        internal long GetSocketOptionInt64(SocketOption option)
        {
            long value;

            if (this.socket.GetSocketOption(option, out value) == -1)
            {
                throw ZmqLibException.GetLastError();
            }

            return value;
        }

        /// <summary>
        /// Gets an option of the current socket as an unsigned long.
        /// </summary>
        /// <param name="option">The <see cref="SocketOption"/> to get.</param>
        /// <returns>The <see cref="ulong"/> value of the specified option.</returns>
        internal ulong GetSocketOptionUInt64(SocketOption option)
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
        /// <param name="option">The <see cref="SocketOption"/> to get.</param>
        /// <returns>The <see cref="string"/> value of the specified option.</returns>
        internal string GetSocketOptionString(SocketOption option)
        {
            return this.GetSocketOptionBytes(option).ToZmqMessage();
        }

        /// <summary>
        /// Gets an option of the current socket as a byte array.
        /// </summary>
        /// <param name="option">The <see cref="SocketOption"/> to get.</param>
        /// <returns>The <see cref="byte"/> array value of the specified option.</returns>
        internal byte[] GetSocketOptionBytes(SocketOption option)
        {
            byte[] value;

            if (this.socket.GetSocketOption(option, out value) == -1)
            {
                throw ZmqLibException.GetLastError();
            }

            return value;
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Receive0"]/*'/>
        internal ReceivedMessage Receive(SocketFlags socketFlags)
        {
            byte[] buffer;

            int bytesReceived = this.socket.Receive((int)socketFlags, out buffer);

            if (bytesReceived >= 0)
            {
                return new ReceivedMessage(buffer, ReceiveResult.Received, this.ReceiveMore);
            }

            if (ZmqLibException.GetErrorCode() == ErrorCode.Eagain)
            {
                return ReceivedMessage.TryAgain;
            }

            throw ZmqLibException.GetLastError();
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Receive2"]/*'/>
        internal ReceivedMessage Receive(TimeSpan timeout)
        {
            if (timeout == TimeSpan.MaxValue)
            {
                return this.Receive(SocketFlags.None);
            }

            ReceivedMessage message;
            var timer = Stopwatch.StartNew();

            do
            {
                message = this.Receive(SocketFlags.DontWait);
            }
            while (timer.Elapsed < timeout && message.Result == ReceiveResult.TryAgain);

            return message;
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Send0"]/*'/>
        internal SendResult Send(byte[] buffer, SocketFlags socketFlags)
        {
            int bytesSent = this.socket.Send((int)socketFlags, buffer);

            if (bytesSent >= 0)
            {
                return SendResult.Sent;
            }

            if (ZmqLibException.GetErrorCode() == ErrorCode.Eagain)
            {
                return SendResult.TryAgain;
            }

            throw ZmqLibException.GetLastError();
        }

        /// <summary>
        /// Queue a single-part message (or the final message-part) to be sent by the socket in non-blocking mode.
        /// If the specified timeout elapses, <see cref="SendResult.TryAgain"/> is returned, indicating that the
        /// send operation should be attempted again.
        /// </summary>
        /// <param name="buffer">An array of type <see cref="byte"/> that contains the message to be sent.</param>
        /// <param name="socketFlags">A bitwise combination of the <see cref="SocketFlags"/> values.</param>
        /// <param name="timeout">A <see cref="TimeSpan"/> indicating the timeout value.</param>
        /// <returns>A <see cref="SendResult"/> value indicating the send operation outcome.</returns>
        /// <exception cref="ZmqLibException">An error occured during the execution of a native procedure.</exception>
        internal SendResult Send(byte[] buffer, SocketFlags socketFlags, TimeSpan timeout)
        {
            if (timeout == TimeSpan.MaxValue)
            {
                return this.Send(buffer, socketFlags & ~SocketFlags.DontWait);
            }

            socketFlags |= SocketFlags.DontWait;

            SendResult result;
            var timer = Stopwatch.StartNew();

            do
            {
                result = this.Send(buffer, socketFlags);
            }
            while (timer.Elapsed < timeout && result == SendResult.TryAgain);

            return result;
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="ZmqSocket"/>, and optionally disposes of the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.socket.Dispose();
            }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Subscribe"]/*'/>
        protected void Subscribe(byte[] prefix)
        {
            this.SetSocketOption(SocketOption.Subscribe, prefix);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Unsubscribe"]/*'/>
        protected void Unsubscribe(byte[] prefix)
        {
            this.SetSocketOption(SocketOption.Unsubscribe, prefix);
        }

        private static TimeSpan GetTimeSpan(int milliseconds)
        {
            return milliseconds == -1 ? TimeSpan.MaxValue : TimeSpan.FromMilliseconds(milliseconds);
        }

        private static int GetMilliseconds(TimeSpan timeSpan)
        {
            return timeSpan == TimeSpan.MaxValue ? -1 : (int)timeSpan.TotalMilliseconds;
        }
    }
}
