namespace ZeroMQ.AcceptanceTests
{
    using Machine.Specifications;

    using ZeroMQ.Sockets;

    static class Messages
    {
        public static readonly byte[] SingleMessage = "Test message".ToZmqBuffer();
        public static readonly byte[] MultiFirst = "First".ToZmqBuffer();
        public static readonly byte[] MultiLast = "Last".ToZmqBuffer();

        public static readonly byte[] PubSubPrefix = "PREFIX".ToZmqBuffer();
        public static readonly byte[] PubSubFirst = "PREFIX Test message".ToZmqBuffer();
        public static readonly byte[] PubSubSecond = "NOPREFIX Test message".ToZmqBuffer();
    }

    [Behaviors]
    class SingleMessageSuccess
    {
        protected static ReceivedMessage message;
        protected static SendResult sendResult;

        It should_be_sent_successfully = () =>
            sendResult.ShouldEqual(SendResult.Sent);

        It should_be_successfully_received = () =>
            message.Result.ShouldEqual(ReceiveResult.Received);

        It should_contain_the_given_message = () =>
            message.Data.ShouldEqual(Messages.SingleMessage);

        It should_not_have_more_parts = () =>
            message.HasMoreParts.ShouldBeFalse();
    }

    [Behaviors]
    class SingleMessageTryAgain
    {
        protected static ReceivedMessage message;

        It should_require_the_receiver_to_try_again = () =>
            message.Result.ShouldEqual(ReceiveResult.TryAgain);

        It should_not_contain_the_given_message = () =>
            message.Data.ShouldBeEmpty();

        It should_not_have_more_parts = () =>
            message.HasMoreParts.ShouldBeFalse();
    }

    [Behaviors]
    class MultiPartMessageSuccess
    {
        protected static ReceivedMessage message1;
        protected static ReceivedMessage message2;
        protected static SendResult sendResult1;
        protected static SendResult sendResult2;

        It should_send_the_first_message_successfully = () =>
            sendResult1.ShouldEqual(SendResult.Sent);

        It should_send_the_second_message_successfully = () =>
            sendResult2.ShouldEqual(SendResult.Sent);

        It should_receive_the_first_message_successfully = () =>
            message1.Result.ShouldEqual(ReceiveResult.Received);

        It should_contain_the_correct_first_message_data = () =>
            message1.Data.ShouldEqual(Messages.MultiFirst);

        It should_have_more_parts_after_the_first_message = () =>
            message1.HasMoreParts.ShouldBeTrue();

        It should_receive_the_second_message_successfully = () =>
            message2.Result.ShouldEqual(ReceiveResult.Received);

        It should_contain_the_correct_second_message_data = () =>
            message2.Data.ShouldEqual(Messages.MultiLast);

        It should_not_have_more_parts_after_the_second_message = () =>
            message2.HasMoreParts.ShouldBeFalse();
    }

    [Behaviors]
    class PubSubReceiveAll
    {
        protected static ReceivedMessage message1;
        protected static ReceivedMessage message2;
        protected static SendResult sendResult1;
        protected static SendResult sendResult2;

        It should_send_the_first_message_successfully = () =>
            sendResult1.ShouldEqual(SendResult.Sent);

        It should_send_the_second_message_successfully = () =>
            sendResult2.ShouldEqual(SendResult.Sent);

        It should_receive_the_first_message_successfully = () =>
            message1.Result.ShouldEqual(ReceiveResult.Received);

        It should_contain_the_correct_first_message_data = () =>
            message1.Data.ShouldEqual(Messages.PubSubFirst);

        It should_not_have_more_parts_after_the_first_message = () =>
            message1.HasMoreParts.ShouldBeFalse();

        It should_receive_the_second_message_successfully = () =>
            message2.Result.ShouldEqual(ReceiveResult.Received);

        It should_contain_the_correct_second_message_data = () =>
            message2.Data.ShouldEqual(Messages.PubSubSecond);

        It should_not_have_more_parts_after_the_second_message = () =>
            message2.HasMoreParts.ShouldBeFalse();
    }

    [Behaviors]
    class PubSubReceiveFirst
    {
        protected static ReceivedMessage message1;
        protected static ReceivedMessage message2;
        protected static SendResult sendResult1;
        protected static SendResult sendResult2;

        It should_send_the_first_message_successfully = () =>
            sendResult1.ShouldEqual(SendResult.Sent);

        It should_send_the_second_message_successfully = () =>
            sendResult2.ShouldEqual(SendResult.Sent);

        It should_receive_the_first_message_successfully = () =>
            message1.Result.ShouldEqual(ReceiveResult.Received);

        It should_contain_the_correct_first_message_data = () =>
            message1.Data.ShouldEqual(Messages.PubSubFirst);

        It should_not_have_more_parts_after_the_first_message = () =>
            message1.HasMoreParts.ShouldBeFalse();

        It should_tell_receiver_to_retry_the_second_message = () =>
            message2.Result.ShouldEqual(ReceiveResult.TryAgain);

        It should_contain_empty_second_message_data = () =>
            message2.Data.ShouldBeEmpty();

        It should_not_have_more_parts_after_the_second_message = () =>
            message2.HasMoreParts.ShouldBeFalse();
    }
}
