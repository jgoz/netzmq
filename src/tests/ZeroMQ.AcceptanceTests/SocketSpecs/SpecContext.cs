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

    abstract class using_threaded_req_rep : using_threaded_socket_pair<IDuplexSocket, IDuplexSocket>
    {
        static using_threaded_req_rep()
        {
            createSender = () => zmqContext.CreateRequestSocket();
            createReceiver = () => zmqContext.CreateReplySocket();
        }
    }

    abstract class using_threaded_pub_sub : using_threaded_socket_pair<ISendSocket, ISubscribeSocket>
    {
        static using_threaded_pub_sub()
        {
            createSender = () => zmqContext.CreatePublishSocket();
            createReceiver = () => zmqContext.CreateSubscribeSocket();
        }
    }

    abstract class using_threaded_socket_pair<TSendSocket, TReceiveSocket>
        where TSendSocket : ISendSocket
        where TReceiveSocket : IReceiveSocket
    {
        protected static Func<TSendSocket> createSender;
        protected static Func<TReceiveSocket> createReceiver;

        protected static TSendSocket sender;
        protected static TReceiveSocket receiver;
        protected static IZmqContext zmqContext;

        protected static Action<TSendSocket> senderAction;
        protected static Action<TReceiveSocket> receiverAction;

        private static Thread receiverThread;
        private static Thread senderThread;

        Establish context = () =>
        {
            zmqContext = ZmqContext.Create();
            sender = createSender();
            receiver = createReceiver();

            senderAction = sck => { };
            receiverAction = sck => { };

            senderThread = new Thread(() =>
            {
                sender.SendHighWatermark = 1;
                sender.Connect("inproc://spec_context");
                senderAction(sender);
            });

            receiverThread = new Thread(() =>
            {
                receiver.ReceiveHighWatermark = 1;
                receiver.Bind("inproc://spec_context");
                receiverAction(receiver);
            });
        };

        Cleanup resources = () =>
        {
            sender.Dispose();
            receiver.Dispose();
            zmqContext.Dispose();
        };

        protected static void StartThreads()
        {
            receiverThread.Start();
            senderThread.Start();

            if (!receiverThread.Join(5000))
            {
                receiverThread.Abort();
            }

            if (!senderThread.Join(5000))
            {
                senderThread.Abort();
            }
        }
    }
}
