namespace ZeroMQ.UnitTests.ZmqPollSetSpecs
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using Machine.Specifications;

    using Moq;

    using ZeroMQ.Proxy;
    using ZeroMQ.Sockets;

    abstract class using_mock_pollset_proxy
    {
        protected static Mock<IPollItem> inItem;
        protected static Mock<IPollItem> outItem;

        protected static Mock<IPollSetProxy> pollsetProxy;
        protected static Mock<IErrorProviderProxy> errorProviderProxy;
        protected static ZmqPollSet pollset;

        Establish context = () =>
        {
            pollsetProxy = new Mock<IPollSetProxy>();
            errorProviderProxy = new Mock<IErrorProviderProxy>();

            inItem = CreatePollItemMock(PollFlags.PollIn);
            outItem = CreatePollItemMock(PollFlags.PollOut);
        };

        protected static void Initialize(IEnumerable<IPollItem> pollItems)
        {
            pollset = new ZmqPollSet(pollItems, pollsetProxy.Object, errorProviderProxy.Object);
        }

        protected static Mock<IPollItem> CreatePollItemMock(PollFlags flags)
        {
            var item = new Mock<IPollItem>();

            item.SetupGet(mock => mock.Events).Returns(flags);
            item.SetupGet(mock => mock.Socket).Returns(new IntPtr());

            return item;
        }

        protected static Exception RunWithTimeout(Action action, int millisecondTimeout)
        {
            Exception caughtException = null;
            Thread threadToKill = null;

            Action wrappedAction = () =>
            {
                threadToKill = Thread.CurrentThread;
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }
            };

            IAsyncResult result = wrappedAction.BeginInvoke(null, null);

            if (result.AsyncWaitHandle.WaitOne(millisecondTimeout))
            {
                wrappedAction.EndInvoke(result);
            }
            else
            {
                threadToKill.Abort();
                throw new TimeoutException();
            }

            return caughtException;
        }
    }
}
