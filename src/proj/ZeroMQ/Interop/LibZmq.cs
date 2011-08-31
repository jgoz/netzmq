namespace ZeroMQ.Interop
{
    using System;
    using System.Runtime.InteropServices;

    internal static class LibZmq
    {
        private static readonly UnmanagedLibrary ZmqLib;

        static LibZmq()
        {
            ZmqLib = new UnmanagedLibrary(Is64BitProcess() ? "libzmq64" : "libzmq32");

            ZmqInit = ZmqLib.GetUnmanagedProcedure<ZmqInitProc>("zmq_init");
            ZmqTerm = ZmqLib.GetUnmanagedProcedure<ZmqTermProc>("zmq_term");
            ZmqClose = ZmqLib.GetUnmanagedProcedure<ZmqCloseProc>("zmq_close");
            ZmqSetsockopt = ZmqLib.GetUnmanagedProcedure<ZmqSetSockOptProc>("zmq_setsockopt");
            ZmqGetsockopt = ZmqLib.GetUnmanagedProcedure<ZmqGetSockOptProc>("zmq_getsockopt");
            ZmqBind = ZmqLib.GetUnmanagedProcedure<ZmqBindProc>("zmq_bind");
            ZmqConnect = ZmqLib.GetUnmanagedProcedure<ZmqConnectProc>("zmq_connect");
            ZmqRecv = ZmqLib.GetUnmanagedProcedure<ZmqRecvProc>("zmq_recv");
            ZmqSend = ZmqLib.GetUnmanagedProcedure<ZmqSendProc>("zmq_send");
            ZmqSocket = ZmqLib.GetUnmanagedProcedure<ZmqSocketProc>("zmq_socket");
            ZmqMsgClose = ZmqLib.GetUnmanagedProcedure<ZmqMsgCloseProc>("zmq_msg_close");
            ZmqMsgData = ZmqLib.GetUnmanagedProcedure<ZmqMsgDataProc>("zmq_msg_data");
            ZmqMsgInit = ZmqLib.GetUnmanagedProcedure<ZmqMsgInitProc>("zmq_msg_init");
            ZmqMsgInitSize = ZmqLib.GetUnmanagedProcedure<ZmqMsgInitSizeProc>("zmq_msg_init_size");
            ZmqMsgSize = ZmqLib.GetUnmanagedProcedure<ZmqMsgSizeProc>("zmq_msg_size");
            ZmqErrno = ZmqLib.GetUnmanagedProcedure<ZmqErrnoProc>("zmq_errno");
            ZmqStrerror = ZmqLib.GetUnmanagedProcedure<ZmqStrErrorProc>("zmq_strerror");
            ZmqDevice = ZmqLib.GetUnmanagedProcedure<ZmqDeviceProc>("zmq_device");
            ZmqVersion = ZmqLib.GetUnmanagedProcedure<ZmqVersionProc>("zmq_version");
            ZmqPoll = ZmqLib.GetUnmanagedProcedure<ZmqPollProc>("zmq_poll");
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr ZmqInitProc(int ioThreads);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int ZmqTermProc(IntPtr context);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int ZmqCloseProc(IntPtr socket);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int ZmqSetSockOptProc(IntPtr socket, int option, IntPtr optval, int optvallen);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int ZmqGetSockOptProc(IntPtr socket, int option, IntPtr optval, IntPtr optvallen);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate int ZmqBindProc(IntPtr socket, string addr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate int ZmqConnectProc(IntPtr socket, string addr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int ZmqRecvProc(IntPtr socket, IntPtr msg, int flags);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int ZmqSendProc(IntPtr socket, IntPtr msg, int flags);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr ZmqSocketProc(IntPtr context, int type);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int ZmqMsgCloseProc(IntPtr msg);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr ZmqMsgDataProc(IntPtr msg);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int ZmqMsgInitProc(IntPtr msg);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int ZmqMsgInitSizeProc(IntPtr msg, int size);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int ZmqMsgSizeProc(IntPtr msg);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int ZmqErrnoProc();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate IntPtr ZmqStrErrorProc(int errnum);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int ZmqDeviceProc(int device, IntPtr inSocket, IntPtr outSocket);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ZmqVersionProc(IntPtr major, IntPtr minor, IntPtr patch);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int ZmqPollProc([In, Out] PollItem[] items, int nItems, long timeout);

        public static ZmqInitProc ZmqInit { get; set; }
        public static ZmqTermProc ZmqTerm { get; set; }
        public static ZmqCloseProc ZmqClose { get; set; }
        public static ZmqSetSockOptProc ZmqSetsockopt { get; set; }
        public static ZmqGetSockOptProc ZmqGetsockopt { get; set; }
        public static ZmqBindProc ZmqBind { get; set; }
        public static ZmqConnectProc ZmqConnect { get; set; }
        public static ZmqRecvProc ZmqRecv { get; set; }
        public static ZmqSendProc ZmqSend { get; set; }
        public static ZmqSocketProc ZmqSocket { get; set; }
        public static ZmqMsgCloseProc ZmqMsgClose { get; set; }
        public static ZmqMsgDataProc ZmqMsgData { get; set; }
        public static ZmqMsgInitProc ZmqMsgInit { get; set; }
        public static ZmqMsgInitSizeProc ZmqMsgInitSize { get; set; }
        public static ZmqMsgSizeProc ZmqMsgSize { get; set; }
        public static ZmqErrnoProc ZmqErrno { get; set; }
        public static ZmqStrErrorProc ZmqStrerror { get; set; }
        public static ZmqDeviceProc ZmqDevice { get; set; }
        public static ZmqVersionProc ZmqVersion { get; set; }
        public static ZmqPollProc ZmqPoll { get; set; }

        public static bool Is64BitProcess()
        {
            return IntPtr.Size == sizeof(long);
        }
    }
}
