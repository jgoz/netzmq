namespace ZeroMQ.UnitTests.ZmqSocketSpecs
{
    using System;

    using Machine.Specifications;

    using Moq;

    using ZeroMQ.Sockets;

    using It = Machine.Specifications.It;

    [Subject("ZMQ Socket")]
    class when_disposing_a_zmq_socket : using_mock_socket_proxy<ZmqSocket>
    {
        Establish context = () =>
            socket = new ConcreteSocket();

        Because of = () =>
            socket.Dispose();

        It should_close_the_underlying_socket = () =>
            socketProxy.Verify(mock => mock.Close());
    }

    [Subject("ZMQ Socket")]
    class when_disposing_a_zmq_socket_multiple_times : using_mock_socket_proxy<ZmqSocket>
    {
        static Exception exception;

        Establish context = () =>
            socket = new ConcreteSocket();

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
