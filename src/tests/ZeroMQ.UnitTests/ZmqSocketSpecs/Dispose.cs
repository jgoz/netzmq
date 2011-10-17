namespace ZeroMQ.UnitTests.ZmqSocketSpecs
{
    using System;

    using Machine.Specifications;

    using Moq;

    using ZeroMQ.Sockets;

    using It = Machine.Specifications.It;

    [Subject(typeof(ZmqSocket), "dispose")]
    class when_disposing_a_zmq_socket : using_base_socket_class
    {
        Because of = () =>
            socket.Dispose();

        It should_close_the_underlying_socket = () =>
            socketProxy.Verify(mock => mock.Close());
    }

    [Subject(typeof(ZmqSocket), "dispose")]
    class when_disposing_a_zmq_socket_multiple_times : using_base_socket_class
    {
        static Exception exception;

        Because of = () =>
            exception = Catch.Exception(() =>
            {
                socket.Dispose();
                socket.Dispose();
            });

        It should_close_the_underlying_socket_once = () =>
            socketProxy.Verify(mock => mock.Close(), Times.Once());

        It should_not_fail = () =>
            exception.ShouldBeNull();
    }
}
