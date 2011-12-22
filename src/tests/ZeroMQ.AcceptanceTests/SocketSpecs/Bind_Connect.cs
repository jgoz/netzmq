namespace ZeroMQ.AcceptanceTests.SocketSpecs
{
    using System;
    using System.Threading;

    using Machine.Specifications;

    using ZeroMQ.Sockets;

    [Subject("Bind and connect")]
    class when_binding_and_connecting_to_a_tcp_ip_address_and_port : using_req_and_rep_sockets
    {
        Because of = () =>
            exception = Catch.Exception(() =>
            {
                rep.Bind("tcp://127.0.0.1:9000");
                req.Connect("tcp://127.0.0.1:9000");
            });

        It should_not_fail = () =>
            exception.ShouldBeNull();
    }

    [Subject("Bind and connect")]
    class when_binding_to_a_tcp_port_and_connecting_to_address_and_port : using_req_and_rep_sockets
    {
        Because of = () =>
            exception = Catch.Exception(() =>
            {
                rep.Bind("tcp://*:9000");
                req.Connect("tcp://127.0.0.1:9000");
            });

        It should_not_fail = () =>
            exception.ShouldBeNull();
    }

    [Subject("Bind and connect")]
    class when_binding_and_connecting_to_a_named_inproc_address : using_req_and_rep_sockets
    {
        Because of = () =>
            exception = Catch.Exception(() =>
            {
                rep.Bind("inproc://named");
                req.Connect("inproc://named");
            });

        It should_not_fail = () =>
            exception.ShouldBeNull();
    }

    [Subject("Bind and connect"), Ignore("PGM not currently working")]
    class when_binding_and_connecting_to_a_pgm_socket_with_pub_and_sub : using_pub_and_sub_sockets
    {
        Because of = () =>
            exception = Catch.Exception(() =>
            {
                pub.Linger = TimeSpan.Zero;
                pub.Connect("epgm://127.0.0.1;239.192.1.1:5000");

                sub.Connect("epgm://127.0.0.1;239.192.1.1:5000");

                // TODO: Is there any other way to ensure the PGM thread has started?
                Thread.Sleep(100);
            });

        It should_not_fail = () =>
            exception.ShouldBeNull();
    }

    [Subject("Bind")]
    class when_binding_to_a_pgm_socket_with_rep : using_req_and_rep_sockets
    {
        Because of = () =>
            exception = Catch.Exception(() => rep.Bind("epgm://127.0.0.1;239.192.1.1:5000"));

        It should_fail_because_pgm_is_not_supported_by_rep = () =>
            exception.ShouldBeOfType<ZmqSocketException>();

        It should_have_an_error_code_of_enocompatproto = () =>
            ((ZmqSocketException)exception).ErrorCode.ShouldEqual(156384764);

        It should_have_an_error_name_of_enocompatproto = () =>
            ((ZmqSocketException)exception).ErrorName.ShouldEqual("ENOCOMPATPROTO");

        It should_have_a_specific_error_message = () =>
            exception.Message.ShouldContain("protocol is not compatible with the socket type");
    }

    [Subject("Bind")]
    class when_binding_to_an_ipc_address : using_req_and_rep_sockets
    {
        Because of = () =>
            exception = Catch.Exception(() => rep.Bind("ipc:///tmp/feeds/0"));

        It should_fail_because_ipc_is_not_supported_on_windows = () =>
            exception.ShouldBeOfType<ZmqSocketException>();

        It should_have_an_error_code_of_eprotonosupport = () =>
            ((ZmqSocketException)exception).ErrorCode.ShouldEqual(135);

        It should_have_an_error_name_of_eprotonosupport = () =>
            ((ZmqSocketException)exception).ErrorName.ShouldEqual("EPROTONOSUPPORT");

        It should_have_a_specific_error_message = () =>
            exception.Message.ShouldContain("Protocol not supported");
    }

    [Subject("Connect")]
    class when_connecting_to_an_ipc_address : using_req_and_rep_sockets
    {
        Because of = () =>
            exception = Catch.Exception(() => rep.Connect("ipc:///tmp/feeds/0"));

        It should_fail_because_ipc_is_not_supported_on_windows = () =>
            exception.ShouldBeOfType<ZmqSocketException>();

        It should_have_an_error_code_of_eprotonosupport = () =>
            ((ZmqSocketException)exception).ErrorCode.ShouldEqual(135);

        It should_have_an_error_name_of_eprotonosupport = () =>
            ((ZmqSocketException)exception).ErrorName.ShouldEqual("EPROTONOSUPPORT");

        It should_have_a_specific_error_message = () =>
            exception.Message.ShouldContain("Protocol not supported");
    }
}
