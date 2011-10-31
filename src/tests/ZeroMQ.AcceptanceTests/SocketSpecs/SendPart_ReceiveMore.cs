namespace ZeroMQ.AcceptanceTests.SocketSpecs
{
    using System;

    using Machine.Specifications;

    [Subject("Send and receive partial")]
    class when_sending_and_receiving_partial_messages_in_blocking_mode : using_threaded_req_rep
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

    [Subject("Send and receive partial")]
    class when_sending_and_receiving_partial_messages_with_an_ample_timeout : using_threaded_req_rep
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

        Behaves_like<MultiPartMessageSuccess> successfully_sent_multi_part_message;
    }
}
