namespace ThroughputTest
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    using ZeroMQ;
    using ZeroMQ.Sockets;

    internal class Program
    {
        private static readonly int[] MessageSizes = { 4, 8, 64, 128, 256, 512 };

        private const int MessageCount = 1000000;

        public static void Main(string[] args)
        {
            var proxyPull = new Thread(ProxyPullThread);
            var proxyPush = new Thread(ProxyPushThread);

            proxyPull.Start();
            proxyPush.Start();

            proxyPush.Join();
            proxyPull.Join();

            Console.Read();
        }

        private static void ProxyPullThread()
        {
            using (var context = new ZmqContext())
            using (var socket = new PullSocket(context))
            {
                socket.Bind("tcp://*:9091");

                foreach (int messageSize in MessageSizes)
                {
                    ReceivedMessage message = socket.Receive(SocketFlags.None);
                    Debug.Assert(message.Data.Length == messageSize, "Message length was different from expected size.");
                    Debug.Assert(message.Data[messageSize / 2] == 0x42, "Message did not contain verification data.");

                    var watch = new Stopwatch();
                    watch.Start();

                    for (int i = 1; i < MessageCount; i++)
                    {
                        message = socket.Receive(SocketFlags.None);
                        Debug.Assert(message.Data.Length == messageSize, "Message length was different from expected size.");
                        Debug.Assert(message.Data[messageSize / 2] == 0x42, "Message did not contain verification data.");
                    }

                    GC.AddMemoryPressure(messageSize * MessageCount);

                    watch.Stop();

                    long elapsedTime = watch.ElapsedTicks;
                    long messageThroughput = MessageCount * Stopwatch.Frequency / elapsedTime;
                    long megabitThroughput = messageThroughput * messageSize * 8 / 1000000;

                    Console.WriteLine("Message size: {0} [B]", messageSize);
                    Console.WriteLine("Average throughput: {0} [msg/s]", messageThroughput);
                    Console.WriteLine("Average throughput: {0} [Mb/s]", megabitThroughput);
                }
            }
        }

        private static void ProxyPushThread()
        {
            using (var context = new ZmqContext())
            using (var socket = new PushSocket(context))
            {
                socket.Connect("tcp://127.0.0.1:9091");

                foreach (int messageSize in MessageSizes)
                {
                    var msg = new byte[messageSize];
                    msg[messageSize / 2] = 0x42;

                    for (int i = 0; i < MessageCount; i++)
                    {
                        socket.Send(msg, SocketFlags.None);
                    }
                }
            }
        }
    }
}
