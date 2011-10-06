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
        protected static Mock<ISocketProxy> frontendProxy;
        protected static Mock<ISocketProxy> backendProxy;

        protected static ZmqDevice device;

        Establish context = () =>
        {
            deviceProxy = new Mock<IDeviceProxy>();
            errorProviderProxy = new Mock<IErrorProviderProxy>();

            frontendProxy = new Mock<ISocketProxy>();
            backendProxy = new Mock<ISocketProxy>();

            deviceProxy.SetupGet(mock => mock.Frontend).Returns(frontendProxy.Object);
            deviceProxy.SetupGet(mock => mock.Backend).Returns(backendProxy.Object);

            device = new ZmqDevice(deviceProxy.Object, errorProviderProxy.Object);
        };
    }
}
