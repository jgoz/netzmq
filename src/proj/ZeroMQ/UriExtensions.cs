namespace ZeroMQ
{
    using System;

    internal static class UriExtensions
    {
        public static string ToZeroMQEndpoint(this Uri uri)
        {
            return uri.Scheme + Uri.SchemeDelimiter + uri.Authority;
        }
    }
}
