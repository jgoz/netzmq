using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("netzmq")]
[assembly: AssemblyDescription("Bindings for the ZeroMQ transport layer.")]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("d8ffb8c6-83cb-4eb1-ac98-1647546fd399")]

[assembly: InternalsVisibleTo("ZeroMQ.Proxy")]
[assembly: InternalsVisibleTo("ZeroMQ.UnitTests")]

// Allow Moq/CastleProxy to see internals
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]