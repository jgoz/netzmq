namespace ZeroMQ.UnitTests.ZmqSocketSpecs
{
    using System;

    using Machine.Specifications;

    using ZeroMQ.Proxy;
    using ZeroMQ.Sockets;

    [Subject(typeof(ZmqSocket), "bind")]
    class when_binding_a_socket_to_a_valid_endpoint : using_base_socket_class
    {
        Because of = () =>
            socket.Bind("tcp://*:9090");

        It should_bind_the_underlying_socket = () =>
            socketProxy.Verify(mock => mock.Bind("tcp://*:9090"));
    }

    [Subject(typeof(ZmqSocket), "bind")]
    class when_binding_a_socket_to_a_null_endpoint : using_base_socket_class
    {
        static Exception exception;

        Because of = () =>
            exception = Catch.Exception(() => socket.Bind(null));

        It should_fail = () =>
            exception.ShouldBeOfType<ArgumentNullException>();
    }

    [Subject(typeof(ZmqSocket), "bind")]
    class when_binding_a_socket_to_an_empty_endpoint : using_base_socket_class
    {
        static Exception exception;

        Because of = () =>
            exception = Catch.Exception(() => socket.Bind(null));

        It should_fail = () =>
            exception.ShouldBeOfType<ArgumentException>();
    }

    [Subject(typeof(ZmqSocket), "bind")]
    class when_binding_is_interrupted_by_context_termination : using_base_socket_class
    {
        static Exception exception;

        Establish context = () =>
        {
            socketProxy.Setup(mock => mock.Bind(Moq.It.IsAny<string>())).Returns(-1);
            errorProviderProxy.Setup(mock => mock.GetErrorCode()).Returns((int)ErrorCode.Eterm);
        };

        Because of = () =>
            exception = Catch.Exception(() => socket.Bind("tcp://*:9090"));

        It should_not_fail = () =>
            exception.ShouldBeNull();
    }

    [Subject(typeof(ZmqSocket), "bind")]
    class when_binding_and_the_proxy_returns_an_error : using_base_socket_class
    {
        static Exception exception;

        Establish context = () =>
        {
            socketProxy.Setup(mock => mock.Bind(Moq.It.IsAny<string>())).Returns(-1);
            errorProviderProxy.Setup(mock => mock.GetErrorCode()).Returns((int)ErrorCode.Eaddrnotavail);
        };

        Because of = () =>
            exception = Catch.Exception(() => socket.Bind("tcp://*:9090"));

        It should_fail_with_socket_exception = () =>
            exception.ShouldBeOfType<ZmqSocketException>();
    }
}
