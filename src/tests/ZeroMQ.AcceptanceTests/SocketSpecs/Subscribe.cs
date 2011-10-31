namespace ZeroMQ.AcceptanceTests.SocketSpecs
{
    using System;
    using System.Threading;

    using Machine.Specifications;

    using ZeroMQ.Sockets;

    [Subject("Subscribe")]
    class when_subscribing_to_a_specific_prefix : using_threaded_pub_sub
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
            var signal = new ManualResetEventSlim(false);

            receiverInit = sub => sub.Subscribe("PREFIX".ZmqEncode());

            receiverAction = sub =>
            {
                signal.Set();

                message1 = sub.Receive();
                receiveResult1 = sub.ReceiveStatus;
                receiveMore1 = sub.ReceiveMore;

                message2 = sub.Receive(TimeSpan.FromMilliseconds(500));
                receiveResult2 = sub.ReceiveStatus;
                receiveMore2 = sub.ReceiveMore;
            };

            senderInit = pub => signal.Wait(1000);

            senderAction = pub =>
            {
                sendResult1 = pub.Send(Messages.PubSubFirst);
                sendResult2 = pub.Send(Messages.PubSubSecond);
            };
        };

        Because of = StartThreads;

        Behaves_like<PubSubReceiveFirst> successfully_received_first_message_and_filtered_out_second;
    }

    [Subject("Subscribe")]
    class when_subscribing_to_all_prefixes : using_threaded_pub_sub
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
            var signal = new ManualResetEventSlim(false);

            receiverInit = sub => sub.SubscribeAll();

            receiverAction = sub =>
            {
                signal.Set();

                message1 = sub.Receive();
                receiveResult1 = sub.ReceiveStatus;
                receiveMore1 = sub.ReceiveMore;

                message2 = sub.Receive(TimeSpan.FromMilliseconds(500));
                receiveResult2 = sub.ReceiveStatus;
                receiveMore2 = sub.ReceiveMore;
            };

            senderInit = pub => signal.Wait(1000);

            senderAction = pub =>
            {
                sendResult1 = pub.Send(Messages.PubSubFirst);
                sendResult2 = pub.Send(Messages.PubSubSecond);
            };
        };

        Because of = StartThreads;

        Behaves_like<PubSubReceiveAll> successfully_received_all_messages;
    }
}
