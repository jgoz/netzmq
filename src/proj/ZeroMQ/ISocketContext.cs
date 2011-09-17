namespace ZeroMQ
{
    using System;

    /// <summary>
    /// Represents a ZeroMQ context object. Implementors must be thread-safe.
    /// </summary>
    public interface ISocketContext : IDisposable
    {
        /// <summary>
        /// Gets the underlying socket context handle.
        /// </summary>
        Proxy.SocketContext Context { get; }

        /// <summary>
        /// Gets the size of the thread pool for this <see cref="ISocketContext"/> object.
        /// </summary>
        int ThreadPoolSize { get; }
    }
}
