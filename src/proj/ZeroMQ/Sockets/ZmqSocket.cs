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
    public abstract class ZmqSocket : ISocket
    {
        private readonly ISocketProxy proxy;

        internal ZmqSocket(ISocketProxy proxy)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }

            this.proxy = proxy;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ZmqSocket"/> class.
        /// </summary>
        ~ZmqSocket()
        {
            this.Dispose(false);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="ReceiveReady"]/*'/>
        protected event EventHandler<ReceiveReadyEventArgs> ReceiveReady;

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="SendReady"]/*'/>
        protected event EventHandler<SendReadyEventArgs> SendReady;

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Affinity"]/*'/>
        public ulong Affinity
        {
            get { return this.GetSocketOptionUInt64(SocketOption.Affinity); }
            set { this.SetSocketOption(SocketOption.Affinity, value); }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Backlog"]/*'/>
        public int Backlog
        {
            get { return this.GetSocketOptionInt32(SocketOption.Backlog); }
            set { this.SetSocketOption(SocketOption.Backlog, value); }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Identity"]/*'/>
        public byte[] Identity
        {
            get { return this.GetSocketOptionBytes(SocketOption.Identity); }
            set { this.SetSocketOption(SocketOption.Identity, value); }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Linger"]/*'/>
        public TimeSpan Linger
        {
            get { return this.GetSocketOptionInt32(SocketOption.Linger).GetTimeSpan(); }
            set { this.SetSocketOption(SocketOption.Linger, value.GetMilliseconds()); }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="MaxMessageSize"]/*'/>
        public long MaxMessageSize
        {
            get { return this.GetSocketOptionInt64(SocketOption.MaxMsgSize); }
            set { this.SetSocketOption(SocketOption.MaxMsgSize, value); }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="MulticastHops"]/*'/>
        public int MulticastHops
        {
            get { return this.GetSocketOptionInt32(SocketOption.MulticastHops); }
            set { this.SetSocketOption(SocketOption.MulticastHops, value); }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="MulticastRate"]/*'/>
        public int MulticastRate
        {
            get { return this.GetSocketOptionInt32(SocketOption.Rate); }
            set { this.SetSocketOption(SocketOption.Rate, value); }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="MulticastRecoveryInterval"]/*'/>
        public TimeSpan MulticastRecoveryInterval
        {
            get { return this.GetSocketOptionInt32(SocketOption.RecoveryIvl).GetTimeSpan(); }
            set { this.SetSocketOption(SocketOption.RecoveryIvl, value.GetMilliseconds()); }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="ReceiveBufferSize"]/*'/>
        public int ReceiveBufferSize
        {
            get { return this.GetSocketOptionInt32(SocketOption.RcvBuf); }
            set { this.SetSocketOption(SocketOption.RcvBuf, value); }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="ReceiveHighWatermark"]/*'/>
        public int ReceiveHighWatermark
        {
            get { return this.GetSocketOptionInt32(SocketOption.RcvHwm); }
            set { this.SetSocketOption(SocketOption.RcvHwm, value); }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="ReceiveMore"]/*'/>
        public bool ReceiveMore
        {
            get { return this.GetSocketOptionInt32(SocketOption.RcvMore) == 1; }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="ReceiveTimeout"]/*'/>
        public TimeSpan ReceiveTimeout
        {
            get { return this.GetSocketOptionInt32(SocketOption.RcvTimeo).GetTimeSpan(); }
            set { this.SetSocketOption(SocketOption.RcvTimeo, value.GetMilliseconds()); }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="ReconnectInterval"]/*'/>
        public TimeSpan ReconnectInterval
        {
            get { return this.GetSocketOptionInt32(SocketOption.ReconnectIvl).GetTimeSpan(); }
            set { this.SetSocketOption(SocketOption.ReconnectIvl, value.GetMilliseconds()); }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="ReconnectIntervalMax"]/*'/>
        public TimeSpan ReconnectIntervalMax
        {
            get { return this.GetSocketOptionInt32(SocketOption.ReconnectIvlMax).GetTimeSpan(); }
            set { this.SetSocketOption(SocketOption.ReconnectIvlMax, value.GetMilliseconds()); }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="SendBufferSize"]/*'/>
        public int SendBufferSize
        {
            get { return this.GetSocketOptionInt32(SocketOption.SndBuf); }
            set { this.SetSocketOption(SocketOption.SndBuf, value); }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="SendHighWatermark"]/*'/>
        public int SendHighWatermark
        {
            get { return this.GetSocketOptionInt32(SocketOption.SndHwm); }
            set { this.SetSocketOption(SocketOption.SndHwm, value); }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="SendTimeout"]/*'/>
        public TimeSpan SendTimeout
        {
            get { return this.GetSocketOptionInt32(SocketOption.SndTimeo).GetTimeSpan(); }
            set { this.SetSocketOption(SocketOption.SndTimeo, value.GetMilliseconds()); }
        }

        internal IntPtr Handle
        {
            get { return this.proxy.Handle; }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Bind"]/*'/>
        public void Bind(string endpoint)
        {
            if (this.proxy.Bind(endpoint) == -1)
            {
                throw ZmqSocketException.GetLastError();
            }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Connect"]/*'/>
        public void Connect(string endpoint)
        {
            if (this.proxy.Connect(endpoint) == -1)
            {
                throw ZmqSocketException.GetLastError();
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
            if (this.proxy.SetSocketOption(option, value) == -1)
            {
                throw ZmqSocketException.GetLastError();
            }
        }

        /// <summary>
        /// Sets an option on the current socket to a long value.
        /// </summary>
        /// <param name="option">The <see cref="SocketOption"/> to set.</param>
        /// <param name="value">The <see cref="long"/> value to set.</param>
        internal void SetSocketOption(SocketOption option, long value)
        {
            if (this.proxy.SetSocketOption(option, value) == -1)
            {
                throw ZmqSocketException.GetLastError();
            }
        }

        /// <summary>
        /// Sets an option on the current socket to an unsigned long value.
        /// </summary>
        /// <param name="option">The <see cref="SocketOption"/> to set.</param>
        /// <param name="value">The <see cref="ulong"/> value to set.</param>
        internal void SetSocketOption(SocketOption option, ulong value)
        {
            if (this.proxy.SetSocketOption(option, value) == -1)
            {
                throw ZmqSocketException.GetLastError();
            }
        }

        /// <summary>
        /// Sets an option on the current socket to a byte array value.
        /// </summary>
        /// <param name="option">The <see cref="SocketOption"/> to set.</param>
        /// <param name="value">The <see cref="byte"/> array value to set.</param>
        internal void SetSocketOption(SocketOption option, byte[] value)
        {
            if (this.proxy.SetSocketOption(option, value) == -1)
            {
                throw ZmqSocketException.GetLastError();
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

            if (this.proxy.GetSocketOption(option, out value) == -1)
            {
                throw ZmqSocketException.GetLastError();
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

            if (this.proxy.GetSocketOption(option, out value) == -1)
            {
                throw ZmqSocketException.GetLastError();
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

            if (this.proxy.GetSocketOption(option, out value) == -1)
            {
                throw ZmqSocketException.GetLastError();
            }

            return value;
        }

        /// <summary>
        /// Gets an option of the current socket as a byte array.
        /// </summary>
        /// <param name="option">The <see cref="SocketOption"/> to get.</param>
        /// <returns>The <see cref="byte"/> array value of the specified option.</returns>
        internal byte[] GetSocketOptionBytes(SocketOption option)
        {
            byte[] value;

            if (this.proxy.GetSocketOption(option, out value) == -1)
            {
                throw ZmqSocketException.GetLastError();
            }

            return value;
        }

        internal ReceivedMessage Receive(SocketFlags socketFlags)
        {
            byte[] buffer;

            int bytesReceived = this.proxy.Receive((int)socketFlags, out buffer);

            if (bytesReceived >= 0)
            {
                return new ReceivedMessage(buffer, ReceiveResult.Received, this.ReceiveMore);
            }

            if (ZmqLibException.GetErrorCode() == ErrorCode.Eagain)
            {
                return ReceivedMessage.TryAgain;
            }

            throw ZmqSocketException.GetLastError();
        }

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

        internal SendResult Send(byte[] buffer, SocketFlags socketFlags)
        {
            int bytesSent = this.proxy.Send((int)socketFlags, buffer);

            if (bytesSent >= 0)
            {
                return SendResult.Sent;
            }

            if (ZmqLibException.GetErrorCode() == ErrorCode.Eagain)
            {
                return SendResult.TryAgain;
            }

            throw ZmqSocketException.GetLastError();
        }

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

        internal void InvokeReceiveReady(ReceiveReadyEventArgs e)
        {
            EventHandler<ReceiveReadyEventArgs> handler = this.ReceiveReady;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        internal void InvokeSendReady(SendReadyEventArgs e)
        {
            EventHandler<SendReadyEventArgs> handler = this.SendReady;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="ZmqSocket"/>, and optionally disposes of the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.proxy.Dispose();
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
    }
}
