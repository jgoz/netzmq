namespace ZeroMQ.AcceptanceTests.DeviceSpecs
{
    using System;

    using Machine.Specifications;

    using ZeroMQ.Sockets.Devices;

    [Subject("Streamer")]
    class when_using_streamer_device_to_send_a_single_message_in_blocking_mode : using_streamer_device
    {
        protected static ReceivedMessage message;
        protected static SendResult sendResult;

        Establish context = () =>
        {
            senderAction = req => sendResult = req.Send(Messages.SingleMessage);
            receiverAction = rep => message = rep.Receive();
        };

        Because of = StartThreads;

        Behaves_like<SingleMessageSuccess> successfully_sent_single_message;
    }

    [Subject("Streamer")]
    class when_using_streamer_device_to_send_a_single_message_with_an_ample_timeout : using_streamer_device
    {
        protected static ReceivedMessage message;
        protected static SendResult sendResult;

        Establish context = () =>
        {
            senderAction = req => sendResult = req.Send(Messages.SingleMessage, TimeSpan.FromMilliseconds(2000));
            receiverAction = rep => message = rep.Receive(TimeSpan.FromMilliseconds(2000));
        };

        Because of = StartThreads;

        Behaves_like<SingleMessageSuccess> successfully_sent_single_message;
    }

    [Subject("Streamer")]
    class when_using_streamer_device_to_receive_a_single_message_with_insufficient_timeout : using_streamer_device
    {
        protected static ReceivedMessage message;

        Establish context = () =>
        {
            receiverAction = rep => message = rep.Receive(TimeSpan.FromMilliseconds(0));
        };

        Because of = StartThreads;

        Behaves_like<SingleMessageTryAgain> receiver_must_try_again;
    }

    [Subject("Streamer")]
    class when_using_streamer_device_to_send_a_multipart_message_in_blocking_mode : using_streamer_device
    {
        protected static ReceivedMessage message1;
        protected static ReceivedMessage message2;
        protected static SendResult sendResult1;
        protected static SendResult sendResult2;

        Establish context = () =>
        {
            senderAction = req =>
            {
                sendResult1 = req.SendPart(Messages.MultiFirst);
                sendResult2 = req.Send(Messages.MultiLast);
            };

            receiverAction = rep =>
            {
                message1 = rep.Receive();
                message2 = rep.Receive();
            };
        };

        Because of = StartThreads;

        Behaves_like<MultiPartMessageSuccess> successfully_sent_multi_part_message;
    }

    [Subject("Streamer")]
    class when_using_streamer_device_to_send_a_multipart_message_with_an_ample_timeout : using_streamer_device
    {
        protected static ReceivedMessage message1;
        protected static ReceivedMessage message2;
        protected static SendResult sendResult1;
        protected static SendResult sendResult2;

        Establish context = () =>
        {
            senderAction = req =>
            {
                sendResult1 = req.SendPart(Messages.MultiFirst, TimeSpan.FromMilliseconds(2000));
                sendResult2 = req.Send(Messages.MultiLast, TimeSpan.FromMilliseconds(2000));
            };

            receiverAction = rep =>
            {
                message1 = rep.Receive(TimeSpan.FromMilliseconds(2000));
                message2 = rep.Receive(TimeSpan.FromMilliseconds(2000));
            };
        };

        Because of = StartThreads;

        Behaves_like<MultiPartMessageSuccess> sends_multi_part_message_successfully;
    }

    abstract class using_streamer_device : using_threaded_device<ISendSocket, IReceiveSocket>
    {
        static using_streamer_device()
        {
            createSender = () => zmqContext.CreatePushSocket();
            createReceiver = () => zmqContext.CreatePullSocket();
            createDevice = () => (ZmqDevice<IReceiveSocket, ISendSocket>)StreamerDevice.Create(zmqContext);
        }
    }
}
