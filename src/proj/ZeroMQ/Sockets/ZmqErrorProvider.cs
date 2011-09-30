namespace ZeroMQ.Sockets
{
    using ZeroMQ.Proxy;

    internal class ZmqErrorProvider
    {
        private readonly IErrorProviderProxy proxy;

        public ZmqErrorProvider(IErrorProviderProxy proxy)
        {
            this.proxy = proxy;
        }

        public bool ShouldTryAgain
        {
            get { return this.GetErrorCode() == ErrorCode.Eagain; }
        }

        public bool ContextWasTerminated
        {
            get { return this.GetErrorCode() == ErrorCode.Eterm; }
        }

        public bool ThreadWasInterrupted
        {
            get { return this.GetErrorCode() == ErrorCode.Eintr; }
        }

        public ErrorCode GetErrorCode()
        {
            return (ErrorCode)this.proxy.GetErrorCode();
        }

        public ZmqLibException GetLastError()
        {
            var errorCode = (int)this.GetErrorCode();

            return new ZmqLibException(errorCode, this.proxy.GetErrorMessage(errorCode));
        }

        public ZmqSocketException GetLastSocketError()
        {
            ZmqLibException lastError = this.GetLastError();

            return new ZmqSocketException(lastError.ErrorCode, lastError.Message);
        }
    }
}
