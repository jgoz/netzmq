namespace ZeroMQ.UnitTests.ZmqContextSpecs
{
    using System;

    using Machine.Specifications;

    using Moq;

    using ZeroMQ.Proxy;
    using ZeroMQ.Sockets;

    using It = Machine.Specifications.It;

    [Subject(typeof(ZmqContext), "socket")]
    class when_creating_a_pair_socket : using_context_to_create_socket
    {
        Because of = () =>
            socket = zmqContext.CreatePairSocket();

        It should_use_zmq_pair_socket_type = () =>
            contextProxy.Verify(p => p.CreateSocket((int)SocketType.Pair));
    }

    [Subject(typeof(ZmqContext), "socket")]
    class when_creating_a_pub_socket : using_context_to_create_socket
    {
        Because of = () =>
            socket = zmqContext.CreatePublishSocket();

        It should_use_zmq_pub_socket_type = () =>
            contextProxy.Verify(p => p.CreateSocket((int)SocketType.Pub));
    }

    [Subject(typeof(ZmqContext), "socket")]
    class when_creating_an_xpub_socket : using_context_to_create_socket
    {
        Because of = () =>
            socket = zmqContext.CreatePublishExtSocket();

        It should_use_zmq_xpub_socket_type = () =>
            contextProxy.Verify(p => p.CreateSocket((int)SocketType.Xpub));
    }

    [Subject(typeof(ZmqContext), "socket")]
    class when_creating_a_sub_socket : using_context_to_create_socket
    {
        Because of = () =>
            socket = zmqContext.CreateSubscribeSocket();

        It should_use_zmq_sub_socket_type = () =>
            contextProxy.Verify(p => p.CreateSocket((int)SocketType.Sub));
    }

    [Subject(typeof(ZmqContext), "socket")]
    class when_creating_an_xsub_socket : using_context_to_create_socket
    {
        Because of = () =>
            socket = zmqContext.CreateSubscribeExtSocket();

        It should_use_zmq_xsub_socket_type = () =>
            contextProxy.Verify(p => p.CreateSocket((int)SocketType.Xsub));
    }

    [Subject(typeof(ZmqContext), "socket")]
    class when_creating_a_pull_socket : using_context_to_create_socket
    {
        Because of = () =>
            socket = zmqContext.CreatePullSocket();

        It should_use_zmq_pull_socket_type = () =>
            contextProxy.Verify(p => p.CreateSocket((int)SocketType.Pull));
    }

    [Subject(typeof(ZmqContext), "socket")]
    class when_creating_a_push_socket : using_context_to_create_socket
    {
        Because of = () =>
            socket = zmqContext.CreatePushSocket();

        It should_use_zmq_push_socket_type = () =>
            contextProxy.Verify(p => p.CreateSocket((int)SocketType.Push));
    }

    [Subject(typeof(ZmqContext), "socket")]
    class when_creating_a_rep_socket : using_context_to_create_socket
    {
        Because of = () =>
            socket = zmqContext.CreateReplySocket();

        It should_use_zmq_rep_socket_type = () =>
            contextProxy.Verify(p => p.CreateSocket((int)SocketType.Rep));
    }

    [Subject(typeof(ZmqContext), "socket")]
    class when_creating_a_req_socket : using_context_to_create_socket
    {
        Because of = () =>
            socket = zmqContext.CreateRequestSocket();

        It should_use_zmq_req_socket_type = () =>
            contextProxy.Verify(p => p.CreateSocket((int)SocketType.Req));
    }

    [Subject(typeof(ZmqContext), "socket")]
    class when_creating_a_xrep_socket : using_context_to_create_socket
    {
        Because of = () =>
            socket = zmqContext.CreateReplyExtSocket();

        It should_use_zmq_xrep_socket_type = () =>
            contextProxy.Verify(p => p.CreateSocket((int)SocketType.Xrep));
    }

    [Subject(typeof(ZmqContext), "socket")]
    class when_creating_a_xreq_socket : using_context_to_create_socket
    {
        Because of = () =>
            socket = zmqContext.CreateRequestExtSocket();

        It should_use_zmq_xreq_socket_type = () =>
            contextProxy.Verify(p => p.CreateSocket((int)SocketType.Xreq));
    }

    [Subject(typeof(ZmqContext), "socket")]
    class when_creating_a_dealer_socket : using_context_to_create_socket
    {
        Because of = () =>
            socket = zmqContext.CreateDealerSocket();

        It should_use_zmq_dealer_socket_type = () =>
            contextProxy.Verify(p => p.CreateSocket((int)SocketType.Dealer));
    }

    [Subject(typeof(ZmqContext), "socket")]
    class when_creating_a_router_socket : using_context_to_create_socket
    {
        Because of = () =>
            socket = zmqContext.CreateRouterSocket();

        It should_use_zmq_router_socket_type = () =>
            contextProxy.Verify(p => p.CreateSocket((int)SocketType.Router));
    }

    abstract class using_context_to_create_socket : using_mock_context_proxy
    {
        protected static Mock<ISocketProxy> socketProxy;
        protected static ISocket socket;

        Establish context = () =>
        {
            socketProxy = new Mock<ISocketProxy>();
            factory.Setup(mock => mock.CreateSocket(Moq.It.IsAny<IntPtr>())).Returns(socketProxy.Object);
        };
    }
}
