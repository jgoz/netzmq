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

## *Another* .NET binding for ZMQ&#x203d;
This project is first and foremost a means to learn ZeroMQ more deeply. While there are other projects that provide .NET bindings for ZeroMQ, the secondary goals of this project are sufficiently different to justify creating a new library, at least for experimentation. If any of the ideas in this project appeal to the maintainers of other bindings, I'd be happy to collaborate on patches.

## Alternatives
* [clrzmq2][clrzmq] (still maintained)
* [ZeroMQ Interop][zeromq-interop] (actively developed)
* [NZMQ][nzmq] (last updated 2010)

## License
netzmq is released under the [Apache 2.0 license][apl]. See LICENSE and NOTICE for details.

[zeromq]: http://www.zeromq.org/
[clrzmq]: https://github.com/zeromq/clrzmq2
[clrzmq-nuget]: http://nuget.org/List/Packages/clrzmq2
[zeromq-interop]: http://zeromq.codeplex.com/
[nzmq]: http://nzmq.codeplex.com/
[apl]: http://www.apache.org/licenses/LICENSE-2.0.html
