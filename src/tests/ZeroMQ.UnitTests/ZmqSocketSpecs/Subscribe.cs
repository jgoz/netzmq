namespace ZeroMQ.UnitTests.ZmqSocketSpecs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Machine.Specifications;

    using ZeroMQ.Proxy;
    using ZeroMQ.Sockets;

    [Subject(typeof(SubscribeSocket), "subscribe prefix")]
    class when_subscribing_to_a_specific_prefix : using_subscribe_socket
    {
        Because of = () =>
            socket.Subscribe(prefix);

        It should_set_zmq_sub_socket_option = () =>
            socketProxy.Verify(mock => mock.SetSocketOption((int)SocketOption.Subscribe, prefix));
    }

    [Subject(typeof(SubscribeSocket), "subscribe prefix")]
    class when_subscribing_to_a_null_prefix : using_subscribe_socket
    {
        static Exception exception;

        Because of = () =>
            exception = Catch.Exception(() => socket.Subscribe(null));

        It should_fail = () =>
            exception.ShouldBeOfType<ArgumentNullException>();
    }

    [Subject(typeof(SubscribeSocket), "subscribe all")]
    class when_subscribing_to_all_messages : using_subscribe_socket
    {
        Because of = () =>
            socket.SubscribeAll();

        It should_set_zmq_sub_socket_option = () =>
            socketProxy.Verify(mock => mock.SetSocketOption((int)SocketOption.Subscribe, new byte[] { }));
    }

    [Subject(typeof(SubscribeSocket), "unsubscribe prefix")]
    class when_unsubscribing_from_a_specific_prefix : using_subscribe_socket
    {
        Because of = () =>
            socket.Unsubscribe(prefix);

        It should_set_zmq_sub_socket_option = () =>
            socketProxy.Verify(mock => mock.SetSocketOption((int)SocketOption.Unsubscribe, prefix));
    }

    [Subject(typeof(SubscribeSocket), "subscribe prefix")]
    class when_unsubscribing_from_a_null_prefix : using_subscribe_socket
    {
        static Exception exception;

        Because of = () =>
            exception = Catch.Exception(() => socket.Unsubscribe(null));

        It should_fail = () =>
            exception.ShouldBeOfType<ArgumentNullException>();
    }

    [Subject(typeof(SubscribeSocket), "unsubscribe all")]
    class when_unsubscribing_from_all_messages : using_subscribe_socket
    {
        Because of = () =>
            socket.UnsubscribeAll();

        It should_set_zmq_sub_socket_option = () =>
            socketProxy.Verify(mock => mock.SetSocketOption((int)SocketOption.Unsubscribe, new byte[0]));
    }

    [Subject(typeof(SubscribeExtSocket), "subscribe prefix")]
    class when_sending_subscription_to_a_specific_prefix : using_subscribe_ext_socket
    {
        Because of = () =>
            socket.Subscribe(prefix);

        It should_send_subscription_message = () =>
            socketProxy.Verify(mock => mock.Send(Moq.It.IsAny<int>(), Moq.It.Is(SubscriptionMessage(prefix))));
    }

    [Subject(typeof(SubscribeExtSocket), "subscribe prefix")]
    class when_sending_subscription_to_a_null_prefix : using_subscribe_ext_socket
    {
        static Exception exception;

        Because of = () =>
            exception = Catch.Exception(() => socket.Subscribe(null));

        It should_fail = () =>
            exception.ShouldBeOfType<ArgumentNullException>();
    }

    [Subject(typeof(SubscribeExtSocket), "subscribe all")]
    class when_sending_subscription_to_all_messages : using_subscribe_ext_socket
    {
        Because of = () =>
            socket.SubscribeAll();

        It should_send_subscription_message = () =>
            socketProxy.Verify(mock => mock.Send(Moq.It.IsAny<int>(), Moq.It.Is(SubscriptionMessage(new byte[0]))));
    }

    [Subject(typeof(SubscribeExtSocket), "unsubscribe prefix")]
    class when_sending_unsubscription_from_a_specific_prefix : using_subscribe_ext_socket
    {
        Because of = () =>
            socket.Unsubscribe(prefix);

        It should_send_unsubscription_message = () =>
            socketProxy.Verify(mock => mock.Send(Moq.It.IsAny<int>(), Moq.It.Is(UnsubscriptionMessage(prefix))));
    }

    [Subject(typeof(SubscribeExtSocket), "subscribe prefix")]
    class when_sending_unsubscription_from_a_null_prefix : using_subscribe_ext_socket
    {
        static Exception exception;

        Because of = () =>
            exception = Catch.Exception(() => socket.Unsubscribe(null));

        It should_fail = () =>
            exception.ShouldBeOfType<ArgumentNullException>();
    }

    [Subject(typeof(SubscribeExtSocket), "unsubscribe all")]
    class when_sending_unsubscription_from_all_messages : using_subscribe_ext_socket
    {
        Because of = () =>
            socket.UnsubscribeAll();

        It should_send_unsubscription_message = () =>
            socketProxy.Verify(mock => mock.Send(Moq.It.IsAny<int>(), Moq.It.Is(UnsubscriptionMessage(new byte[0]))));
    }

    abstract class using_subscribe_socket : using_mock_socket_proxy<ISubscribeSocket>
    {
        protected static byte[] prefix = "Filter".ToZmqBuffer();

        Establish context = () =>
            socket = new SubscribeSocket(socketProxy.Object, errorProviderProxy.Object);
    }

    abstract class using_subscribe_ext_socket : using_mock_socket_proxy<ISubscribeSocket>
    {
        protected static byte[] prefix = "Filter".ToZmqBuffer();

        Establish context = () =>
            socket = new SubscribeExtSocket(socketProxy.Object, errorProviderProxy.Object);

        protected static Expression<Func<byte[], bool>> SubscriptionMessage(IEnumerable<byte> expected)
        {
            return bytes => bytes.Length >= 1 && bytes[0] == SubscribeExtSocket.SubscribePrefix && bytes.Skip(1).SequenceEqual(expected);
        }

        protected static Expression<Func<byte[], bool>> UnsubscriptionMessage(IEnumerable<byte> expected)
        {
            return bytes => bytes.Length >= 1 && bytes[0] == SubscribeExtSocket.UnsubscribePrefix && bytes.Skip(1).SequenceEqual(expected);
        }
    }
}
