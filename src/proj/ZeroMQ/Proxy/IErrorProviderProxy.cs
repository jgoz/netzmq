namespace ZeroMQ.Proxy
{
    internal interface IErrorProviderProxy
    {
        int GetErrorCode();

        string GetErrorMessage(int errorCode);
    }
}
