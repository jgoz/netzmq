namespace ZeroMQ.UnitTests.ZmqDeviceSpecs
{
    using Machine.Specifications;

    using Moq;

    using It = Machine.Specifications.It;

    class when_configuring_the_frontend_socket : using_mock_device_proxy
    {
        Because of = () =>
        {
            device.ConfigureFrontend().BindTo("bind").SetSocketOption(s => s.ReceiveHighWatermark, 10);
            device.Start();
        };

        It should_set_high_watermark_option = () =>
            frontend.VerifySet(mock => mock.ReceiveHighWatermark = 10);

        It should_bind_to_the_specified_endpoint = () =>
            frontend.Verify(mock => mock.Bind("bind"));
    }

    class when_configuring_the_backend_socket : using_mock_device_proxy
    {
        Because of = () =>
        {
            device.ConfigureBackend().ConnectTo("connect").SetSocketOption(s => s.SendHighWatermark, 10);
            device.Start();
        };

        It should_set_high_watermark_option = () =>
            backend.VerifySet(mock => mock.SendHighWatermark = 10);

        It should_connect_to_the_specified_endpoint = () =>
            backend.Verify(mock => mock.Connect("connect"));
    }

    class when_starting_multiple_times_with_configuration : using_mock_device_proxy
    {
        Because of = () =>
        {
            device.ConfigureFrontend().BindTo("bind").SetSocketOption(s => s.ReceiveHighWatermark, 10);
            device.ConfigureBackend().ConnectTo("connect").SetSocketOption(s => s.SendHighWatermark, 10);

            device.Start();
            device.Stop();
            device.Start();
        };

        It should_set_frontend_high_watermark_option_once = () =>
            frontend.VerifySet(mock => mock.ReceiveHighWatermark = 10, Times.Once());

        It should_bind_to_the_specified_endpoint_once = () =>
            frontend.Verify(mock => mock.Bind("bind"), Times.Once());

        It should_set_backend_high_watermark_option_once = () =>
            backend.VerifySet(mock => mock.SendHighWatermark = 10, Times.Once());

        It should_connect_to_the_specified_endpoint_once = () =>
            backend.Verify(mock => mock.Connect("connect"), Times.Once());
    }
}
