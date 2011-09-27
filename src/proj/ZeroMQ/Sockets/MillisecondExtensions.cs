namespace ZeroMQ.Sockets
{
    using System;

    internal static class MillisecondExtensions
    {
        public static TimeSpan GetTimeSpan(this int milliseconds)
        {
            return milliseconds == -1 ? TimeSpan.MaxValue : TimeSpan.FromMilliseconds(milliseconds);
        }

        public static int GetMilliseconds(this TimeSpan timeSpan)
        {
            return timeSpan == TimeSpan.MaxValue ? -1 : (int)timeSpan.TotalMilliseconds;
        }
    }
}
