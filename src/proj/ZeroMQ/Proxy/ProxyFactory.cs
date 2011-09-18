namespace ZeroMQ.Proxy
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    internal static class ProxyFactory
    {
        private const string InteropAssemblyName = "ZeroMQ.Proxy";

        private static readonly Assembly NativeAssembly;

        private static readonly Type SocketContextProxyType;
        private static readonly Type SocketProxyType;

        static ProxyFactory()
        {
            NativeAssembly = LoadInteropAssembly();

            if (NativeAssembly == null)
            {
                throw new FileNotFoundException("Unable to find suitable ZeroMQ.Proxy assembly.");
            }

            SocketContextProxyType = NativeAssembly.GetTypes().Single(t => typeof(ISocketContextProxy).IsAssignableFrom(t));
            SocketProxyType = NativeAssembly.GetTypes().Single(t => typeof(ISocketProxy).IsAssignableFrom(t));

            Type errorProviderType = NativeAssembly.GetTypes().Single(t => typeof(IErrorProviderProxy).IsAssignableFrom(t));
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

        private static Assembly LoadInteropAssembly()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;

            return Assembly.Load(InteropAssemblyName);
        }

        private static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            if (!args.Name.StartsWith(InteropAssemblyName))
            {
                return Assembly.Load(args.Name);
            }

            // TODO: Try loading normally first, then write manifest resource to disk

            string shortName = new AssemblyName(args.Name).Name;

            return Assembly.LoadFrom(GetAssemblyPath(shortName));
        }

        private static string GetAssemblyPath(string shortName)
        {
            return Path.Combine(Environment.Is64BitProcess ? "x64" : "x86", shortName + ".dll");
        }
    }
}
