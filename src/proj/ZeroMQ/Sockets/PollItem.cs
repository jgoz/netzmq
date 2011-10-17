namespace ZeroMQ.Sockets
{
    using System;

    using ZeroMQ.Proxy;

    internal class PollItem : IPollItem
    {
        private readonly ZmqSocket socket;

        public PollItem(ZmqSocket socket)
        {
            this.socket = socket;

            if (socket is IReceiveSocket)
            {
                this.Events |= PollFlags.PollIn;
            }

            if (socket is ISendSocket)
            {
                this.Events |= PollFlags.PollOut;
            }
        }

        public IntPtr Socket
        {
            get { return this.socket.Handle; }
        }

        public PollFlags Events { get; set; }
        public PollFlags REvents { get; set; }

        public void InvokeEvents()
        {
            if (this.REvents.HasFlag(PollFlags.PollIn))
            {
                this.socket.InvokeReceiveReady(new ReceiveReadyEventArgs(this.socket as IReceiveSocket));
            }

            if (this.REvents.HasFlag(PollFlags.PollOut))
            {
                this.socket.InvokeSendReady(new SendReadyEventArgs(this.socket as ISendSocket));
            }
        }
    }
}
