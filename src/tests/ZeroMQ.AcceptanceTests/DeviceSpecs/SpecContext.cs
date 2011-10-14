namespace ZeroMQ.AcceptanceTests.DeviceSpecs
{
    using System;
    using System.Threading;

    using Machine.Specifications;

    using ZeroMQ.Sockets;
    using ZeroMQ.Sockets.Devices;

    abstract class using_threaded_device<TSendSocket, TReceiveSocket>
        where TSendSocket : class, ISocket
        where TReceiveSocket : class, ISocket
    {
        protected static Func<TSendSocket> createSender;
        protected static Func<TReceiveSocket> createReceiver;
        protected static Func<ZmqDevice<TReceiveSocket, TSendSocket>> createDevice;

        protected static TSendSocket sender;
        protected static TReceiveSocket receiver;
        protected static ZmqDevice<TReceiveSocket, TSendSocket> device;
        protected static IZmqContext zmqContext;

        protected static Action<ZmqDevice<TReceiveSocket, TSendSocket>> deviceInit;
        protected static Action<TSendSocket> senderInit;
        protected static Action<TSendSocket> senderAction;
        protected static Action<TReceiveSocket> receiverInit;
        protected static Action<TReceiveSocket> receiverAction;

        private static Thread deviceThread;
        private static Thread receiverThread;
        private static Thread senderThread;
        private static AutoResetEvent signal;

        Establish context = () =>
        {
            zmqContext = ZmqContext.Create();
            device = createDevice();
            sender = createSender();
            receiver = createReceiver();

            deviceInit = dev => { };
            senderInit = sck => { };
            receiverInit = sck => { };
            senderAction = sck => { };
            receiverAction = sck => { };

            signal = new AutoResetEvent(false);

            deviceThread = new Thread(() =>
            {
                deviceInit(device);

                device.ConfigureFrontend().BindTo("inproc://dev_frontend");
                device.ConfigureBackend().BindTo("inproc://dev_backend");
                device.InitializeSockets();

                signal.Set();

                device.Start();
            });

            receiverThread = new Thread(() =>
            {
                signal.WaitOne();

                receiverInit(receiver);
                receiver.ReceiveHighWatermark = 1;
                receiver.Connect("inproc://dev_backend");

                signal.Set();

                receiverAction(receiver);
            });

            senderThread = new Thread(() =>
            {
                signal.WaitOne();

                senderInit(sender);
                sender.SendHighWatermark = 1;
                sender.Connect("inproc://dev_frontend");

                signal.Set();

                senderAction(sender);
            });
        };

        Cleanup resources = () =>
        {
            sender.Dispose();
            receiver.Dispose();
            device.Dispose();
            zmqContext.Dispose();
        };

        protected static void StartThreads()
        {
            deviceThread.Start();
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

            device.Stop();

            if (!deviceThread.Join(5000))
            {
                deviceThread.Abort();
            }
        }
    }
}
