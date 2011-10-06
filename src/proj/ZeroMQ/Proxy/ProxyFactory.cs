namespace ZeroMQ.Proxy
{
    using System;
    using System.Linq;
    using System.Reflection;

    internal class ProxyFactory : IProxyFactory
    {
        private readonly Type contextProxyType;
        private readonly Type socketProxyType;
        private readonly Type pollSetProxyType;
        private readonly Type deviceProxyType;

        private ProxyFactory(Type contextProxyType, Type socketProxyType, Type pollSetProxyType, Type deviceProxyType, IErrorProviderProxy errorProviderProxy)
        {
            this.contextProxyType = contextProxyType;
            this.socketProxyType = socketProxyType;
            this.pollSetProxyType = pollSetProxyType;
            this.deviceProxyType = deviceProxyType;
            this.ErrorProvider = errorProviderProxy;
        }

        public IErrorProviderProxy ErrorProvider { get; private set; }

        public static IProxyFactory Create()
        {
            Assembly proxyAssembly = ProxyAssemblyLoader.Load();

            Type contextProxyType = GetSingleImplementor<IContextProxy>(proxyAssembly);
            Type socketProxyType = GetSingleImplementor<ISocketProxy>(proxyAssembly);
            Type pollSetProxyType = GetSingleImplementor<IPollSetProxy>(proxyAssembly);
            Type deviceProxyType = GetSingleImplementor<IDeviceProxy>(proxyAssembly);

            Type errorProviderType = GetSingleImplementor<IErrorProviderProxy>(proxyAssembly);
            var errorProviderProxy = (IErrorProviderProxy)Activator.CreateInstance(errorProviderType);

            return new ProxyFactory(contextProxyType, socketProxyType, pollSetProxyType, deviceProxyType, errorProviderProxy);
        }

        public IContextProxy CreateContext(int threadPoolSize)
        {
            return (IContextProxy)Activator.CreateInstance(this.contextProxyType, threadPoolSize);
        }

        public ISocketProxy CreateSocket(IntPtr socket)
        {
            return (ISocketProxy)Activator.CreateInstance(this.socketProxyType, socket);
        }

        public IPollSetProxy CreatePollSet(int socketCount)
        {
            return (IPollSetProxy)Activator.CreateInstance(this.pollSetProxyType, socketCount);
        }

        public IDeviceProxy CreateDevice(IntPtr inSocket, IntPtr outSocket)
        {
            return (IDeviceProxy)Activator.CreateInstance(this.deviceProxyType, inSocket, outSocket);
        }

        private static Type GetSingleImplementor<TInterface>(Assembly assembly)
        {
            var matchingTypes = assembly.GetTypes().Where(t => typeof(TInterface).IsAssignableFrom(t));

            if (!matchingTypes.Any())
            {
                throw new TypeLoadException(string.Format(
                    "Error loading ZeroMQ Proxy assembly. Concrete implementation of '{0}' not found.",
                    typeof(TInterface).Name));
            }

            if (matchingTypes.Count() > 1)
            {
                throw new TypeLoadException(string.Format(
                    "Error loading ZeroMQ Proxy assembly. More than one concrete implementation of '{0}' was found.",
                    typeof(TInterface).Name));
            }

            return matchingTypes.Single();
        }
    }
}
