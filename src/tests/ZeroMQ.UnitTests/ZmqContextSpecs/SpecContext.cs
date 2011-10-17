namespace ZeroMQ.UnitTests.ZmqContextSpecs
{
    using Machine.Specifications;

    using Moq;

    using ZeroMQ.Proxy;
    using ZeroMQ.Sockets;

    abstract class using_mock_context_proxy
    {
        protected static Mock<IProxyFactory> factory;
        protected static Mock<IContextProxy> contextProxy;
        protected static Mock<IErrorProviderProxy> errorProviderProxy;
        protected static IZmqContext zmqContext;

        Establish context = () =>
        {
            factory = new Mock<IProxyFactory>();
            contextProxy = new Mock<IContextProxy>();
            errorProviderProxy = new Mock<IErrorProviderProxy>();

            factory.SetupGet(mock => mock.ErrorProvider).Returns(errorProviderProxy.Object);

            zmqContext = new ZmqContext(factory.Object, contextProxy.Object);
        };
    }
}
