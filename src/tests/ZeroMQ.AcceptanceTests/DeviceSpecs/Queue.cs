namespace ZeroMQ.AcceptanceTests.DeviceSpecs
{
    using System;

    using Machine.Specifications;

    using ZeroMQ.Sockets.Devices;

    [Subject("Queue")]
    class when_using_queue_device_to_send_a_single_message_in_blocking_mode : using_queue_device
    {
        protected static byte[] message;
        protected static SendResult sendResult;

        Establish context = () =>
        {
            senderAction = req => sendResult = req.Send(Messages.SingleMessage);
            receiverAction = rep => message = rep.Receive();
        };

        Because of = StartThreads;

        Behaves_like<SingleMessageSuccess> successfully_sent_single_message;
    }

    [Subject("Queue")]
    class when_using_queue_device_to_send_a_single_message_with_an_ample_timeout : using_queue_device
    {
        protected static byte[] message;
        protected static SendResult sendResult;

        Establish context = () =>
        {
            senderAction = req => sendResult = req.Send(Messages.SingleMessage, TimeSpan.FromMilliseconds(2000));
            receiverAction = rep => message = rep.Receive(TimeSpan.FromMilliseconds(2000));
        };

        Because of = StartThreads;

        Behaves_like<SingleMessageSuccess> successfully_sent_single_message;
    }

    [Subject("Queue")]
    class when_using_queue_device_to_receive_a_single_message_with_insufficient_timeout : using_queue_device
    {
        protected static byte[] message;

        Establish context = () =>
        {
            receiverAction = rep => message = rep.Receive(TimeSpan.FromMilliseconds(0));
        };

        Because of = StartThreads;

        Behaves_like<SingleMessageTryAgain> receiver_must_try_again;
    }

    [Subject("Queue")]
    class when_using_queue_device_to_send_a_multipart_message_in_blocking_mode : using_queue_device
    {
        protected static byte[] message1;
        protected static byte[] message2;
        protected static SendResult sendResult1;
        protected static SendResult sendResult2;
        protected static ReceiveResult receiveResult1;
        protected static ReceiveResult receiveResult2;
        protected static bool receiveMore1;
        protected static bool receiveMore2;

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
                receiveResult1 = rep.ReceiveStatus;
                receiveMore1 = rep.ReceiveMore;

                message2 = rep.Receive();
                receiveResult2 = rep.ReceiveStatus;
                receiveMore2 = rep.ReceiveMore;
            };
        };

        Because of = StartThreads;

        Behaves_like<MultiPartMessageSuccess> successfully_sent_multi_part_message;
    }

    [Subject("Queue")]
    class when_using_queue_device_to_send_a_multipart_message_with_an_ample_timeout : using_queue_device
    {
        protected static byte[] message1;
        protected static byte[] message2;
        protected static SendResult sendResult1;
        protected static SendResult sendResult2;
        protected static ReceiveResult receiveResult1;
        protected static ReceiveResult receiveResult2;
        protected static bool receiveMore1;
        protected static bool receiveMore2;

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
                receiveResult1 = rep.ReceiveStatus;
                receiveMore1 = rep.ReceiveMore;

                message2 = rep.Receive(TimeSpan.FromMilliseconds(2000));
                receiveResult2 = rep.ReceiveStatus;
                receiveMore2 = rep.ReceiveMore;
            };
        };

        Because of = StartThreads;

        Behaves_like<MultiPartMessageSuccess> sends_multi_part_message_successfully;
    }

    abstract class using_queue_device : using_threaded_device<IDuplexSocket, IDuplexSocket>
    {
        static using_queue_device()
        {
            createSender = () => zmqContext.CreateRequestSocket();
            createReceiver = () => zmqContext.CreateReplySocket();
            createDevice = () => (ZmqDevice<IDuplexSocket, IDuplexSocket>)QueueDevice.Create(zmqContext);
        }
    }
}
