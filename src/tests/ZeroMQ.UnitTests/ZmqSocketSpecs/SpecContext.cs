﻿namespace ZeroMQ.UnitTests.ZmqSocketSpecs
{
    using System;

    using Machine.Specifications;

    using Moq;

    using ZeroMQ.Proxy;
    using ZeroMQ.Sockets;

    using It = Machine.Specifications.It;

    [Behaviors]
    class FailsWithSocketException
    {
        protected static Exception exception;

        It should_fail_with_socket_exception = () =>
            exception.ShouldBeOfType<ZmqSocketException>();
    }

    abstract class using_base_socket_class : using_mock_socket_proxy<ZmqSocket>
    {
        Establish context = () =>
            socket = new ZmqSocket(socketProxy.Object, errorProviderProxy.Object);
    }

    abstract class using_mock_socket_proxy<TSocket> where TSocket : ISocket
    {
        protected static Mock<ISocketProxy> socketProxy;
        protected static Mock<IErrorProviderProxy> errorProviderProxy;
        protected static TSocket socket;

        Establish context = () =>
        {
            socketProxy = new Mock<ISocketProxy>();
            errorProviderProxy = new Mock<IErrorProviderProxy>();
        };

        protected static int IncludesSocketFlag(SocketFlags flag)
        {
            return Moq.It.Is<int>(i => ((SocketFlags)i).HasFlag(flag));
        }

        protected static int ExcludesSocketFlag(SocketFlags flag)
        {
            return Moq.It.Is<int>(i => !((SocketFlags)i).HasFlag(flag));
        }
    }
}
