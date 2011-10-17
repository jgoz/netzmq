namespace ZeroMQ.UnitTests.ZmqPollSetSpecs
{
    using System;

    using Machine.Specifications;

    using Moq;

    using ZeroMQ.Proxy;
    using ZeroMQ.Sockets;

    using It = Machine.Specifications.It;

    [Subject(typeof(ZmqPollSet))]
    class when_polling_in_blocking_mode : using_mock_pollset_proxy
    {
        Establish context = () =>
        {
            pollsetProxy.Setup(mock => mock.Poll(Moq.It.IsAny<IPollItem[]>(), Moq.It.IsAny<int>())).Returns(1);
            inItem.Setup(mock => mock.REvents).Returns(PollFlags.PollIn);

            Initialize(new[] { inItem.Object, outItem.Object });
        };

        Because of = () =>
            RunWithTimeout(() => pollset.Poll(), 2000);

        It should_pass_poll_items_to_underlying_poll_method = () =>
            pollsetProxy.Verify(mock => mock.Poll(new[] { inItem.Object, outItem.Object }, Moq.It.IsAny<int>()));

        It should_use_infinite_timeout_value = () =>
            pollsetProxy.Verify(mock => mock.Poll(Moq.It.IsAny<IPollItem[]>(), -1));

        It should_invoke_events_on_ready_sockets = () =>
            inItem.Verify(mock => mock.InvokeEvents());

        It should_not_invoke_events_on_nonready_sockets = () =>
            outItem.Verify(mock => mock.InvokeEvents(), Times.Never());
    }

    [Subject(typeof(ZmqPollSet))]
    class when_polling_in_blocking_mode_and_thread_is_interrupted : using_mock_pollset_proxy
    {
        static Exception exception;

        Establish context = () =>
        {
            pollsetProxy.SetupSequence(mock => mock.Poll(Moq.It.IsAny<IPollItem[]>(), Moq.It.IsAny<int>()))
                .Returns(-1)
                .Returns(1);
            errorProviderProxy.Setup(mock => mock.GetErrorCode()).Returns((int)ErrorCode.Eintr);

            inItem.Setup(mock => mock.REvents).Returns(PollFlags.PollIn);

            Initialize(new[] { inItem.Object, outItem.Object });
        };

        Because of = () =>
            exception = RunWithTimeout(() => pollset.Poll(), 2000);

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_invoke_events_on_ready_sockets = () =>
            inItem.Verify(mock => mock.InvokeEvents());

        It should_not_invoke_events_on_nonready_sockets = () =>
            outItem.Verify(mock => mock.InvokeEvents(), Times.Never());
    }

    [Subject(typeof(ZmqPollSet))]
    class when_polling_in_blocking_mode_and_context_is_terminated : using_mock_pollset_proxy
    {
        static Exception exception;

        Establish context = () =>
        {
            pollsetProxy.Setup(mock => mock.Poll(Moq.It.IsAny<IPollItem[]>(), Moq.It.IsAny<int>())).Returns(-1);
            errorProviderProxy.Setup(mock => mock.GetErrorCode()).Returns((int)ErrorCode.Eterm);

            inItem.Setup(mock => mock.REvents).Returns(PollFlags.PollIn);

            Initialize(new[] { inItem.Object, outItem.Object });
        };

        Because of = () =>
            exception = RunWithTimeout(() => pollset.Poll(), 2000);

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_not_invoke_events_on_ready_sockets = () =>
            inItem.Verify(mock => mock.InvokeEvents(), Times.Never());

        It should_not_invoke_events_on_nonready_sockets = () =>
            outItem.Verify(mock => mock.InvokeEvents(), Times.Never());
    }

    [Subject(typeof(ZmqPollSet))]
    class when_polling_in_blocking_mode_and_a_fatal_error_occurs : using_mock_pollset_proxy
    {
        static Exception exception;

        Establish context = () =>
        {
            pollsetProxy.Setup(mock => mock.Poll(Moq.It.IsAny<IPollItem[]>(), Moq.It.IsAny<int>())).Returns(-1);
            errorProviderProxy.Setup(mock => mock.GetErrorCode()).Returns((int)ErrorCode.Efault);

            Initialize(new[] { inItem.Object, outItem.Object });
        };

        Because of = () =>
            exception = RunWithTimeout(() => pollset.Poll(), 2000);

        It should_not_fail = () =>
            exception.ShouldBeOfType<ZmqSocketException>();

        It should_not_invoke_events_on_ready_sockets = () =>
            inItem.Verify(mock => mock.InvokeEvents(), Times.Never());

        It should_not_invoke_events_on_nonready_sockets = () =>
            outItem.Verify(mock => mock.InvokeEvents(), Times.Never());
    }
}
