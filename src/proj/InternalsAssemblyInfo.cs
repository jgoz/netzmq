using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ZeroMQ.Proxy")]
[assembly: InternalsVisibleTo("ZeroMQ.UnitTests")]

// Allow Moq/CastleProxy to see internals
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
