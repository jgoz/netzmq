namespace ZeroMQ.Sockets
{
    /// <summary>
    /// Provides extension methods for conversions between message formats.
    /// </summary>
    public static class MessageBufferExtensions
    {
        /// <summary>
        /// Converts a <see cref="string"/> to a <see cref="byte"/> array suitable for transmission via sockets.
        /// Uses <see cref="ZmqContext.DefaultEncoding"/> to perform the conversion.
        /// </summary>
        /// <param name="message">The string to convert.</param>
        /// <returns>A <see cref="byte"/> array containing the encoded string.</returns>
        public static byte[] ToZmqBuffer(this string message)
        {
            return ZmqContext.DefaultEncoding.GetBytes(message);
        }

        /// <summary>
        /// Converts a <see cref="byte"/> array to a <see cref="string"/> using <see cref="ZmqContext.DefaultEncoding"/>.
        /// </summary>
        /// <param name="buffer">A <see cref="byte"/> array value containing the message to convert.</param>
        /// <returns>A <see cref="string"/> containing the decoded string.</returns>
        public static string ToZmqMessage(this byte[] buffer)
        {
            return ZmqContext.DefaultEncoding.GetString(buffer);
        }
    }
}
