namespace ZeroMQ.UnitTests.ZmqSocketSpecs
{
    using Machine.Specifications;

    using Moq;

    using ZeroMQ.Proxy;
    using ZeroMQ.Sockets;

    abstract class using_mock_socket_proxy<TSocket> where TSocket : ISocket
    {
        protected static Mock<ISocketProxy> socketProxy;
        protected static Mock<IErrorProviderProxy> errorProviderProxy;
        protected static TSocket socket;

        Establish context = () =>
        {
            socketProxy = new Mock<ISocketProxy>();
            errorProviderProxy = new Mock<IErrorProviderProxy>();
        };

        protected class ConcreteSocket : ZmqSocket
        {
            public ConcreteSocket()
                : base(socketProxy.Object, errorProviderProxy.Object)
            {
            }
        }
    }
}
