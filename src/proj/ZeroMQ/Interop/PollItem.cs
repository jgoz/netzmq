namespace ZeroMQ.Interop
{
    using System;
    using System.Runtime.InteropServices;

    [Flags]
    internal enum PollEvent
    {
        PollIn = 0x1,
        PollOut = 0x2,
        PollErr = 0x4
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PollItem
    {
#pragma warning disable 414

        private readonly IntPtr socket;
#if POSIX
        private int fd;
#else
        private readonly IntPtr fd;
#endif
        private short events;
        private short revents;

#pragma warning restore

        /// <summary>
        /// Initializes a new instance of the PollItem struct.
        /// </summary>
        /// <param name="socket">Target ZMQ socket ptr</param>
        /// <param name="fd">Non ZMQ socket (Not Supported)</param>
        /// <param name="events">Desired events</param>
        internal PollItem(IntPtr socket, IntPtr fd, short events)
        {
            this.socket = socket;
            this.events = events;
            this.revents = 0;
#if POSIX
            this.fd = fd.ToInt32();
#else
            this.fd = fd;
#endif
        }

        /// <summary>
        /// Gets returned event flags
        /// </summary>
        public PollEvent Revents
        {
            get { return (PollEvent)this.revents; }
        }

        /// <summary>
        /// Reset revents so that poll item can be safely reused
        /// </summary>
        public void ResetRevents()
        {
            this.revents = 0;
        }

        internal void ActivateEvent(params PollEvent[] evts)
        {
            foreach (PollEvent evt in evts)
            {
                this.events = (short)(this.events | (short)evt);
            }
        }

        internal void DeactivateEvent(params PollEvent[] evts)
        {
            foreach (PollEvent evt in evts)
            {
                this.events &= (short)evt;
            } 
        }
    }
}
