#pragma warning disable 649

namespace ZeroMQ.AcceptanceTests
{
    using Machine.Specifications;

    using ZeroMQ.Sockets;

    static class Messages
    {
        public static readonly byte[] SingleMessage = "Test message".ZmqEncode();
        public static readonly byte[] MultiFirst = "First".ZmqEncode();
        public static readonly byte[] MultiLast = "Last".ZmqEncode();

        public static readonly byte[] PubSubPrefix = "PREFIX".ZmqEncode();
        public static readonly byte[] PubSubFirst = "PREFIX Test message".ZmqEncode();
        public static readonly byte[] PubSubSecond = "NOPREFIX Test message".ZmqEncode();
    }

    [Behaviors]
    class SingleMessageSuccess
    {
        protected static IReceiveSocket receiver;
        protected static byte[] message;
        protected static SendResult sendResult;

        It should_be_sent_successfully = () =>
            sendResult.ShouldEqual(SendResult.Sent);

        It should_contain_the_given_message = () =>
            message.ShouldEqual(Messages.SingleMessage);

        It should_be_successfully_received = () =>
            receiver.ReceiveStatus.ShouldEqual(ReceiveResult.Received);

        It should_not_have_more_parts = () =>
            receiver.ReceiveMore.ShouldBeFalse();
    }

    [Behaviors]
    class SingleMessageTryAgain
    {
        protected static IReceiveSocket receiver;
        protected static byte[] message;

        It should_not_contain_the_given_message = () =>
            message.ShouldBeEmpty();

        It should_require_the_receiver_to_try_again = () =>
            receiver.ReceiveStatus.ShouldEqual(ReceiveResult.TryAgain);

        It should_not_have_more_parts = () =>
            receiver.ReceiveMore.ShouldBeFalse();
    }

    [Behaviors]
    class MultiPartMessageSuccess
    {
        protected static byte[] message1;
        protected static byte[] message2;
        protected static SendResult sendResult1;
        protected static SendResult sendResult2;
        protected static ReceiveResult receiveResult1;
        protected static ReceiveResult receiveResult2;
        protected static bool receiveMore1;
        protected static bool receiveMore2;

        It should_send_the_first_message_successfully = () =>
            sendResult1.ShouldEqual(SendResult.Sent);

        It should_send_the_second_message_successfully = () =>
            sendResult2.ShouldEqual(SendResult.Sent);

        It should_receive_the_first_message_successfully = () =>
            receiveResult1.ShouldEqual(ReceiveResult.Received);

        It should_contain_the_correct_first_message_data = () =>
            message1.ShouldEqual(Messages.MultiFirst);

        It should_have_more_parts_after_the_first_message = () =>
            receiveMore1.ShouldBeTrue();

        It should_receive_the_second_message_successfully = () =>
            receiveResult2.ShouldEqual(ReceiveResult.Received);

        It should_contain_the_correct_second_message_data = () =>
            message2.ShouldEqual(Messages.MultiLast);

        It should_not_have_more_parts_after_the_second_message = () =>
            receiveMore2.ShouldBeFalse();
    }

    [Behaviors]
    class PubSubReceiveAll
    {
        protected static byte[] message1;
        protected static byte[] message2;
        protected static SendResult sendResult1;
        protected static SendResult sendResult2;
        protected static ReceiveResult receiveResult1;
        protected static ReceiveResult receiveResult2;
        protected static bool receiveMore1;
        protected static bool receiveMore2;

        It should_send_the_first_message_successfully = () =>
            sendResult1.ShouldEqual(SendResult.Sent);

        It should_send_the_second_message_successfully = () =>
            sendResult2.ShouldEqual(SendResult.Sent);

        It should_receive_the_first_message_successfully = () =>
            receiveResult1.ShouldEqual(ReceiveResult.Received);

        It should_contain_the_correct_first_message_data = () =>
            message1.ShouldEqual(Messages.PubSubFirst);

        It should_not_have_more_parts_after_the_first_message = () =>
            receiveMore1.ShouldBeFalse();

        It should_receive_the_second_message_successfully = () =>
            receiveResult2.ShouldEqual(ReceiveResult.Received);

        It should_contain_the_correct_second_message_data = () =>
            message2.ShouldEqual(Messages.PubSubSecond);

        It should_not_have_more_parts_after_the_second_message = () =>
            receiveMore2.ShouldBeFalse();
    }

    [Behaviors]
    class PubSubReceiveFirst
    {
        protected static byte[] message1;
        protected static byte[] message2;
        protected static SendResult sendResult1;
        protected static SendResult sendResult2;
        protected static ReceiveResult receiveResult1;
        protected static ReceiveResult receiveResult2;
        protected static bool receiveMore1;
        protected static bool receiveMore2;

        It should_send_the_first_message_successfully = () =>
            sendResult1.ShouldEqual(SendResult.Sent);

        It should_send_the_second_message_successfully = () =>
            sendResult2.ShouldEqual(SendResult.Sent);

        It should_receive_the_first_message_successfully = () =>
            receiveResult1.ShouldEqual(ReceiveResult.Received);

        It should_contain_the_correct_first_message_data = () =>
            message1.ShouldEqual(Messages.PubSubFirst);

        It should_not_have_more_parts_after_the_first_message = () =>
            receiveMore1.ShouldBeFalse();

        It should_tell_receiver_to_retry_the_second_message = () =>
            receiveResult2.ShouldEqual(ReceiveResult.TryAgain);

        It should_contain_empty_second_message_data = () =>
            message2.ShouldBeEmpty();

        It should_not_have_more_parts_after_the_second_message = () =>
            receiveMore2.ShouldBeFalse();
    }
}

#pragma warning restore 649