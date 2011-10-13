namespace ZeroMQ.AcceptanceTests.DeviceSpecs
{
    using System;

    using Machine.Specifications;

    using ZeroMQ.Sockets;
    using ZeroMQ.Sockets.Devices;

    [Subject("Forwarder")]
    class when_using_forwarder_device_with_full_subscription : using_forwarder_device
    {
        static ReceivedMessage message1;
        static ReceivedMessage message2;
        static SendResult sendResult1;
        static SendResult sendResult2;

        Establish context = () =>
        {
            deviceInit = dev => dev.ConfigureFrontend().SubscribeToAll();
            receiverInit = sub => sub.SubscribeAll();

            receiverAction = sub =>
            {
                message1 = sub.Receive();
                message2 = sub.Receive(TimeSpan.FromMilliseconds(500));
            };

            senderAction = pub =>
            {
                sendResult1 = pub.Send("PREFIX Test message".ToZmqBuffer());
                sendResult2 = pub.Send("NOTPREFIX Test message".ToZmqBuffer());
            };
        };

        Because of = StartThreads;

        It should_send_the_first_message_successfully = () =>
            sendResult1.ShouldEqual(SendResult.Sent);

        It should_send_the_second_message_successfully = () =>
            sendResult2.ShouldEqual(SendResult.Sent);

        It should_receive_the_first_message_successfully = () =>
            message1.Result.ShouldEqual(ReceiveResult.Received);

        It should_contain_the_correct_first_message_data = () =>
            message1.Data.ShouldEqual("PREFIX Test message".ToZmqBuffer());

        It should_not_have_more_parts_after_the_first_message = () =>
            message1.HasMoreParts.ShouldBeFalse();

        It should_receive_the_second_message_successfully = () =>
            message2.Result.ShouldEqual(ReceiveResult.Received);

        It should_contain_the_correct_second_message_data = () =>
            message2.Data.ShouldEqual("NOTPREFIX Test message".ToZmqBuffer());

        It should_not_have_more_parts_after_the_second_message = () =>
            message2.HasMoreParts.ShouldBeFalse();
    }

    [Subject("Forwarder")]
    class when_using_forwarder_device_with_a_receiver_subscription : using_forwarder_device
    {
        static ReceivedMessage message1;
        static ReceivedMessage message2;
        static SendResult sendResult1;
        static SendResult sendResult2;

        Establish context = () =>
        {
            deviceInit = dev => dev.ConfigureFrontend().SubscribeToAll();
            receiverInit = sub => sub.Subscribe("PREFIX".ToZmqBuffer());

            receiverAction = sub =>
            {
                message1 = sub.Receive();
                message2 = sub.Receive(TimeSpan.FromMilliseconds(500));
            };

            senderAction = pub =>
            {
                sendResult1 = pub.Send("PREFIX Test message".ToZmqBuffer());
                sendResult2 = pub.Send("NOTPREFIX Test message".ToZmqBuffer());
            };
        };

        Because of = StartThreads;

        It should_send_the_first_message_successfully = () =>
            sendResult1.ShouldEqual(SendResult.Sent);

        It should_send_the_second_message_successfully = () =>
            sendResult2.ShouldEqual(SendResult.Sent);

        It should_receive_the_first_message_successfully = () =>
            message1.Result.ShouldEqual(ReceiveResult.Received);

        It should_contain_the_correct_first_message_data = () =>
            message1.Data.ShouldEqual("PREFIX Test message".ToZmqBuffer());

        It should_not_have_more_parts_after_the_first_message = () =>
            message1.HasMoreParts.ShouldBeFalse();

        It should_tell_receiver_to_retry_the_second_message = () =>
            message2.Result.ShouldEqual(ReceiveResult.TryAgain);

        It should_contain_empty_second_message_data = () =>
            message2.Data.ShouldBeEmpty();

        It should_not_have_more_parts_after_the_second_message = () =>
            message2.HasMoreParts.ShouldBeFalse();
    }

    [Subject("Forwarder")]
    class when_using_forwarder_device_with_a_device_subscription : using_forwarder_device
    {
        static ReceivedMessage message1;
        static ReceivedMessage message2;
        static SendResult sendResult1;
        static SendResult sendResult2;

        Establish context = () =>
        {
            deviceInit = dev => dev.ConfigureFrontend().SubscribeTo("PREFIX".ToZmqBuffer());
            receiverInit = sub => sub.SubscribeAll();

            receiverAction = sub =>
            {
                message1 = sub.Receive();
                message2 = sub.Receive(TimeSpan.FromMilliseconds(500));
            };

            senderAction = pub =>
            {
                sendResult1 = pub.Send("PREFIX Test message".ToZmqBuffer());
                sendResult2 = pub.Send("NOTPREFIX Test message".ToZmqBuffer());
            };
        };

        Because of = StartThreads;

        It should_send_the_first_message_successfully = () =>
            sendResult1.ShouldEqual(SendResult.Sent);

        It should_send_the_second_message_successfully = () =>
            sendResult2.ShouldEqual(SendResult.Sent);

        It should_receive_the_first_message_successfully = () =>
            message1.Result.ShouldEqual(ReceiveResult.Received);

        It should_contain_the_correct_first_message_data = () =>
            message1.Data.ShouldEqual("PREFIX Test message".ToZmqBuffer());

        It should_not_have_more_parts_after_the_first_message = () =>
            message1.HasMoreParts.ShouldBeFalse();

        It should_tell_receiver_to_retry_the_second_message = () =>
            message2.Result.ShouldEqual(ReceiveResult.TryAgain);

        It should_contain_empty_second_message_data = () =>
            message2.Data.ShouldBeEmpty();

        It should_not_have_more_parts_after_the_second_message = () =>
            message2.HasMoreParts.ShouldBeFalse();
    }

    abstract class using_forwarder_device : using_threaded_device<ISendSocket, ISubscribeSocket>
    {
        static using_forwarder_device()
        {
            createSender = () => zmqContext.CreatePublishSocket();
            createReceiver = () => zmqContext.CreateSubscribeSocket();
            createDevice = () => ForwarderDevice.Create(zmqContext);
        }
    }
}
