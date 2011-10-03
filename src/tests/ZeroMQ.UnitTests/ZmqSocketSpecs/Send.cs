namespace ZeroMQ.UnitTests.ZmqSocketSpecs
{
    using System;

    using Machine.Specifications;

    using Moq;

    using ZeroMQ.Proxy;
    using ZeroMQ.Sockets;

    using It = Machine.Specifications.It;

    [Subject(typeof(SendSocket), "blocking single")]
    class when_sending_a_message_to_a_remote_socket : using_send_socket
    {
        Establish context = () =>
            socketProxy.Setup(mock => mock.Send(Moq.It.IsAny<int>(), message)).Returns(message.Length);

        Because of = () =>
            result = socket.Send(message);

        It should_return_a_successful_status = () =>
            result.ShouldEqual(SendResult.Sent);

        It should_send_the_message_using_the_underlying_socket = () =>
            socketProxy.Verify(mock => mock.Send(Moq.It.IsAny<int>(), message));

        It should_use_blocking_mode = () =>
            socketProxy.Verify(mock => mock.Send(ExcludesSocketFlag(SocketFlags.DontWait), Moq.It.IsAny<byte[]>()));

        It should_indicate_no_message_parts_will_follow = () =>
            socketProxy.Verify(mock => mock.Send(ExcludesSocketFlag(SocketFlags.SendMore), Moq.It.IsAny<byte[]>()));
    }

    [Subject(typeof(SendSocket), "blocking single")]
    class when_sending_a_message_is_interrupted_by_context_termination : using_send_socket_with_context_termination
    {
        Because of = () =>
            exception = Catch.Exception(() => result = socket.Send(message));

        Behaves_like<InterruptedSendResult> interrupted;
    }

    [Subject(typeof(SendSocket), "blocking single")]
    class when_sending_a_message_and_the_proxy_returns_an_error : using_send_socket_with_socket_error
    {
        Because of = () =>
            exception = Catch.Exception(() => result = socket.Send(message));

        Behaves_like<FailsWithSocketException> fails_with_exception;
    }

    [Subject(typeof(SendSocket), "non-blocking single")]
    class when_sending_a_message_to_a_remote_socket_with_timeout : using_send_socket
    {
        Establish context = () =>
        {
            socketProxy.SetupSequence(mock => mock.Send(Moq.It.IsAny<int>(), message))
                .Returns(-1)
                .Returns(message.Length);

            errorProviderProxy.Setup(mock => mock.GetErrorCode()).Returns((int)ErrorCode.Eagain);
        };

        Because of = () =>
            result = socket.Send(message, TimeSpan.FromMilliseconds(1000));

        It should_return_a_successful_status = () =>
            result.ShouldEqual(SendResult.Sent);

        It should_send_the_message_using_the_underlying_socket = () =>
            socketProxy.Verify(mock => mock.Send(Moq.It.IsAny<int>(), message));

        It should_use_non_blocking_mode = () =>
            socketProxy.Verify(mock => mock.Send(IncludesSocketFlag(SocketFlags.DontWait), Moq.It.IsAny<byte[]>()));

        It should_indicate_no_message_parts_will_follow = () =>
            socketProxy.Verify(mock => mock.Send(ExcludesSocketFlag(SocketFlags.SendMore), Moq.It.IsAny<byte[]>()));
    }

    [Subject(typeof(SendSocket), "non-blocking single")]
    class when_sending_a_message_to_a_remote_socket_with_expiring_timeout : using_send_socket
    {
        Establish context = () =>
        {
            socketProxy.Setup(mock => mock.Send(Moq.It.IsAny<int>(), message)).Returns(-1);
            errorProviderProxy.Setup(mock => mock.GetErrorCode()).Returns((int)ErrorCode.Eagain);
        };

        Because of = () =>
            result = socket.Send(message, TimeSpan.FromMilliseconds(5));

        It should_return_a_try_again_status = () =>
            result.ShouldEqual(SendResult.TryAgain);

        It should_send_the_message_using_the_underlying_socket = () =>
            socketProxy.Verify(mock => mock.Send(Moq.It.IsAny<int>(), message));

        It should_use_non_blocking_mode = () =>
            socketProxy.Verify(mock => mock.Send(IncludesSocketFlag(SocketFlags.DontWait), Moq.It.IsAny<byte[]>()));

        It should_indicate_no_message_parts_will_follow = () =>
            socketProxy.Verify(mock => mock.Send(ExcludesSocketFlag(SocketFlags.SendMore), Moq.It.IsAny<byte[]>()));
    }

    [Subject(typeof(SendSocket), "non-blocking single")]
    class when_sending_a_message_with_timeout_is_interrupted_by_context_termination : using_send_socket_with_context_termination
    {
        Because of = () =>
            exception = Catch.Exception(() => result = socket.Send(message, TimeSpan.FromMilliseconds(5)));

        Behaves_like<InterruptedSendResult> interrupted;
    }

    [Subject(typeof(SendSocket), "non-blocking single")]
    class when_sending_a_message_with_timeout_and_the_proxy_returns_an_error : using_send_socket_with_socket_error
    {
        Because of = () =>
            exception = Catch.Exception(() => result = socket.Send(message, TimeSpan.FromMilliseconds(5)));

        Behaves_like<FailsWithSocketException> fails_with_exception;
    }

    [Subject(typeof(SendSocket), "blocking partial")]
    class when_sending_a_partial_message_to_a_remote_socket : using_send_socket
    {
        Establish context = () =>
            socketProxy.Setup(mock => mock.Send(Moq.It.IsAny<int>(), message)).Returns(message.Length);

        Because of = () =>
            result = socket.SendPart(message);

        It should_return_a_successful_status = () =>
            result.ShouldEqual(SendResult.Sent);

        It should_send_the_message_using_the_underlying_socket = () =>
            socketProxy.Verify(mock => mock.Send(Moq.It.IsAny<int>(), message));

        It should_use_blocking_mode = () =>
            socketProxy.Verify(mock => mock.Send(ExcludesSocketFlag(SocketFlags.DontWait), Moq.It.IsAny<byte[]>()));

        It should_indicate_message_parts_will_follow = () =>
            socketProxy.Verify(mock => mock.Send(IncludesSocketFlag(SocketFlags.SendMore), Moq.It.IsAny<byte[]>()));
    }

    [Subject(typeof(SendSocket), "blocking partial")]
    class when_sending_a_partial_message_is_interrupted_by_context_termination : using_send_socket_with_context_termination
    {
        Because of = () =>
            exception = Catch.Exception(() => result = socket.SendPart(message));

        Behaves_like<InterruptedSendResult> interrupted;
    }

    [Subject(typeof(SendSocket), "blocking partial")]
    class when_sending_a_partial_message_and_the_proxy_returns_an_error : using_send_socket_with_socket_error
    {
        Because of = () =>
            exception = Catch.Exception(() => result = socket.SendPart(message));

        Behaves_like<FailsWithSocketException> fails_with_exception;
    }

    [Subject(typeof(SendSocket), "non-blocking partial")]
    class when_sending_a_partial_message_to_a_remote_socket_with_timeout : using_send_socket
    {
        Establish context = () =>
        {
            socketProxy.SetupSequence(mock => mock.Send(Moq.It.IsAny<int>(), message))
                .Returns(-1)
                .Returns(message.Length);

            errorProviderProxy.Setup(mock => mock.GetErrorCode()).Returns((int)ErrorCode.Eagain);
        };

        Because of = () =>
            result = socket.SendPart(message, TimeSpan.FromMilliseconds(1000));

        It should_return_a_successful_status = () =>
            result.ShouldEqual(SendResult.Sent);

        It should_send_the_message_using_the_underlying_socket = () =>
            socketProxy.Verify(mock => mock.Send(Moq.It.IsAny<int>(), message));

        It should_use_non_blocking_mode = () =>
            socketProxy.Verify(mock => mock.Send(IncludesSocketFlag(SocketFlags.DontWait), Moq.It.IsAny<byte[]>()));

        It should_indicate_no_message_parts_will_follow = () =>
            socketProxy.Verify(mock => mock.Send(IncludesSocketFlag(SocketFlags.SendMore), Moq.It.IsAny<byte[]>()));
    }

    [Subject(typeof(SendSocket), "non-blocking partial")]
    class when_sending_a_partial_message_to_a_remote_socket_with_expiring_timeout : using_send_socket
    {
        Establish context = () =>
        {
            socketProxy.Setup(mock => mock.Send(Moq.It.IsAny<int>(), message)).Returns(-1);
            errorProviderProxy.Setup(mock => mock.GetErrorCode()).Returns((int)ErrorCode.Eagain);
        };

        Because of = () =>
            result = socket.SendPart(message, TimeSpan.FromMilliseconds(5));

        It should_return_a_try_again_status = () =>
            result.ShouldEqual(SendResult.TryAgain);

        It should_send_the_message_using_the_underlying_socket = () =>
            socketProxy.Verify(mock => mock.Send(Moq.It.IsAny<int>(), message));

        It should_use_non_blocking_mode = () =>
            socketProxy.Verify(mock => mock.Send(IncludesSocketFlag(SocketFlags.DontWait), Moq.It.IsAny<byte[]>()));

        It should_indicate_message_parts_will_follow = () =>
            socketProxy.Verify(mock => mock.Send(IncludesSocketFlag(SocketFlags.SendMore), Moq.It.IsAny<byte[]>()));
    }

    [Subject(typeof(SendSocket), "non-blocking partial")]
    class when_sending_a_partial_message_with_timeout_is_interrupted_by_context_termination : using_send_socket_with_context_termination
    {
        Because of = () =>
            exception = Catch.Exception(() => result = socket.SendPart(message, TimeSpan.FromMilliseconds(5)));

        Behaves_like<InterruptedSendResult> interrupted;
    }

    [Subject(typeof(SendSocket), "non-blocking partial")]
    class when_sending_a_partial_message_with_timeout_and_the_proxy_returns_an_error : using_send_socket_with_socket_error
    {
        Because of = () =>
            exception = Catch.Exception(() => result = socket.SendPart(message, TimeSpan.FromMilliseconds(5)));

        Behaves_like<FailsWithSocketException> fails_with_exception;
    }

    [Behaviors]
    class InterruptedSendResult
    {
        protected static SendResult result;
        protected static Exception exception;

        It should_return_an_interrupted_status = () =>
            result.ShouldEqual(SendResult.Interrupted);

        It should_not_fail = () =>
            exception.ShouldBeNull();
    }

    [Behaviors]
    class FailsWithSocketException
    {
        protected static Exception exception;

        It should_fail_with_socket_exception = () =>
            exception.ShouldBeOfType<ZmqSocketException>();
    }

    abstract class using_send_socket_with_context_termination : using_send_socket
    {
        protected static Exception exception;

        Establish context = () =>
        {
            socketProxy.Setup(mock => mock.Send(Moq.It.IsAny<int>(), Moq.It.IsAny<byte[]>())).Returns(-1);
            errorProviderProxy.Setup(mock => mock.GetErrorCode()).Returns((int)ErrorCode.Eterm);
        };
    }

    abstract class using_send_socket_with_socket_error : using_send_socket
    {
        protected static Exception exception;

        Establish context = () =>
        {
            socketProxy.Setup(mock => mock.Send(Moq.It.IsAny<int>(), Moq.It.IsAny<byte[]>())).Returns(-1);
            errorProviderProxy.Setup(mock => mock.GetErrorCode()).Returns((int)ErrorCode.Efsm);
        };
    }

    abstract class using_send_socket : using_mock_socket_proxy<ISendSocket>
    {
        protected static byte[] message = "Test".ToZmqBuffer();
        protected static SendResult result;

        Establish context = () =>
            socket = new SendSocket(socketProxy.Object, errorProviderProxy.Object);
    }
}
