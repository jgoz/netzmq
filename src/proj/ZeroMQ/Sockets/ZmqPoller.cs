namespace ZeroMQ.Sockets
{
    using System;
    using System.Diagnostics;

    using ZeroMQ.Proxy;

    internal class ZmqPoller : IPoller
    {
        private readonly IPollItem[] pollItems;
        private readonly IPollerProxy proxy;

        private bool disposed;

        public ZmqPoller(IPollerProxy proxy, IPollItem[] pollItems)
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Poll()
        {
            this.PollBlocking();
        }

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

        private static void CheckErrorCode()
        {
            // An error value of EINTR indicates that the operation was interrupted
            // by delivery of a signal before any events were available. This is a recoverable
            // error, so try polling again for the remaining amount of time in the timeout.
            if (ZmqLibException.GetErrorCode() != ErrorCode.Eintr)
            {
                throw ZmqLibException.GetLastError();
            }
        }

        private void PollBlocking()
        {
            while (this.Poll(-1) == -1)
            {
                CheckErrorCode();
            }
        }

        private void PollNonBlocking(TimeSpan timeout)
        {
            int remainingTimeout = timeout.GetMilliseconds();
            var elapsed = Stopwatch.StartNew();

            do
            {
                int result = this.Poll(remainingTimeout);

                if (result >= 0)
                {
                    break;
                }

                CheckErrorCode();
                remainingTimeout -= (int)elapsed.ElapsedMilliseconds;
            }
            while (remainingTimeout >= 0);
        }

        private int Poll(int timeoutMilliseconds)
        {
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
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.proxy.Dispose();
            }

            this.disposed = true;
        }
    }
}
