namespace ZeroMQ.UnitTests.ZmqSocketSpecs
{
    using System;

    using Machine.Specifications;

    using Moq;

    using ZeroMQ.Proxy;
    using ZeroMQ.Sockets;

    using It = Machine.Specifications.It;

    [Subject(typeof(ZmqSocket), "close")]
    class when_closing_an_open_socket : using_base_socket_class
    {
        Because of = () =>
            socket.Close();

        It should_close_the_underlying_socket = () =>
            socketProxy.Verify(mock => mock.Close());
    }

    [Subject(typeof(ZmqSocket), "close")]
    class when_closing_a_closed_socket : using_base_socket_class
    {
        static Exception exception;

        Because of = () =>
            exception = Catch.Exception(() =>
            {
                socket.Close();
                socket.Close();
            });

        It should_close_the_underlying_socket_once = () =>
            socketProxy.Verify(mock => mock.Close(), Times.Once());

        It should_not_fail = () =>
            exception.ShouldBeNull();
    }

    [Subject(typeof(ZmqSocket), "close")]
    class when_closing_a_disposed_socket : using_base_socket_class
    {
        static Exception exception;

        Because of = () =>
            exception = Catch.Exception(() =>
            {
                socket.Dispose();
                socket.Close();
            });

        It should_close_the_underlying_socket_once = () =>
            socketProxy.Verify(mock => mock.Close(), Times.Once());

        It should_not_fail = () =>
            exception.ShouldBeNull();
    }

    [Subject(typeof(ZmqSocket), "close")]
    class when_closing_is_interrupted_by_context_termination : using_base_socket_class
    {
        static Exception exception;

        Establish context = () =>
        {
            socketProxy.Setup(mock => mock.Close()).Returns(-1);
            errorProviderProxy.Setup(mock => mock.GetErrorCode()).Returns((int)ErrorCode.Eterm);
        };

        Because of = () =>
            exception = Catch.Exception(() => socket.Close());

        It should_not_fail = () =>
            exception.ShouldBeNull();
    }

    [Subject(typeof(ZmqSocket), "close")]
    class when_closing_and_the_proxy_returns_an_error : using_base_socket_class
    {
        static Exception exception;

        Establish context = () =>
        {
            socketProxy.Setup(mock => mock.Close()).Returns(-1);
            errorProviderProxy.Setup(mock => mock.GetErrorCode()).Returns((int)ErrorCode.Enotsock);
        };

        Because of = () =>
            exception = Catch.Exception(() => socket.Close());

        It should_fail_with_socket_exception = () =>
            exception.ShouldBeOfType<ZmqSocketException>();
    }
}
