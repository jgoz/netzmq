namespace ZeroMQ.UnitTests.ZmqSocketSpecs
{
    using System;

    using Machine.Specifications;

    using Moq;

    using ZeroMQ.Proxy;
    using ZeroMQ.Sockets;

    using It = Machine.Specifications.It;

    [Subject(typeof(ReceiveSocket), "blocking")]
    class when_receiving_a_message_from_a_remote_socket : using_receive_socket
    {
        Establish context = () =>
            socketProxy.Setup(mock => mock.Receive(Moq.It.IsAny<int>(), out message)).Returns(message.Length);

        Because of = () =>
            result = socket.Receive();

        Behaves_like<SuccessfulReceiveResult> receive_success;

        It should_use_non_blocking_mode = () =>
            socketProxy.Verify(mock => mock.Receive(ExcludesSocketFlag(SocketFlags.DontWait), out message));
    }

    [Subject(typeof(ReceiveSocket), "blocking")]
    class when_receiving_a_final_message_part_from_a_remote_socket : using_receive_socket
    {
        Establish context = () =>
        {
            int receiveMore;

            socketProxy.Setup(mock => mock.Receive(Moq.It.IsAny<int>(), out message)).Returns(message.Length);
            socketProxy.Setup(mock => mock.GetSocketOption((int)SocketOption.RcvMore, out receiveMore)).Returns(0);
        };

        Because of = () =>
            result = socket.Receive();

        Behaves_like<NoReceiveMore> will_not_receive_more_parts;
    }

    [Subject(typeof(ReceiveSocket), "blocking")]
    class when_receiving_a_message_is_interrupted_by_context_termination : using_receive_socket_with_context_termination
    {
        Because of = () =>
            exception = Catch.Exception(() => result = socket.Receive());

        Behaves_like<InterruptedReceiveResult> interrupted;
    }

    [Subject(typeof(ReceiveSocket), "blocking")]
    class when_receiving_a_message_and_the_proxy_returns_an_error : using_receive_socket_with_socket_error
    {
        Because of = () =>
            exception = Catch.Exception(() => result = socket.Receive());

        Behaves_like<FailsWithSocketException> fails_with_exception;
    }

    [Subject(typeof(ReceiveSocket), "blocking")]
    class when_receiving_a_multi_part_message_from_a_remote_socket : using_receive_socket
    {
        protected static int receiveMore = 1;

        Establish context = () =>
        {
            socketProxy.Setup(mock => mock.Receive(Moq.It.IsAny<int>(), out message)).Returns(message.Length);
            socketProxy.Setup(mock => mock.GetSocketOption((int)SocketOption.RcvMore, out receiveMore)).Returns(0);
        };

        Because of = () =>
            result = socket.Receive();

        Behaves_like<SuccessfulReceiveResult> receive_success;
        Behaves_like<ReceiveMore> will_receive_more_parts;
    }

    [Subject(typeof(ReceiveSocket), "non-blocking")]
    class when_receiving_a_message_from_a_remote_socket_with_timeout : using_receive_socket
    {
        Establish context = () =>
        {
            socketProxy.SetupSequence(mock => mock.Receive(Moq.It.IsAny<int>(), out message))
                .Returns(-1)
                .Returns(message.Length);

            errorProviderProxy.Setup(mock => mock.GetErrorCode()).Returns((int)ErrorCode.Eagain);
        };

        Because of = () =>
            result = socket.Receive(TimeSpan.FromMilliseconds(1000));

        Behaves_like<SuccessfulReceiveResult> receive_success;
        Behaves_like<NoReceiveMore> will_not_receive_more_parts;

        It should_use_non_blocking_mode = () =>
            socketProxy.Verify(mock => mock.Receive(IncludesSocketFlag(SocketFlags.DontWait), out message));
    }

    [Subject(typeof(ReceiveSocket), "non-blocking")]
    class when_receiving_a_message_from_a_remote_socket_with_expiring_timeout : using_receive_socket
    {
        Establish context = () =>
        {
            socketProxy.Setup(mock => mock.Receive(Moq.It.IsAny<int>(), out message)).Returns(-1);
            errorProviderProxy.Setup(mock => mock.GetErrorCode()).Returns((int)ErrorCode.Eagain);
        };

        Because of = () =>
            result = socket.Receive(TimeSpan.FromMilliseconds(5));

        It should_return_a_try_again_status = () =>
            socket.ReceiveStatus.ShouldEqual(ReceiveResult.TryAgain);

        It should_have_empty_message_data = () =>
            result.ShouldBeEmpty();

        It should_use_non_blocking_mode = () =>
            socketProxy.Verify(mock => mock.Receive(IncludesSocketFlag(SocketFlags.DontWait), out message));
    }

    [Subject(typeof(ReceiveSocket), "non-blocking")]
    class when_receiving_a_message_with_timeout_is_interrupted_by_context_termination : using_receive_socket_with_context_termination
    {
        Because of = () =>
            exception = Catch.Exception(() => result = socket.Receive(TimeSpan.FromMilliseconds(5)));

        Behaves_like<InterruptedReceiveResult> interrupted;
    }

    [Subject(typeof(ReceiveSocket), "non-blocking")]
    class when_receiving_a_message_with_timeout_and_the_proxy_returns_an_error : using_receive_socket_with_socket_error
    {
        Because of = () =>
            exception = Catch.Exception(() => result = socket.Receive(TimeSpan.FromMilliseconds(5)));

        Behaves_like<FailsWithSocketException> fails_with_exception;
    }

    [Subject(typeof(ReceiveSocket), "non-blocking")]
    class when_receiving_a_multi_part_message_from_a_remote_socket_with_timeout : using_receive_socket
    {
        protected static int receiveMore = 1;

        Establish context = () =>
        {
            socketProxy.SetupSequence(mock => mock.Receive(Moq.It.IsAny<int>(), out message))
                .Returns(-1)
                .Returns(message.Length);

            socketProxy.Setup(mock => mock.GetSocketOption((int)SocketOption.RcvMore, out receiveMore)).Returns(0);
            errorProviderProxy.Setup(mock => mock.GetErrorCode()).Returns((int)ErrorCode.Eagain);
        };

        Because of = () =>
            result = socket.Receive(TimeSpan.FromMilliseconds(1000));

        Behaves_like<SuccessfulReceiveResult> receive_success;
        Behaves_like<ReceiveMore> will_receive_more_parts;
    }

    [Behaviors]
    class SuccessfulReceiveResult
    {
        protected static IReceiveSocket socket;
        protected static byte[] message;
        protected static byte[] result;

        It should_return_a_successful_status = () =>
            socket.ReceiveStatus.ShouldEqual(ReceiveResult.Received);

        It should_receive_the_underlying_message_contents = () =>
            result.ShouldEqual(message);
    }

    [Behaviors]
    class ReceiveMore
    {
        protected static ISocket socket;

        It should_indicate_more_message_parts_will_follow_on_socket = () =>
            socket.ReceiveMore.ShouldBeTrue();
    }

    [Behaviors]
    class NoReceiveMore
    {
        protected static ISocket socket;

        It should_indicate_no_more_message_parts_will_follow_on_socket = () =>
            socket.ReceiveMore.ShouldBeFalse();
    }

    [Behaviors]
    class InterruptedReceiveResult
    {
        protected static IReceiveSocket socket;
        protected static byte[] result;
        protected static Exception exception;

        It should_return_an_interrupted_status = () =>
            socket.ReceiveStatus.ShouldEqual(ReceiveResult.Interrupted);

        It should_have_empty_message_data = () =>
            result.ShouldBeEmpty();

        It should_not_have_more_parts = () =>
            socket.ReceiveMore.ShouldBeFalse();

        It should_not_fail = () =>
            exception.ShouldBeNull();
    }

    abstract class using_receive_socket_with_context_termination : using_receive_socket
    {
        protected static Exception exception;

        Establish context = () =>
        {
            socketProxy.Setup(mock => mock.Receive(Moq.It.IsAny<int>(), out nullMessage)).Returns(-1);
            errorProviderProxy.Setup(mock => mock.GetErrorCode()).Returns((int)ErrorCode.Eterm);
        };
    }

    abstract class using_receive_socket_with_socket_error : using_receive_socket
    {
        protected static Exception exception;

        Establish context = () =>
        {
            socketProxy.Setup(mock => mock.Receive(Moq.It.IsAny<int>(), out nullMessage)).Returns(-1);
            errorProviderProxy.Setup(mock => mock.GetErrorCode()).Returns((int)ErrorCode.Efsm);
        };
    }

    abstract class using_receive_socket : using_mock_socket_proxy<IReceiveSocket>
    {
        protected static byte[] message = "Test".ToZmqBuffer();
        protected static byte[] nullMessage;
        protected static byte[] result;

        Establish context = () =>
            socket = new ReceiveSocket(socketProxy.Object, errorProviderProxy.Object);
    }
}
