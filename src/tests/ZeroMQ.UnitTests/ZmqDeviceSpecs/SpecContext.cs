namespace ZeroMQ.UnitTests.ZmqDeviceSpecs
{
    using Machine.Specifications;

    using Moq;

    using ZeroMQ.Proxy;
    using ZeroMQ.Sockets.Devices;

    abstract class using_mock_device_proxy
    {
        protected static Mock<IDeviceProxy> deviceProxy;
        protected static Mock<IErrorProviderProxy> errorProviderProxy;
        protected static Mock<ISocket> frontend;
        protected static Mock<ISocket> backend;

        protected static ZmqDevice<ISocket, ISocket> device;

        Establish context = () =>
        {
            deviceProxy = new Mock<IDeviceProxy>();
            errorProviderProxy = new Mock<IErrorProviderProxy>();

            frontend = new Mock<ISocket>();
            backend = new Mock<ISocket>();

            device = new ZmqDevice<ISocket, ISocket>(frontend.Object, backend.Object, deviceProxy.Object, errorProviderProxy.Object);
        };
    }
}
