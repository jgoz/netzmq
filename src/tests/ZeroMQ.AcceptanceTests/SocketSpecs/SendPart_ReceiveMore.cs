namespace ZeroMQ.AcceptanceTests.SocketSpecs
{
    using System;

    using Machine.Specifications;

    using ZeroMQ.Sockets;

    [Subject("Send and receive partial")]
    class when_sending_and_receiving_partial_messages_in_blocking_mode : using_threaded_req_and_rep_sockets
    {
        static ReceivedMessage message1;
        static ReceivedMessage message2;
        static SendResult sendResult1;
        static SendResult sendResult2;

        Establish context = () =>
        {
            reqAction = req =>
            {
                sendResult1 = req.SendPart("First".ToZmqBuffer());
                sendResult2 = req.Send("Last".ToZmqBuffer());
            };

            repAction = rep =>
            {
                message1 = rep.Receive();
                message2 = rep.Receive();
            };
        };

        Because of = StartThreads;

        It first_message_should_be_sent_successfully = () =>
            sendResult1.ShouldEqual(SendResult.Sent);

        It second_message_should_be_sent_successfully = () =>
            sendResult2.ShouldEqual(SendResult.Sent);

        It first_message_should_be_successfully_received = () =>
            message1.Result.ShouldEqual(ReceiveResult.Received);

        It first_message_should_contain_the_given_message = () =>
            message1.Data.ShouldEqual("First".ToZmqBuffer());

        It first_message_should_have_more_parts = () =>
            message1.HasMoreParts.ShouldBeTrue();

        It second_message_should_be_successfully_received = () =>
            message2.Result.ShouldEqual(ReceiveResult.Received);

        It second_message_should_contain_the_given_message = () =>
            message2.Data.ShouldEqual("Last".ToZmqBuffer());

        It second_message_should_not_have_more_parts = () =>
            message2.HasMoreParts.ShouldBeFalse();
    }

    [Subject("Send and receive partial")]
    class when_sending_and_receiving_partial_messages_with_an_ample_timeout : using_threaded_req_and_rep_sockets
    {
        static ReceivedMessage message1;
        static ReceivedMessage message2;
        static SendResult sendResult1;
        static SendResult sendResult2;

        Establish context = () =>
        {
            reqAction = req =>
            {
                sendResult1 = req.SendPart("First".ToZmqBuffer(), TimeSpan.FromMilliseconds(2000));
                sendResult2 = req.Send("Last".ToZmqBuffer(), TimeSpan.FromMilliseconds(2000));
            };

            repAction = rep =>
            {
                message1 = rep.Receive(TimeSpan.FromMilliseconds(2000));
                message2 = rep.Receive(TimeSpan.FromMilliseconds(2000));
            };
        };

        Because of = StartThreads;

        It first_message_should_be_sent_successfully = () =>
            sendResult1.ShouldEqual(SendResult.Sent);

        It second_message_should_be_sent_successfully = () =>
            sendResult2.ShouldEqual(SendResult.Sent);

        It first_message_should_be_successfully_received = () =>
            message1.Result.ShouldEqual(ReceiveResult.Received);

        It first_message_should_contain_the_given_message = () =>
            message1.Data.ShouldEqual("First".ToZmqBuffer());

        It first_message_should_have_more_parts = () =>
            message1.HasMoreParts.ShouldBeTrue();

        It second_message_should_be_successfully_received = () =>
            message2.Result.ShouldEqual(ReceiveResult.Received);

        It second_message_should_contain_the_given_message = () =>
            message2.Data.ShouldEqual("Last".ToZmqBuffer());

        It second_message_should_not_have_more_parts = () =>
            message2.HasMoreParts.ShouldBeFalse();
    }
}
