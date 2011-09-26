namespace ZeroMQ.Polling
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using ZeroMQ.Proxy;
    using ZeroMQ.Sockets;

    /// <summary>
    /// Multiplexes input/output events in a level-triggered fashion over a set of sockets.
    /// </summary>
    public sealed class ZmqPoller : IPoller
    {
        private readonly PollItem[] pollItems;
        private readonly IPollerProxy proxy;

        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZmqPoller"/> class.
        /// </summary>
        /// <param name="sockets">
        /// The set of <see cref="ZmqSocket"/>s to multiplex. Set <see cref="IReceiveSocket.ReceiveReady"/>
        /// and/or <see cref="ISendSocket.SendReady"/> as appropriate on each socket prior to calling Poll.
        /// </param>
        public ZmqPoller(IEnumerable<ZmqSocket> sockets)
        {
            if (sockets == null)
            {
                throw new ArgumentNullException("sockets");
            }

            this.pollItems = sockets.Select(s => new PollItem(s)).ToArray();
            this.proxy = ProxyFactory.CreatePoller(this.pollItems.Length);
        }

        /// <summary>
        /// Frees any unmanaged resources used by the Poller.
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
