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

            SocketContextProxyType = ProxyAssembly.GetTypes().Single(t => typeof(IContextProxy).IsAssignableFrom(t));
            SocketProxyType = ProxyAssembly.GetTypes().Single(t => typeof(ISocketProxy).IsAssignableFrom(t));

            Type errorProviderType = ProxyAssembly.GetTypes().Single(t => typeof(IErrorProviderProxy).IsAssignableFrom(t));
            ErrorProvider = (IErrorProviderProxy)Activator.CreateInstance(errorProviderType);
        }

        public static IErrorProviderProxy ErrorProvider { get; private set; }

        public static IContextProxy CreateContext(int threadPoolSize)
        {
            return (IContextProxy)Activator.CreateInstance(SocketContextProxyType, threadPoolSize);
        }

        public static ISocketProxy CreateSocket(IntPtr context, int socketType)
        {
            return (ISocketProxy)Activator.CreateInstance(SocketProxyType, context, socketType);
        }
    }
}
