# netzmq &mdash; ZeroMQ bindings for .NET and Mono

NOTE: These are not the official .NET bindings. Until this project is stable, you should use the [official bindings][clrzmq], which are also available [via NuGet][clrzmq-nuget].

## Goals
* Provide an idiomatic .NET 4.0 API for ZeroMQ (for some definition of *idiomatic*)
* Support the Microsoft CLR and the Mono CLR
* Fully support unit-testing and mocking in client projects
* Provide a smooth transition to ZMQ 3.0 when it is released
* Exclusively target the AnyCPU platform for deployments that "just work", even on ASP.NET/IIS

## Example API Usage

```c#
// Client socket
using (var ctx = ZmqContext.Create())
using (var requestSocket = ctx.CreateRequestSocket())
{
    requestSocket.Connect("tcp://127.0.0.1:9001");
    requestSocket.Send("Hello world!".ToZmqBuffer());
}

// Server socket
using (var ctx = ZmqContext.Create())
using (var replySocket = ctx.CreateReplySocket())
{
    replySocket.Bind("tcp://*:9001");
    ReceivedMessage msg = replySocket.Receive();

    Console.WriteLine("Message received: {0}", msg);
}
```

## Build Instructions
This project currently depends on [libzmq 3.0][libzmq-src]. In order to build netzmq, the build output from libzmq must be placed in the solution hierarchy. A rough guide follows:

1. Clone [libzmq][libzmq-src]
2. Open builds\msvc\msvc.sln in Visual Studio
3. Set the solution build configuration to Release/x86
4. Build the `libzmq` project
5. Copy `builds\msvc\Release\libzmq.{dll,exp,lib}` to `lib\x86` in your netzmq repository 
6. Repeat steps 3-5 using the Release/x64 configuration, copying to `lib\x64`
7. Copy `include\zmq.h` to `include` in your netzmq repository
8. Run `build.cmd`

To include OpenPGM support, extract the included libpgm tarball to a path inside libzmq and perform the above steps using `WithOpenPGM` as the build configuration instead of `Release`. Project and library reference paths will need to be updated to reflect the OpenPGM extract path.

NOTE: The netzmq build scripts currently assume a 64-bit build environment.

## *Another* .NET binding for ZMQ&#x203d;
This project is first and foremost a means to learn ZeroMQ more deeply. While there are other projects that provide .NET bindings for ZeroMQ, the secondary goals of this project are sufficiently different to justify creating a new library, at least for experimentation. If any of the ideas in this project appeal to the maintainers of other bindings, I'd be happy to collaborate on patches.

## Alternatives
* [clrzmq2][clrzmq] (still maintained)
* [ZeroMQ Interop][zeromq-interop] (actively developed)
* [NZMQ][nzmq] (last updated 2010)

## License
netzmq is released under the [Apache 2.0 license][apl]. See LICENSE and NOTICE for details.

[libzmq-src]: https://github.com/zeromq/zeromq3-0
[zeromq]: http://www.zeromq.org/
[clrzmq]: https://github.com/zeromq/clrzmq2
[clrzmq-nuget]: http://nuget.org/List/Packages/clrzmq2
[zeromq-interop]: http://zeromq.codeplex.com/
[nzmq]: http://nzmq.codeplex.com/
[apl]: http://www.apache.org/licenses/LICENSE-2.0.html
