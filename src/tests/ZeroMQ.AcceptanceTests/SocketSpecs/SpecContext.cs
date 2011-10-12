namespace ZeroMQ.AcceptanceTests.SocketSpecs
{
    using System;

    using Machine.Specifications;

    using ZeroMQ.Sockets;

    abstract class using_req_socket
    {
        protected static ISocket socket;
        protected static IZmqContext zmqContext;
        protected static Exception exception;

        Establish context = () =>
        {
            zmqContext = ZmqContext.Create();
            socket = zmqContext.CreateRequestSocket();
        };

        Cleanup resources = () =>
        {
            socket.Dispose();
            zmqContext.Dispose();
        };
    }

    abstract class using_req_and_rep_sockets
    {
        protected static IDuplexSocket req;
        protected static IDuplexSocket rep;
        protected static IZmqContext zmqContext;
        protected static Exception exception;

        Establish context = () =>
        {
            zmqContext = ZmqContext.Create();
            req = zmqContext.CreateRequestSocket();
            rep = zmqContext.CreateReplySocket();
        };

        Cleanup resources = () =>
        {
            req.Dispose();
            rep.Dispose();
            zmqContext.Dispose();
        };
    }

    abstract class using_pub_and_sub_sockets
    {
        protected static ISendSocket pub;
        protected static ISubscribeSocket sub;
        protected static IZmqContext zmqContext;
        protected static Exception exception;

        Establish context = () =>
        {
            zmqContext = ZmqContext.Create();
            pub = zmqContext.CreatePublishSocket();
            sub = zmqContext.CreateSubscribeSocket();
        };

        Cleanup resources = () =>
        {
            sub.Dispose();
            pub.Dispose();
            zmqContext.Dispose();
        };
    }
}
