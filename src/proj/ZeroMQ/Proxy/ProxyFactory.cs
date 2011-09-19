namespace ZeroMQ.Proxy
{
    using System;
    using System.Linq;
    using System.Reflection;

    internal static class ProxyFactory
    {

        private static readonly Assembly ProxyAssembly;

        private static readonly Type SocketContextProxyType;
        private static readonly Type SocketProxyType;

        static ProxyFactory()
        {
            ProxyAssembly = ProxyAssemblyLoader.Load();

            SocketContextProxyType = ProxyAssembly.GetTypes().Single(t => typeof(ISocketContextProxy).IsAssignableFrom(t));
            SocketProxyType = ProxyAssembly.GetTypes().Single(t => typeof(ISocketProxy).IsAssignableFrom(t));

            Type errorProviderType = ProxyAssembly.GetTypes().Single(t => typeof(IErrorProviderProxy).IsAssignableFrom(t));
            ErrorProvider = (IErrorProviderProxy)Activator.CreateInstance(errorProviderType);
        }

        public static IErrorProviderProxy ErrorProvider { get; private set; }

        public static ISocketContextProxy CreateSocketContext(int threadPoolSize)
        {
            return (ISocketContextProxy)Activator.CreateInstance(SocketContextProxyType, threadPoolSize);
        }

        public static ISocketProxy CreateSocket(IntPtr context, SocketType socketType)
        {
            return (ISocketProxy)Activator.CreateInstance(SocketProxyType, context, socketType);
        }
    }
}
