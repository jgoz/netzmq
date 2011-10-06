namespace ZeroMQ.Sockets.Devices
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Defines a fluent interface for configuring device sockets.
    /// </summary>
    public class DeviceSocketSetup
    {
        private readonly ISocket socket;
        private readonly List<Action<ISocket>> socketOptions;
        private readonly List<string> bindings;
        private readonly List<string> connections;

        private bool isConfigured;

        internal DeviceSocketSetup(ISocket socket)
        {
            if (socket == null)
            {
                throw new ArgumentNullException("socket");
            }

            this.socket = socket;
            this.socketOptions = new List<Action<ISocket>>();
            this.bindings = new List<string>();
            this.connections = new List<string>();
        }

        /// <summary>
        /// Configure the socket to bind to a given endpoint. See <see cref="ZmqSocket.Bind"/> for details.
        /// </summary>
        /// <param name="endpoint">A string representing the endpoint to which the socket will bind.</param>
        /// <returns>The current <see cref="DeviceSocketSetup"/> object.</returns>
        public DeviceSocketSetup BindTo(string endpoint)
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
        /// <returns>The current <see cref="DeviceSocketSetup"/> object.</returns>
        public DeviceSocketSetup ConnectTo(string endpoint)
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
        /// <returns>The current <see cref="DeviceSocketSetup"/> object.</returns>
        public DeviceSocketSetup SetSocketOption(Expression<Func<ISocket, int>> property, int value)
        {
            return this.SetSocketOption<int>(property, value);
        }

        /// <summary>
        /// Set a long-based socket option.
        /// </summary>
        /// <param name="property">The <see cref="ISocket"/> property to set.</param>
        /// <param name="value">The long value to assign.</param>
        /// <returns>The current <see cref="DeviceSocketSetup"/> object.</returns>
        public DeviceSocketSetup SetSocketOption(Expression<Func<ISocket, long>> property, long value)
        {
            return this.SetSocketOption<long>(property, value);
        }

        /// <summary>
        /// Set a ulong-based socket option.
        /// </summary>
        /// <param name="property">The <see cref="ISocket"/> property to set.</param>
        /// <param name="value">The ulong value to assign.</param>
        /// <returns>The current <see cref="DeviceSocketSetup"/> object.</returns>
        public DeviceSocketSetup SetSocketOption(Expression<Func<ISocket, ulong>> property, ulong value)
        {
            return this.SetSocketOption<ulong>(property, value);
        }

        /// <summary>
        /// Set a byte array-based socket option.
        /// </summary>
        /// <param name="property">The <see cref="ISocket"/> property to set.</param>
        /// <param name="value">The byte array value to assign.</param>
        /// <returns>The current <see cref="DeviceSocketSetup"/> object.</returns>
        public DeviceSocketSetup SetSocketOption(Expression<Func<ISocket, byte[]>> property, byte[] value)
        {
            return this.SetSocketOption<byte[]>(property, value);
        }

        /// <summary>
        /// Set a <see cref="TimeSpan"/>-based socket option.
        /// </summary>
        /// <param name="property">The <see cref="ISocket"/> property to set.</param>
        /// <param name="value">The <see cref="TimeSpan"/> value to assign.</param>
        /// <returns>The current <see cref="DeviceSocketSetup"/> object.</returns>
        public DeviceSocketSetup SetSocketOption(Expression<Func<ISocket, TimeSpan>> property, TimeSpan value)
        {
            return this.SetSocketOption<TimeSpan>(property, value);
        }

        internal void Configure()
        {
            if (this.isConfigured)
            {
                return;
            }

            foreach (Action<ISocket> optionAction in this.socketOptions)
            {
                optionAction.Invoke(this.socket);
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

        private DeviceSocketSetup SetSocketOption<T>(Expression<Func<ISocket, T>> property, T value)
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

            this.socketOptions.Add(s => propertyInfo.SetValue(s, value, null));

            return this;
        }
    }
}
