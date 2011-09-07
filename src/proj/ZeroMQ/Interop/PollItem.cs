namespace ZeroMQ.Interop
{
    using System;
    using System.Runtime.InteropServices;

    [Flags]
    internal enum PollEvent
    {
        // ZMQ_POLLIN
        In = 0x1,

        // ZMQ_POLLOUT
        Out = 0x2,

        // ZMQ_POLLERR
        Error = 0x4
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PollItem
    {
        private readonly IntPtr socket;
#if POSIX
        private readonly int fd;
#else
        private readonly IntPtr fd;
#endif
        private short events;
        private short revents;

        public PollItem(IntPtr socket, IntPtr fd, short events)
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

        public void ActivateEvent(params PollEvent[] evts)
        {
            foreach (PollEvent evt in evts)
            {
                this.events = (short)(this.events | (short)evt);
            }
        }

        public void DeactivateEvent(params PollEvent[] evts)
        {
            foreach (PollEvent evt in evts)
            {
                this.events &= (short)evt;
            }
        }

        public void ResetRevents()
        {
            this.revents = 0;
        }
    }
}
