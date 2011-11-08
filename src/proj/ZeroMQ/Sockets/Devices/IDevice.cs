namespace ZeroMQ.Sockets.Devices
{
    using System;

    /// <summary>
    /// Represents a ZeroMQ device, connecting a frontend socket to a backend socket.
    /// </summary>
    /// <typeparam name="TFrontend">The frontend socket type.</typeparam>
    /// <typeparam name="TBackend">The backend socket type.</typeparam>
    public interface IDevice<TFrontend, TBackend> : IDisposable
        where TFrontend : class, ISocket
        where TBackend : class, ISocket
    {
        /// <include file='DeviceDoc.xml' path='Devices/Members[@name="IsRunning"]/*'/>
        bool IsRunning { get; }

        /// <include file='DeviceDoc.xml' path='Devices/Members[@name="ConfigureFrontend"]/*'/>
        DeviceSocketSetup<TFrontend> ConfigureFrontend();

        /// <include file='DeviceDoc.xml' path='Devices/Members[@name="ConfigureBackend"]/*'/>
        DeviceSocketSetup<TBackend> ConfigureBackend();

        /// <summary>
        /// Start the device.
        /// </summary>
        void Start();

        /// <include file='DeviceDoc.xml' path='Devices/Members[@name="Join1"]/*'/>
        void Join();

        /// <include file='DeviceDoc.xml' path='Devices/Members[@name="Join2"]/*'/>
        bool Join(TimeSpan timeout);

        /// <include file='DeviceDoc.xml' path='Devices/Members[@name="Stop"]/*'/>
        void Stop();

        /// <include file='DeviceDoc.xml' path='Devices/Members[@name="Close"]/*'/>
        void Close();
    }
}