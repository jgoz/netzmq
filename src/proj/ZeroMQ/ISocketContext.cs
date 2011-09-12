namespace ZeroMQ
{
    using System;

    /// <summary>
    /// Represents a ZeroMQ context object. Implementors must be thread-safe.
    /// </summary>
    public interface ISocketContext : IDisposable
    {
        /// <summary>
        /// Gets the handle to the underlying ZeroMQ context object.
        /// </summary>
        IntPtr Handle { get; }

        /// <summary>
        /// Gets the size of the thread pool for this <see cref="ISocketContext"/> object.
        /// </summary>
        int ThreadPoolSize { get; }
    }
}
