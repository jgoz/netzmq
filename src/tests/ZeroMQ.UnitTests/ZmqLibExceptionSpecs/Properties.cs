namespace ZeroMQ.UnitTests.ZmqLibExceptionSpecs
{
    using Machine.Specifications;

    using ZeroMQ.Proxy;
    using ZeroMQ.Sockets;

    class when_error_has_known_posix_error_code
    {
        protected static ZmqLibException exception;

        Because of = () =>
            exception = new ZmqLibException((int)ErrorCode.Eprotonosupport, "Protocol is not supported.");

        It should_set_error_code = () =>
            exception.ErrorCode.ShouldEqual((int)ErrorCode.Eprotonosupport);

        It should_set_error_name = () =>
            exception.ErrorName.ShouldEqual("EPROTONOSUPPORT");

        It should_set_message = () =>
            exception.Message.ShouldEqual("Protocol is not supported.");
    }

    class when_error_has_known_zmq_error_code
    {
        protected static ZmqLibException exception;

        Because of = () =>
            exception = new ZmqLibException((int)ErrorCode.Enocompatproto, "Protocol is not compatible with socket type.");

        It should_set_error_code = () =>
            exception.ErrorCode.ShouldEqual((int)ErrorCode.Enocompatproto);

        It should_set_error_name = () =>
            exception.ErrorName.ShouldEqual("ENOCOMPATPROTO");

        It should_set_message = () =>
            exception.Message.ShouldEqual("Protocol is not compatible with socket type.");
    }

    class when_error_has_unknown_error_code
    {
        protected static ZmqLibException exception;

        Because of = () =>
            exception = new ZmqLibException(12345, "Some unknown error.");

        It should_set_error_code = () =>
            exception.ErrorCode.ShouldEqual(12345);

        It should_set_error_name_to_default_value = () =>
            exception.ErrorName.ShouldEqual("Error 12345");

        It should_set_message = () =>
            exception.Message.ShouldEqual("Some unknown error.");
    }
}
