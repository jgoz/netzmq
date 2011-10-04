namespace ZeroMQ.Sockets
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using ZeroMQ.Proxy;

    /// <include file='..\CommonDoc.xml' path='ZeroMQ/Members[@name="ZmqPollSet"]/*'/>
    public sealed class ZmqPollSet : IPollSet
    {
        private readonly IPollItem[] pollItems;
        private readonly IPollSetProxy proxy;
        private readonly ZmqErrorProvider errorProvider;

        private bool disposed;

        internal ZmqPollSet(IEnumerable<IPollItem> pollItems, IPollSetProxy proxy, IErrorProviderProxy errorProviderProxy)
        {
            if (pollItems == null)
            {
                throw new ArgumentNullException("pollItems");
            }

            if (!pollItems.Any())
            {
                throw new ArgumentException("At least one poll item is required.", "pollItems");
            }

            if (proxy == null)
            {
                throw new ArgumentNullException("proxy");
            }

            if (errorProviderProxy == null)
            {
                throw new ArgumentNullException("errorProviderProxy");
            }

            this.pollItems = pollItems.ToArray();
            this.proxy = proxy;
            this.errorProvider = new ZmqErrorProvider(errorProviderProxy);
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

        private void PollBlocking()
        {
            while (this.Poll(-1) == -1 && !this.errorProvider.ContextWasTerminated)
            {
                this.ContinueIfInterrupted();
            }
        }

        private void PollNonBlocking(TimeSpan timeout)
        {
            int remainingTimeout = timeout.GetMilliseconds();
            var elapsed = Stopwatch.StartNew();

            do
            {
                int result = this.Poll(remainingTimeout);

                if (result >= 0 || this.errorProvider.ContextWasTerminated)
                {
                    break;
                }

                this.ContinueIfInterrupted();
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
                foreach (IPollItem pollItem in this.pollItems.Where(item => item.REvents != PollFlags.None))
                {
                    pollItem.InvokeEvents();
                }
            }

            return readyCount;
        }

        private void ContinueIfInterrupted()
        {
            // An error value of EINTR indicates that the operation was interrupted
            // by delivery of a signal before any events were available. This is a recoverable
            // error, so try polling again for the remaining amount of time in the timeout.
            if (!this.errorProvider.ThreadWasInterrupted)
            {
                throw this.errorProvider.GetLastSocketError();
            }
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
