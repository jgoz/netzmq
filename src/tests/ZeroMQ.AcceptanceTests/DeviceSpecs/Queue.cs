namespace ZeroMQ.AcceptanceTests.DeviceSpecs
{
    using System;

    using Machine.Specifications;

    using ZeroMQ.Sockets;
    using ZeroMQ.Sockets.Devices;

    [Subject("Queue")]
    class when_using_queue_device_to_send_a_single_message_in_blocking_mode : using_queue_device
    {
        static ReceivedMessage message;
        static SendResult sendResult;

        Establish context = () =>
        {
            senderAction = req => sendResult = req.Send("Test message".ToZmqBuffer());
            receiverAction = rep => message = rep.Receive();
        };

        Because of = StartThreads;

        It should_be_sent_successfully = () =>
            sendResult.ShouldEqual(SendResult.Sent);

        It should_be_successfully_received = () =>
            message.Result.ShouldEqual(ReceiveResult.Received);

        It should_contain_the_given_message = () =>
            message.Data.ShouldEqual("Test message".ToZmqBuffer());

        It should_not_have_more_parts = () =>
            message.HasMoreParts.ShouldBeFalse();
    }

    [Subject("Queue")]
    class when_using_queue_device_to_send_a_single_message_with_an_ample_timeout : using_queue_device
    {
        static ReceivedMessage message;
        static SendResult sendResult;

        Establish context = () =>
        {
            senderAction = req => sendResult = req.Send("Test message".ToZmqBuffer(), TimeSpan.FromMilliseconds(2000));
            receiverAction = rep => message = rep.Receive(TimeSpan.FromMilliseconds(2000));
        };

        Because of = StartThreads;

        It should_be_sent_successfully = () =>
            sendResult.ShouldEqual(SendResult.Sent);

        It should_be_successfully_received = () =>
            message.Result.ShouldEqual(ReceiveResult.Received);

        It should_contain_the_given_message = () =>
            message.Data.ShouldEqual("Test message".ToZmqBuffer());

        It should_not_have_more_parts = () =>
            message.HasMoreParts.ShouldBeFalse();
    }

    [Subject("Queue")]
    class when_using_queue_device_to_receive_a_single_message_with_insufficient_timeout : using_queue_device
    {
        static ReceivedMessage message;

        Establish context = () =>
        {
            receiverAction = rep => message = rep.Receive(TimeSpan.FromMilliseconds(0));
        };

        Because of = StartThreads;

        It should_require_the_receiver_to_try_again = () =>
            message.Result.ShouldEqual(ReceiveResult.TryAgain);

        It should_not_contain_the_given_message = () =>
            message.Data.ShouldBeEmpty();

        It should_not_have_more_parts = () =>
            message.HasMoreParts.ShouldBeFalse();
    }

    [Subject("Queue")]
    class when_using_queue_device_to_send_a_multipart_message_in_blocking_mode : using_queue_device
    {
        static ReceivedMessage message1;
        static ReceivedMessage message2;
        static SendResult sendResult1;
        static SendResult sendResult2;

        Establish context = () =>
        {
            senderAction = req =>
            {
                sendResult1 = req.SendPart("First".ToZmqBuffer());
                sendResult2 = req.Send("Last".ToZmqBuffer());
            };

            receiverAction = rep =>
            {
                message1 = rep.Receive();
                message2 = rep.Receive();
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
            message1.Data.ShouldEqual("First".ToZmqBuffer());

        It should_have_more_parts_after_the_first_message = () =>
            message1.HasMoreParts.ShouldBeTrue();

        It should_receive_the_second_message_successfully = () =>
            message2.Result.ShouldEqual(ReceiveResult.Received);

        It should_contain_the_correct_second_message_data = () =>
            message2.Data.ShouldEqual("Last".ToZmqBuffer());

        It should_not_have_more_parts_after_the_second_message = () =>
            message2.HasMoreParts.ShouldBeFalse();
    }

    [Subject("Queue")]
    class when_using_queue_device_to_send_a_multipart_message_with_an_ample_timeout : using_queue_device
    {
        static ReceivedMessage message1;
        static ReceivedMessage message2;
        static SendResult sendResult1;
        static SendResult sendResult2;

        Establish context = () =>
        {
            senderAction = req =>
            {
                sendResult1 = req.SendPart("First".ToZmqBuffer(), TimeSpan.FromMilliseconds(2000));
                sendResult2 = req.Send("Last".ToZmqBuffer(), TimeSpan.FromMilliseconds(2000));
            };

            receiverAction = rep =>
            {
                message1 = rep.Receive(TimeSpan.FromMilliseconds(2000));
                message2 = rep.Receive(TimeSpan.FromMilliseconds(2000));
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
            message1.Data.ShouldEqual("First".ToZmqBuffer());

        It should_have_more_parts_after_the_first_message = () =>
            message1.HasMoreParts.ShouldBeTrue();

        It should_receive_the_second_message_successfully = () =>
            message2.Result.ShouldEqual(ReceiveResult.Received);

        It should_contain_the_correct_second_message_data = () =>
            message2.Data.ShouldEqual("Last".ToZmqBuffer());

        It should_not_have_more_parts_after_the_second_message = () =>
            message2.HasMoreParts.ShouldBeFalse();
    }

    abstract class using_queue_device : using_threaded_device<IDuplexSocket, IDuplexSocket>
    {
        static using_queue_device()
        {
            createSender = () => zmqContext.CreateRequestSocket();
            createReceiver = () => zmqContext.CreateReplySocket();
            createDevice = () => QueueDevice.Create(zmqContext);
        }
    }
}
