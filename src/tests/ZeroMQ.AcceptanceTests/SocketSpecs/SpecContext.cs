namespace ZeroMQ.AcceptanceTests.SocketSpecs
{
    using System;
    using System.Threading;

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

    abstract class using_threaded_req_and_rep_sockets
    {
        protected static IDuplexSocket req;
        protected static IDuplexSocket rep;
        protected static IZmqContext zmqContext;

        protected static Action<IDuplexSocket> reqAction;
        protected static Action<IDuplexSocket> repAction;
        protected static Exception reqException;
        protected static Exception repException;

        private static Thread repThread;
        private static Thread reqThread;

        Establish context = () =>
        {
            zmqContext = ZmqContext.Create();
            req = zmqContext.CreateRequestSocket();
            rep = zmqContext.CreateReplySocket();

            reqAction = sck => { };
            repAction = sck => { };

            reqThread = new Thread(() => reqException = Catch.Exception(() =>
            {
                req.SendHighWatermark = 1;
                req.Connect("inproc://spec_context");
                reqAction(req);
            }));

            repThread = new Thread(() => repException = Catch.Exception(() =>
            {
                rep.ReceiveHighWatermark = 1;
                rep.Bind("inproc://spec_context");
                repAction(rep);
            }));
        };

        Cleanup resources = () =>
        {
            req.Dispose();
            rep.Dispose();
            zmqContext.Dispose();
        };

        protected static void StartThreads()
        {
            repThread.Start();
            reqThread.Start();

            if (!repThread.Join(5000))
            {
                repThread.Abort();
            }

            if (!reqThread.Join(5000))
            {
                reqThread.Abort();
            }
        }
    }
}
