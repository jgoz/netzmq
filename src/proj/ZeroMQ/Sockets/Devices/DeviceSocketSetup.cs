namespace ZeroMQ.Sockets.Devices
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Defines a fluent interface for configuring device sockets.
    /// </summary>
    /// <typeparam name="TSocket">The device socket type.</typeparam>
    public class DeviceSocketSetup<TSocket>
        where TSocket : class, ISocket
    {
        private readonly TSocket socket;
        private readonly List<Action<TSocket>> socketInitializers;
        private readonly List<string> bindings;
        private readonly List<string> connections;

        private bool isConfigured;

        internal DeviceSocketSetup(TSocket socket)
        {
            if (socket == null)
            {
                throw new ArgumentNullException("socket");
            }

            this.socket = socket;
            this.socketInitializers = new List<Action<TSocket>>();
            this.bindings = new List<string>();
            this.connections = new List<string>();
        }

        /// <summary>
        /// Configure the socket to bind to a given endpoint. See <see cref="ZmqSocket.Bind"/> for details.
        /// </summary>
        /// <param name="endpoint">A string representing the endpoint to which the socket will bind.</param>
        /// <returns>The current <see cref="DeviceSocketSetup{TSocket}"/> object.</returns>
        public DeviceSocketSetup<TSocket> BindTo(string endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }

            this.bindings.Add(endpoint);

            return this;
        }

        /// <summary>
        /// Configure the socket to connect to a given endpoint. See <see cref="ZmqSocket.Connect"/> for details.
        /// </summary>
        /// <param name="endpoint">A string representing the endpoint to which the socket will connect.</param>
        /// <returns>The current <see cref="DeviceSocketSetup{TSocket}"/> object.</returns>
        public DeviceSocketSetup<TSocket> ConnectTo(string endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }

            this.connections.Add(endpoint);

            return this;
        }

        /// <summary>
        /// Set an int-based socket option.
        /// </summary>
        /// <param name="property">The <see cref="ISocket"/> property to set.</param>
        /// <param name="value">The int value to assign.</param>
        /// <returns>The current <see cref="DeviceSocketSetup{TSocket}"/> object.</returns>
        public DeviceSocketSetup<TSocket> SetSocketOption(Expression<Func<ISocket, int>> property, int value)
        {
            return this.SetSocketOption<int>(property, value);
        }

        /// <summary>
        /// Set a long-based socket option.
        /// </summary>
        /// <param name="property">The <see cref="ISocket"/> property to set.</param>
        /// <param name="value">The long value to assign.</param>
        /// <returns>The current <see cref="DeviceSocketSetup{TSocket}"/> object.</returns>
        public DeviceSocketSetup<TSocket> SetSocketOption(Expression<Func<ISocket, long>> property, long value)
        {
            return this.SetSocketOption<long>(property, value);
        }

        /// <summary>
        /// Set a ulong-based socket option.
        /// </summary>
        /// <param name="property">The <see cref="ISocket"/> property to set.</param>
        /// <param name="value">The ulong value to assign.</param>
        /// <returns>The current <see cref="DeviceSocketSetup{TSocket}"/> object.</returns>
        public DeviceSocketSetup<TSocket> SetSocketOption(Expression<Func<ISocket, ulong>> property, ulong value)
        {
            return this.SetSocketOption<ulong>(property, value);
        }

        /// <summary>
        /// Set a byte array-based socket option.
        /// </summary>
        /// <param name="property">The <see cref="ISocket"/> property to set.</param>
        /// <param name="value">The byte array value to assign.</param>
        /// <returns>The current <see cref="DeviceSocketSetup{TSocket}"/> object.</returns>
        public DeviceSocketSetup<TSocket> SetSocketOption(Expression<Func<ISocket, byte[]>> property, byte[] value)
        {
            return this.SetSocketOption<byte[]>(property, value);
        }

        /// <summary>
        /// Set a <see cref="TimeSpan"/>-based socket option.
        /// </summary>
        /// <param name="property">The <see cref="ISocket"/> property to set.</param>
        /// <param name="value">The <see cref="TimeSpan"/> value to assign.</param>
        /// <returns>The current <see cref="DeviceSocketSetup{TSocket}"/> object.</returns>
        public DeviceSocketSetup<TSocket> SetSocketOption(Expression<Func<ISocket, TimeSpan>> property, TimeSpan value)
        {
            return this.SetSocketOption<TimeSpan>(property, value);
        }

        internal DeviceSocketSetup<TSocket> AddSocketInitializer(Action<TSocket> setupMethod)
        {
            this.socketInitializers.Add(setupMethod);

            return this;
        }

        internal void Configure()
        {
            if (this.isConfigured)
            {
                return;
            }

            foreach (Action<TSocket> initializer in this.socketInitializers)
            {
                initializer.Invoke(this.socket);
            }

            foreach (string endpoint in this.bindings)
            {
                this.socket.Bind(endpoint);
            }

            foreach (string endpoint in this.connections)
            {
                this.socket.Connect(endpoint);
            }

            this.isConfigured = true;
        }

        private DeviceSocketSetup<TSocket> SetSocketOption<T>(Expression<Func<ISocket, T>> property, T value)
        {
            PropertyInfo propertyInfo;

            if (property.Body is MemberExpression)
            {
                propertyInfo = ((MemberExpression)property.Body).Member as PropertyInfo;
            }
            else
            {
                propertyInfo = ((MemberExpression)((UnaryExpression)property.Body).Operand).Member as PropertyInfo;
            }

            if (propertyInfo == null)
            {
                throw new InvalidOperationException("The specified ISocket member is not a property: " + property.Body);
            }

            this.socketInitializers.Add(s => propertyInfo.SetValue(s, value, null));

            return this;
        }
    }
}
