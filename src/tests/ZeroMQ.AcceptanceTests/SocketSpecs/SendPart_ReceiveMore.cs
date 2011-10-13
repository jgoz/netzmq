namespace ZeroMQ.AcceptanceTests.SocketSpecs
{
    using System;

    using Machine.Specifications;

    using ZeroMQ.Sockets;

    [Subject("Send and receive partial")]
    class when_sending_and_receiving_partial_messages_in_blocking_mode : using_threaded_req_rep
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

    [Subject("Send and receive partial")]
    class when_sending_and_receiving_partial_messages_with_an_ample_timeout : using_threaded_req_rep
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
}
