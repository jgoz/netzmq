namespace ZeroMQ.UnitTests.ZmqDeviceSpecs
{
    using Machine.Specifications;

    using Moq;

    using ZeroMQ.Proxy;
    using ZeroMQ.Sockets;
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

            device = new ConcreteDevice(frontend.Object, backend.Object, deviceProxy.Object, errorProviderProxy.Object);
        };

        private class ConcreteDevice : ZmqDevice<ISocket, ISocket>
        {
            public ConcreteDevice(ISocket frontend, ISocket backend, IDeviceProxy deviceProxy, IErrorProviderProxy errorProviderProxy)
                : base(frontend, backend)
            {
                this.Device = deviceProxy;
                this.ErrorProvider = new ZmqErrorProvider(errorProviderProxy);
            }
        }
    }
}
