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
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        private IntPtr socket;

#if POSIX
        private int fd;
#else
        private IntPtr fd;
#endif

        // ReSharper restore FieldCanBeMadeReadOnly.Local
        private short events;
        private short revents;

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

        public PollEvent Revents
        {
            get { return (PollEvent)this.revents; }
        }

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
