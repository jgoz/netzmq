﻿namespace LatencyTest
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    using ZeroMQ;
    using ZeroMQ.Sockets;

    internal class Program
    {
        private static readonly int[] MessageSizes = { 8, 64, 512, 4096, 32768 };

        private const int RoundtripCount = 10000;

        public static void Main(string[] args)
        {
            var proxyRequest = new Thread(ProxyRequestThread);
            var proxyReply = new Thread(ProxyReplyThread);

            Console.WriteLine();
            Console.WriteLine("Proxy Sockets");
            Console.WriteLine();

            proxyReply.Start();
            proxyRequest.Start();

            proxyRequest.Join();
            proxyReply.Join();

            Console.Read();
        }

        private static void ProxyRequestThread()
        {
            using (var context = ZmqContext.Create())
            using (var socket = context.CreateRequestSocket())
            {
                socket.Connect("tcp://127.0.0.1:9091");

                foreach (int messageSize in MessageSizes)
                {
                    var msg = new byte[messageSize];
                    byte[] reply;

                    var watch = new Stopwatch();
                    watch.Start();

                    for (int i = 0; i < RoundtripCount; i++)
                    {
                        SendResult result = socket.Send(msg);

                        Debug.Assert(result == SendResult.Sent, "Message was not indicated as sent.");

                        reply = socket.Receive();

                        Debug.Assert(reply.Length == messageSize, "Pong message did not have the expected size.");

                        msg = reply;
                    }

                    watch.Stop();
                    long elapsedTime = watch.ElapsedTicks;

                    Console.WriteLine("Message size: " + messageSize + " [B]");
                    Console.WriteLine("Roundtrips: " + RoundtripCount);

                    double latency = (double)elapsedTime / RoundtripCount / 2 * 1000000 / Stopwatch.Frequency;
                    Console.WriteLine("Your average latency is {0} [us]", latency.ToString("f2"));
                }
            }
        }

        private static void ProxyReplyThread()
        {
            using (var context = ZmqContext.Create())
            using (var socket = context.CreateReplySocket())
            {
                socket.Bind("tcp://*:9091");

                foreach (int messageSize in MessageSizes)
                {
                    for (int i = 0; i < RoundtripCount; i++)
                    {
                        byte[] message = socket.Receive();

                        Debug.Assert(socket.ReceiveStatus == ReceiveResult.Received, "Message result was non-successful.");
                        Debug.Assert(message.Length == messageSize, "Message length did not match expected value.");

                        socket.Send(message);
                    }
                }
            }
        }
    }
}
