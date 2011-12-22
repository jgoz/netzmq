namespace ZeroMQ.Sockets
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    using ZeroMQ.Proxy;

    /// <summary>
    /// Sends and receives messages across various transports, synchronously or asynchronously.
    /// </summary>
    /// <remarks>
    /// The <see cref="ZmqSocket"/> class defines the common behavior for derived socket types. 
    /// </remarks>
    public class ZmqSocket : ISocket
    {
        /// <summary>
        /// An byte array containing no data.
        /// </summary>
        public static readonly byte[] EmptyMessage = new byte[0];

        private static readonly int ProcessorCount = Environment.ProcessorCount;

        private readonly ISocketProxy proxy;
        private readonly ZmqErrorProvider errorProvider;

        private bool disposed;
        private bool closed;

        internal ZmqSocket(ISocketProxy socketProxy, IErrorProviderProxy errorProviderProxy)
        {
            if (socketProxy == null)
            {
                throw new ArgumentNullException("socketProxy");
            }

            if (errorProviderProxy == null)
            {
                throw new ArgumentNullException("errorProviderProxy");
            }

            this.proxy = socketProxy;
            this.errorProvider = new ZmqErrorProvider(errorProviderProxy);
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
            get { return TimeSpan.FromMilliseconds(this.GetSocketOptionInt32(SocketOption.Linger)); }
            set { this.SetSocketOption(SocketOption.Linger, (int)value.TotalMilliseconds); }
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
            get { return TimeSpan.FromMilliseconds(this.GetSocketOptionInt32(SocketOption.RecoveryIvl)); }
            set { this.SetSocketOption(SocketOption.RecoveryIvl, (int)value.TotalMilliseconds); }
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
            get { return TimeSpan.FromMilliseconds(this.GetSocketOptionInt32(SocketOption.RcvTimeo)); }
            set { this.SetSocketOption(SocketOption.RcvTimeo, (int)value.TotalMilliseconds); }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="ReconnectInterval"]/*'/>
        public TimeSpan ReconnectInterval
        {
            get { return TimeSpan.FromMilliseconds(this.GetSocketOptionInt32(SocketOption.ReconnectIvl)); }
            set { this.SetSocketOption(SocketOption.ReconnectIvl, (int)value.TotalMilliseconds); }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="ReconnectIntervalMax"]/*'/>
        public TimeSpan ReconnectIntervalMax
        {
            get { return TimeSpan.FromMilliseconds(this.GetSocketOptionInt32(SocketOption.ReconnectIvlMax)); }
            set { this.SetSocketOption(SocketOption.ReconnectIvlMax, (int)value.TotalMilliseconds); }
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
            get { return TimeSpan.FromMilliseconds(this.GetSocketOptionInt32(SocketOption.SndTimeo)); }
            set { this.SetSocketOption(SocketOption.SndTimeo, (int)value.TotalMilliseconds); }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="SendTimeout"]/*'/>
        public ProtocolType SupportedProtocol
        {
            get { return (ProtocolType)this.GetSocketOptionInt32(SocketOption.Ipv4Only); }
            set { this.SetSocketOption(SocketOption.Ipv4Only, (int)value); }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="ReceiveStatus"]/*'/>
        public ReceiveResult ReceiveStatus { get; private set; }

        internal IntPtr Handle
        {
            get { return this.proxy.Handle; }
        }

        private bool ShouldTryAgain
        {
            get { return this.errorProvider.ShouldTryAgain; }
        }

        private bool ContextWasTerminated
        {
            get { return this.errorProvider.ContextWasTerminated; }
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Bind"]/*'/>
        public void Bind(string endpoint)
        {
            this.EnsureNotDisposed();

            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }

            if (endpoint == string.Empty)
            {
                throw new ArgumentException("Unable to Bind to an empty endpoint.", "endpoint");
            }

            this.HandleProxyResult(this.proxy.Bind(endpoint));
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Connect"]/*'/>
        public void Connect(string endpoint)
        {
            this.EnsureNotDisposed();

            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }

            if (endpoint == string.Empty)
            {
                throw new ArgumentException("Unable to Connect to an empty endpoint.", "endpoint");
            }

            this.HandleProxyResult(this.proxy.Connect(endpoint));
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Close"]/*'/>
        public void Close()
        {
            if (this.disposed || this.closed)
            {
                return;
            }

            this.HandleProxyResult(this.proxy.Close());

            this.closed = true;
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="ZmqSocket"/> class.
        /// </summary>
        public virtual void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal void SetSocketOption(SocketOption option, int value)
        {
            this.EnsureNotDisposed();

            this.HandleProxyResult(this.proxy.SetSocketOption((int)option, value));
        }

        internal void SetSocketOption(SocketOption option, long value)
        {
            this.EnsureNotDisposed();

            this.HandleProxyResult(this.proxy.SetSocketOption((int)option, value));
        }

        internal void SetSocketOption(SocketOption option, ulong value)
        {
            this.EnsureNotDisposed();

            this.HandleProxyResult(this.proxy.SetSocketOption((int)option, value));
        }

        internal void SetSocketOption(SocketOption option, byte[] value)
        {
            this.EnsureNotDisposed();

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            this.HandleProxyResult(this.proxy.SetSocketOption((int)option, value));
        }

        internal int GetSocketOptionInt32(SocketOption option)
        {
            this.EnsureNotDisposed();

            int value;

            this.HandleProxyResult(this.proxy.GetSocketOption((int)option, out value));

            return value;
        }

        internal long GetSocketOptionInt64(SocketOption option)
        {
            this.EnsureNotDisposed();

            long value;

            this.HandleProxyResult(this.proxy.GetSocketOption((int)option, out value));

            return value;
        }

        internal ulong GetSocketOptionUInt64(SocketOption option)
        {
            this.EnsureNotDisposed();

            ulong value;

            this.HandleProxyResult(this.proxy.GetSocketOption((int)option, out value));

            return value;
        }

        internal byte[] GetSocketOptionBytes(SocketOption option)
        {
            this.EnsureNotDisposed();

            byte[] value;

            this.HandleProxyResult(this.proxy.GetSocketOption((int)option, out value));

            return value;
        }

        internal byte[] Receive(SocketFlags socketFlags)
        {
            this.EnsureNotDisposed();

            byte[] buffer;
            int bytesReceived = this.proxy.Receive((int)socketFlags, out buffer);

            if (bytesReceived >= 0)
            {
                this.ReceiveStatus = ReceiveResult.Received;
                return buffer;
            }

            if (this.ShouldTryAgain)
            {
                this.ReceiveStatus = ReceiveResult.TryAgain;
                return EmptyMessage;
            }

            if (this.ContextWasTerminated)
            {
                this.ReceiveStatus = ReceiveResult.Interrupted;
                return EmptyMessage;
            }

            throw this.errorProvider.GetLastSocketError();
        }

        internal byte[] Receive(TimeSpan timeout)
        {
            if (timeout == TimeSpan.MaxValue)
            {
                return this.Receive(SocketFlags.None);
            }

            int iterations = 0;
            byte[] message;

            var timeoutMilliseconds = (int)timeout.TotalMilliseconds;
            var timer = Stopwatch.StartNew();

            do
            {
                message = this.Receive(SocketFlags.DontWait);

                if (this.ReceiveStatus != ReceiveResult.TryAgain || timeoutMilliseconds <= 1)
                {
                    break;
                }

                if (iterations < 20 && ProcessorCount > 1)
                {
                    // If we have a short wait (< 20 iterations) we
                    // SpinWait to allow other threads on HT CPUs
                    // to use the CPU. The more CPUs we have
                    // the longer it's acceptable to SpinWait since
                    // we stall the overall system less.
                    Thread.SpinWait(100 * ProcessorCount);
                }
                else
                {
                    Thread.Yield();
                }

                ++iterations;
            }
            while (timer.Elapsed < timeout);

            return message;
        }

        internal SendResult Send(byte[] buffer, SocketFlags socketFlags)
        {
            this.EnsureNotDisposed();

            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            int bytesSent = this.proxy.Send((int)socketFlags, buffer);

            if (bytesSent >= 0)
            {
                return SendResult.Sent;
            }

            if (this.ShouldTryAgain)
            {
                return SendResult.TryAgain;
            }

            if (this.ContextWasTerminated)
            {
                return SendResult.Interrupted;
            }

            throw this.errorProvider.GetLastSocketError();
        }

        internal SendResult Send(byte[] buffer, SocketFlags socketFlags, TimeSpan timeout)
        {
            if (timeout == TimeSpan.MaxValue)
            {
                return this.Send(buffer, socketFlags & ~SocketFlags.DontWait);
            }

            socketFlags |= SocketFlags.DontWait;

            int iterations = 0;
            SendResult result;

            var timeoutMilliseconds = (int)timeout.TotalMilliseconds;
            var timer = Stopwatch.StartNew();

            do
            {
                result = this.Send(buffer, socketFlags);

                if (result != SendResult.TryAgain || timeoutMilliseconds <= 1)
                {
                    break;
                }

                if (iterations < 20 && ProcessorCount > 1)
                {
                    Thread.SpinWait(100 * ProcessorCount);
                }
                else
                {
                    Thread.Yield();
                }

                ++iterations;
            }
            while (timer.Elapsed < timeout);

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
            if (disposing && !this.disposed)
            {
                this.Close();
            }

            this.disposed = true;
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

        private void HandleProxyResult(int result)
        {
            // Context termination (ETERM) is an allowable error state, occurring when the
            // ZmqContext was terminated during a socket method.
            if (result == -1 && !this.ContextWasTerminated)
            {
                throw this.errorProvider.GetLastSocketError();
            }
        }

        private void EnsureNotDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("ZmqSocket", "The current ZmqSocket has already been disposed and cannot be reused.");
            }
        }
    }
}
