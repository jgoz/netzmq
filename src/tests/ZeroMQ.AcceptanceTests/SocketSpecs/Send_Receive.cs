namespace ZeroMQ.AcceptanceTests.SocketSpecs
{
    using System;

    using Machine.Specifications;

    using ZeroMQ.Sockets;

    [Subject("Send and receive")]
    class when_sending_and_receiving_in_blocking_mode : using_threaded_req_rep
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

    [Subject("Send and receive")]
    class when_sending_and_receiving_with_an_ample_timeout : using_threaded_req_rep
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

    [Subject("Receive")]
    class when_receiving_with_insufficient_timeout : using_threaded_req_rep
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
}
