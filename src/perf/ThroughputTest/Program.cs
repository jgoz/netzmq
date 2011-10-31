namespace ThroughputTest
{
    using System;
    using System.Diagnostics;
    using System.Threading;

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
            using (var context = ZmqContext.Create())
            using (var socket = context.CreatePullSocket())
            {
                socket.Bind("tcp://*:9091");

                foreach (int messageSize in MessageSizes)
                {
                    byte[] message = socket.Receive();
                    Debug.Assert(message.Length == messageSize, "Message length was different from expected size.");
                    Debug.Assert(message[messageSize / 2] == 0x42, "Message did not contain verification data.");

                    var watch = new Stopwatch();
                    watch.Start();

                    for (int i = 1; i < MessageCount; i++)
                    {
                        message = socket.Receive();
                        Debug.Assert(message.Length == messageSize, "Message length was different from expected size.");
                        Debug.Assert(message[messageSize / 2] == 0x42, "Message did not contain verification data.");
                    }

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
            using (var context = ZmqContext.Create())
            using (var socket = context.CreatePushSocket())
            {
                socket.Connect("tcp://127.0.0.1:9091");

                foreach (int messageSize in MessageSizes)
                {
                    var msg = new byte[messageSize];
                    msg[messageSize / 2] = 0x42;

                    for (int i = 0; i < MessageCount; i++)
                    {
                        socket.Send(msg);
                    }
                }
            }
        }
    }
}
