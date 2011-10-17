namespace ZeroMQ.UnitTests.ZmqPollSetSpecs
{
    using System;
    using System.Threading;

    using Machine.Specifications;

    using Moq;

    using ZeroMQ.Proxy;
    using ZeroMQ.Sockets;

    using It = Machine.Specifications.It;

    [Subject(typeof(ZmqPollSet))]
    class when_polling_in_nonblocking_mode : using_mock_pollset_proxy
    {
        Establish context = () =>
        {
            pollsetProxy.Setup(mock => mock.Poll(Moq.It.IsAny<IPollItem[]>(), Moq.It.IsAny<int>())).Returns(1);
            inItem.Setup(mock => mock.REvents).Returns(PollFlags.PollIn);

            Initialize(new[] { inItem.Object, outItem.Object });
        };

        Because of = () =>
            pollset.Poll(TimeSpan.FromMilliseconds(2000));

        It should_pass_poll_items_to_underlying_poll_method = () =>
            pollsetProxy.Verify(mock => mock.Poll(new[] { inItem.Object, outItem.Object }, Moq.It.IsAny<int>()));

        It should_use_specified_timeout_value = () =>
            pollsetProxy.Verify(mock => mock.Poll(Moq.It.IsAny<IPollItem[]>(), 2000));

        It should_invoke_events_on_ready_sockets = () =>
            inItem.Verify(mock => mock.InvokeEvents());

        It should_not_invoke_events_on_nonready_sockets = () =>
            outItem.Verify(mock => mock.InvokeEvents(), Times.Never());
    }

    [Subject(typeof(ZmqPollSet))]
    class when_polling_in_nonblocking_mode_and_thread_is_interrupted : using_mock_pollset_proxy
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
            exception = Catch.Exception(() => pollset.Poll(TimeSpan.FromMilliseconds(2000)));

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_invoke_events_on_ready_sockets = () =>
            inItem.Verify(mock => mock.InvokeEvents());

        It should_not_invoke_events_on_nonready_sockets = () =>
            outItem.Verify(mock => mock.InvokeEvents(), Times.Never());
    }

    [Subject(typeof(ZmqPollSet))]
    class when_polling_in_nonblocking_mode_and_timeout_expires : using_mock_pollset_proxy
    {
        static Exception exception;

        Establish context = () =>
        {
            pollsetProxy.Setup(mock => mock.Poll(Moq.It.IsAny<IPollItem[]>(), Moq.It.IsAny<int>()))
                .Callback(() => Thread.Sleep(500))
                .Returns(-1);
            errorProviderProxy.Setup(mock => mock.GetErrorCode()).Returns((int)ErrorCode.Eintr);

            Initialize(new[] { inItem.Object, outItem.Object });
        };

        Because of = () =>
            exception = Catch.Exception(() => pollset.Poll(TimeSpan.FromMilliseconds(2000)));

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_only_run_for_allotted_time = () =>
            pollsetProxy.Verify(mock => mock.Poll(Moq.It.IsAny<IPollItem[]>(), Moq.It.IsAny<int>()), Times.Between(3, 4, Range.Inclusive));

        It should_not_invoke_events_on_input_sockets = () =>
            inItem.Verify(mock => mock.InvokeEvents(), Times.Never());

        It should_not_invoke_events_on_output_sockets = () =>
            outItem.Verify(mock => mock.InvokeEvents(), Times.Never());
    }

    [Subject(typeof(ZmqPollSet))]
    class when_polling_in_nonblocking_mode_and_context_is_terminated : using_mock_pollset_proxy
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
            exception = Catch.Exception(() => pollset.Poll(TimeSpan.FromMilliseconds(2000)));

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_not_invoke_events_on_ready_sockets = () =>
            inItem.Verify(mock => mock.InvokeEvents(), Times.Never());

        It should_not_invoke_events_on_nonready_sockets = () =>
            outItem.Verify(mock => mock.InvokeEvents(), Times.Never());
    }

    [Subject(typeof(ZmqPollSet))]
    class when_polling_in_nonblocking_mode_and_a_fatal_error_occurs : using_mock_pollset_proxy
    {
        static Exception exception;

        Establish context = () =>
        {
            pollsetProxy.Setup(mock => mock.Poll(Moq.It.IsAny<IPollItem[]>(), Moq.It.IsAny<int>())).Returns(-1);
            errorProviderProxy.Setup(mock => mock.GetErrorCode()).Returns((int)ErrorCode.Efault);

            Initialize(new[] { inItem.Object, outItem.Object });
        };

        Because of = () =>
            exception = Catch.Exception(() => pollset.Poll(TimeSpan.FromMilliseconds(2000)));

        It should_not_fail = () =>
            exception.ShouldBeOfType<ZmqSocketException>();

        It should_not_invoke_events_on_ready_sockets = () =>
            inItem.Verify(mock => mock.InvokeEvents(), Times.Never());

        It should_not_invoke_events_on_nonready_sockets = () =>
            outItem.Verify(mock => mock.InvokeEvents(), Times.Never());
    }
}
