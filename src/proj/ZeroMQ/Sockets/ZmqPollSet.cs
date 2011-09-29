namespace ZeroMQ.Sockets
{
    using System;
    using System.Diagnostics;

    using ZeroMQ.Proxy;

    /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="ZmqPollSet"]/*'/>
    public sealed class ZmqPollSet : IPollSet
    {
        private readonly IPollItem[] pollItems;
        private readonly IPollSetProxy proxy;

        private bool disposed;

        internal ZmqPollSet(IPollSetProxy proxy, IPollItem[] pollItems)
        {
            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }

            if (pollItems == null)
            {
                throw new ArgumentNullException("pollItems");
            }

            this.pollItems = pollItems;
            this.proxy = proxy;
        }

        private static bool WasInterrupted
        {
            get { return ZmqLibException.GetErrorCode() == ErrorCode.Eintr; }
        }

        private static bool WasTerminated
        {
            get { return ZmqLibException.GetErrorCode() == ErrorCode.Eterm; }
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="ZmqPollSet"/> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Poll1"]/*'/>
        public void Poll()
        {
            this.PollBlocking();
        }

        /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="Poll2"]/*'/>
        public void Poll(TimeSpan timeout)
        {
            if (timeout == TimeSpan.MaxValue)
            {
                this.PollBlocking();
            }
            else
            {
                this.PollNonBlocking(timeout);
            }
        }

        private static void ContinueIfInterrupted()
        {
            // An error value of EINTR indicates that the operation was interrupted
            // by delivery of a signal before any events were available. This is a recoverable
            // error, so try polling again for the remaining amount of time in the timeout.
            //
            // ETERM indicates that the context was terminated during the operation execution.
            // While it is not recoverable, it should not result in an exception being thrown.
            if (!WasInterrupted && !WasTerminated)
            {
                throw ZmqSocketException.GetLastError();
            }
        }

        private void PollBlocking()
        {
            while (this.Poll(-1) == -1)
            {
                ContinueIfInterrupted();
            }
        }

        private void PollNonBlocking(TimeSpan timeout)
        {
            int remainingTimeout = timeout.GetMilliseconds();
            var elapsed = Stopwatch.StartNew();

            do
            {
                int result = this.Poll(remainingTimeout);

                if (result >= 0 || WasTerminated)
                {
                    break;
                }

                ContinueIfInterrupted();
                remainingTimeout -= (int)elapsed.ElapsedMilliseconds;
            }
            while (remainingTimeout >= 0);
        }

        private int Poll(int timeoutMilliseconds)
        {
            this.EnsureNotDisposed();

            int readyCount = this.proxy.Poll(this.pollItems, timeoutMilliseconds);

            if (readyCount > 0)
            {
                foreach (PollItem pollItem in this.pollItems)
                {
                    pollItem.InvokeEvents();
                }
            }

            return readyCount;
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !this.disposed)
            {
                this.proxy.Dispose();
            }

            this.disposed = true;
        }

        private void EnsureNotDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("ZmqPollSet", "The current ZmqPollSet has already been disposed and cannot be reused.");
            }
        }
    }
}
