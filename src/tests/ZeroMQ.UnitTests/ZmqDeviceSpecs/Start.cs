namespace ZeroMQ.UnitTests.ZmqDeviceSpecs
{
    using System;

    using Machine.Specifications;

    using ZeroMQ.Proxy;
    using ZeroMQ.Sockets;
    using ZeroMQ.Sockets.Devices;

    [Subject(typeof(ZmqDevice))]
    class when_starting_a_base_device : using_mock_device_proxy
    {
        Because of = () =>
        {
            device.ConfigureFrontend().BindTo("bind").SetSocketOption(s => s.ReceiveHighWatermark, 10);
            device.ConfigureBackend().ConnectTo("connect").SetSocketOption(s => s.SendHighWatermark, 10);
            device.Start();
        };

        It should_run_the_underlying_device = () =>
            deviceProxy.Verify(mock => mock.Run());

        It should_set_frontend_socket_options = () =>
            frontendProxy.Verify(mock => mock.SetSocketOption((int)SocketOption.RcvHwm, 10));

        It should_bind_the_frontend_socket = () =>
            frontendProxy.Verify(mock => mock.Bind("bind"));

        It should_set_backend_socket_options = () =>
            backendProxy.Verify(mock => mock.SetSocketOption((int)SocketOption.SndHwm, 10));

        It should_connect_the_backend_socket = () =>
            backendProxy.Verify(mock => mock.Connect("connect"));
    }

    [Subject(typeof(ZmqDevice))]
    class when_the_context_is_terminated : using_mock_device_proxy
    {
        static Exception exception;

        Establish context = () =>
        {
            deviceProxy.Setup(mock => mock.Run()).Returns(-1);
            errorProviderProxy.Setup(mock => mock.GetErrorCode()).Returns((int)ErrorCode.Eterm);
        };

        Because of = () =>
            exception = Catch.Exception(() => device.Start());

        It should_not_fail = () =>
            exception.ShouldBeNull();

        It should_not_be_running = () =>
            device.IsRunning.ShouldBeFalse();
    }

    [Subject(typeof(ZmqDevice))]
    class when_a_fatal_error_occurs : using_mock_device_proxy
    {
        static Exception exception;

        Establish context = () =>
        {
            deviceProxy.Setup(mock => mock.Run()).Returns(-1);
            errorProviderProxy.Setup(mock => mock.GetErrorCode()).Returns((int)ErrorCode.Efault);
        };

        Because of = () =>
            exception = Catch.Exception(() => device.Start());

        It should_fail = () =>
            exception.ShouldBeOfType<ZmqLibException>();

        It should_not_be_running = () =>
            device.IsRunning.ShouldBeFalse();
    }
}
