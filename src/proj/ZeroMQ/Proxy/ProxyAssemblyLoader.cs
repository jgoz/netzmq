namespace ZeroMQ.Proxy
{
    using System;
    using System.IO;
    using System.Reflection;

    internal static class ProxyAssemblyLoader
    {
        private const string ProxyAssemblyFileName = "ZeroMQ.Proxy.dll";
        private const string NativeDllName = "libzmq.dll";
        private const string LoadError = "Unable to load {0} for {1} platform.";
        private const string WriteAccessRequired = " Write access to the current executing path or a temporary folder is required.";

        private static readonly string CurrentPlatform = Environment.Is64BitProcess ? "x64" : "x86";

        public static Assembly Load()
        {
            Assembly assembly = LoadFromLocalBinPath() ?? LoadFromExecutingPath() ?? LoadFromTempPath();

            if (assembly == null)
            {
                throw new InvalidOperationException(string.Format(LoadError, ProxyAssemblyFileName, CurrentPlatform) + WriteAccessRequired);
            }

            return assembly;
        }

        private static Assembly LoadFromLocalBinPath()
        {
            return ExtractAndLoadFromPath("bin");
        }

        private static Assembly LoadFromExecutingPath()
        {
            return ExtractAndLoadFromPath(".");
        }

        private static Assembly LoadFromTempPath()
        {
            string dir = Path.Combine(Path.GetTempPath(), "netzmq", CurrentPlatform);
            Directory.CreateDirectory(dir);

            return ExtractAndLoadFromPath(dir);
        }

        private static Assembly ExtractAndLoadFromPath(string dir)
        {
            if (!Directory.Exists(dir))
            {
                return null;
            }

            string proxyPath = Path.GetFullPath(Path.Combine(dir, ProxyAssemblyFileName));
            string nativePath = Path.GetFullPath(Path.Combine(dir, NativeDllName));
            string platformSuffix = "." + CurrentPlatform;

            if (!ExtractManifestResource(NativeDllName + platformSuffix, nativePath))
            {
                return null;
            }

            if (!ExtractManifestResource(ProxyAssemblyFileName + platformSuffix, proxyPath))
            {
                return null;
            }

            return Assembly.LoadFrom(proxyPath);
        }

        private static bool ExtractManifestResource(string resourceName, string outputPath)
        {
            if (File.Exists(outputPath))
            {
                // There is potential for version conflicts by doing this. However, it is necessary to
                // prevent file access conflicts if multiple processes are run from the same location.

                // TODO: Implement a better file conflict prevention scheme
                return true;
            }

            Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

            if (resourceStream == null)
            {
                // No manifest resources were compiled into the current assembly. This is likely a 'manual
                // deployment' situation, so do not throw an exception at this point and allow all deployment
                // paths to be searched.
                return false;
            }

            try
            {
                using (FileStream fileStream = File.Create(outputPath))
                {
                    resourceStream.CopyTo(fileStream);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Caller does not have write permission for the current file
                return false;
            }

            return true;
        }
    }
}
