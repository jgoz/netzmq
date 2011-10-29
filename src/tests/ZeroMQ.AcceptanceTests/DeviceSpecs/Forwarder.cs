namespace ZeroMQ.AcceptanceTests.DeviceSpecs
{
    using System;

    using Machine.Specifications;

    using ZeroMQ.Sockets.Devices;

    [Subject("Forwarder")]
    class when_using_forwarder_device_with_full_subscription : using_forwarder_device
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
            deviceInit = dev => dev.ConfigureFrontend().SubscribeToAll();
            receiverInit = sub => sub.SubscribeAll();

            receiverAction = sub =>
            {
                message1 = sub.Receive();
                receiveResult1 = sub.ReceiveStatus;
                receiveMore1 = sub.ReceiveMore;

                message2 = sub.Receive(TimeSpan.FromMilliseconds(500));
                receiveResult2 = sub.ReceiveStatus;
                receiveMore2 = sub.ReceiveMore;
            };

            senderAction = pub =>
            {
                sendResult1 = pub.Send(Messages.PubSubFirst);
                sendResult2 = pub.Send(Messages.PubSubSecond);
            };
        };

        Because of = StartThreads;

        Behaves_like<PubSubReceiveAll> successfully_received_all_messages;
    }

    [Subject("Forwarder")]
    class when_using_forwarder_device_with_a_receiver_subscription : using_forwarder_device
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
            deviceInit = dev => dev.ConfigureFrontend().SubscribeToAll();
            receiverInit = sub => sub.Subscribe(Messages.PubSubPrefix);

            receiverAction = sub =>
            {
                message1 = sub.Receive();
                receiveResult1 = sub.ReceiveStatus;
                receiveMore1 = sub.ReceiveMore;

                message2 = sub.Receive(TimeSpan.FromMilliseconds(500));
                receiveResult2 = sub.ReceiveStatus;
                receiveMore2 = sub.ReceiveMore;
            };

            senderAction = pub =>
            {
                sendResult1 = pub.Send(Messages.PubSubFirst);
                sendResult2 = pub.Send(Messages.PubSubSecond);
            };
        };

        Because of = StartThreads;

        Behaves_like<PubSubReceiveFirst> successfully_received_first_message_and_filtered_out_second;
    }

    [Subject("Forwarder")]
    class when_using_forwarder_device_with_a_device_subscription : using_forwarder_device
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
            deviceInit = dev => dev.ConfigureFrontend().SubscribeTo(Messages.PubSubPrefix);
            receiverInit = sub => sub.SubscribeAll();

            receiverAction = sub =>
            {
                message1 = sub.Receive();
                receiveResult1 = sub.ReceiveStatus;
                receiveMore1 = sub.ReceiveMore;

                message2 = sub.Receive(TimeSpan.FromMilliseconds(500));
                receiveResult2 = sub.ReceiveStatus;
                receiveMore2 = sub.ReceiveMore;
            };

            senderAction = pub =>
            {
                sendResult1 = pub.Send(Messages.PubSubFirst);
                sendResult2 = pub.Send(Messages.PubSubSecond);
            };
        };

        Because of = StartThreads;

        Behaves_like<PubSubReceiveFirst> successfully_received_first_message_and_filtered_out_second;
    }

    abstract class using_forwarder_device : using_threaded_device<ISendSocket, ISubscribeSocket>
    {
        static using_forwarder_device()
        {
            createSender = () => zmqContext.CreatePublishSocket();
            createReceiver = () => zmqContext.CreateSubscribeSocket();
            createDevice = () => (ZmqDevice<ISubscribeSocket, ISendSocket>)ForwarderDevice.Create(zmqContext);
        }
    }
}
