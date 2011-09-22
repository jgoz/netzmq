namespace ZeroMQ
{
    using System;

    /// <summary>
    /// Represents a ZeroMQ context object. Implementors must be thread-safe.
    /// </summary>
    public interface IZmqContext : IDisposable
    {
        /// <summary>
        /// Gets the size of the thread pool for this <see cref="IZmqContext"/> object.
        /// </summary>
        int ThreadPoolSize { get; }
    }
}
