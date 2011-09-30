namespace ZeroMQ.UnitTests.ZmqContextSpecs
{
    using System;

    using Machine.Specifications;

    using Moq;

    using ZeroMQ.Proxy;
    using ZeroMQ.Sockets;

    using It = Machine.Specifications.It;

    [Subject("ZMQ Context")]
    class when_creating_a_poll_set_with_one_socket : using_context_to_create_poll_set
    {
        Because of = () =>
            pollSet = zmqContext.CreatePollSet(new[] { new DuplexSocket(socketProxy.Object, errorProviderProxy.Object) });

        It should_use_correct_poll_item_count_in_proxy_call = () =>
            factory.Verify(mock => mock.CreatePollSet(1));
    }

    [Subject("ZMQ Context")]
    class when_creating_a_poll_set_with_no_sockets : using_context_to_create_poll_set
    {
        static Exception exception;

        Because of = () =>
            exception = Catch.Exception(() => pollSet = zmqContext.CreatePollSet(new ISocket[] { }));

        It should_fail = () =>
            exception.ShouldBeOfType<ArgumentException>();
    }

    [Subject("ZMQ Context")]
    class when_creating_a_poll_set_with_a_foreign_socket_implementation : using_context_to_create_poll_set
    {
        static Exception exception;

        Because of = () =>
            exception = Catch.Exception(() => pollSet = zmqContext.CreatePollSet(new[] { new Mock<ISocket>().Object }));

        It should_fail = () =>
            exception.ShouldBeOfType<ArgumentException>();
    }

    abstract class using_context_to_create_poll_set : using_mock_context_proxy
    {
        protected static Mock<IPollSetProxy> pollSetProxy;
        protected static Mock<ISocketProxy> socketProxy;
        protected static IPollSet pollSet;

        Establish context = () =>
        {
            pollSetProxy = new Mock<IPollSetProxy>();
            socketProxy = new Mock<ISocketProxy>();
            factory.Setup(mock => mock.CreatePollSet(Moq.It.IsAny<int>())).Returns(pollSetProxy.Object);
        };
    }
}
