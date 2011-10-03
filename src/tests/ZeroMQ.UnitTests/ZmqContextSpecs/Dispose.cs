namespace ZeroMQ.UnitTests.ZmqContextSpecs
{
    using System;

    using Machine.Specifications;

    using Moq;

    using ZeroMQ.Sockets;

    using It = Machine.Specifications.It;

    [Subject(typeof(ZmqContext), "dispose")]
    class when_disposing_a_zmq_context : using_mock_context_proxy
    {
        Because of = () =>
            zmqContext.Dispose();

        It should_terminate_the_underlying_context = () =>
            contextProxy.Verify(mock => mock.Terminate());
    }

    [Subject(typeof(ZmqContext), "dispose")]
    class when_disposing_a_zmq_context_multiple_times : using_mock_context_proxy
    {
        static Exception exception;

        Because of = () =>
            exception = Catch.Exception(() =>
            {
                zmqContext.Dispose();
                zmqContext.Dispose();
            });

        It should_terminate_the_underlying_context_once = () =>
            contextProxy.Verify(mock => mock.Terminate(), Times.Once());

        It should_not_fail = () =>
            exception.ShouldBeNull();
    }
}
