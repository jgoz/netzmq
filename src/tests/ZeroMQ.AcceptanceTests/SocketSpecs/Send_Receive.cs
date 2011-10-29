namespace ZeroMQ.AcceptanceTests.SocketSpecs
{
    using System;

    using Machine.Specifications;

    [Subject("Send and receive")]
    class when_sending_and_receiving_in_blocking_mode : using_threaded_req_rep
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

    [Subject("Send and receive")]
    class when_sending_and_receiving_with_an_ample_timeout : using_threaded_req_rep
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

    [Subject("Receive")]
    class when_receiving_with_insufficient_timeout : using_threaded_req_rep
    {
        protected static byte[] message;

        Establish context = () =>
        {
            receiverAction = rep => message = rep.Receive(TimeSpan.FromMilliseconds(0));
        };

        Because of = StartThreads;

        Behaves_like<SingleMessageTryAgain> receiver_must_try_again;
    }
}
