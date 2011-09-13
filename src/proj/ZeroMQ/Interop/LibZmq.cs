namespace ZeroMQ.Interop
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Interop methods targeting libzmq v3.0
    /// </summary>
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
        private delegate IntPtr ZmqInitProc(int ioThreads);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ZmqTermProc(IntPtr context);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ZmqCloseProc(IntPtr socket);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ZmqSetSockOptProc(IntPtr socket, int option, IntPtr optval, int optvallen);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ZmqGetSockOptProc(IntPtr socket, int option, IntPtr optval, IntPtr optvallen);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate int ZmqBindProc(IntPtr socket, string addr);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate int ZmqConnectProc(IntPtr socket, string addr);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ZmqRecvProc(IntPtr socket, IntPtr buf, UIntPtr len, int flags);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ZmqSendProc(IntPtr socket, IntPtr buf, UIntPtr len, int flags);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr ZmqSocketProc(IntPtr context, int type);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ZmqMsgCloseProc(IntPtr msg);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr ZmqMsgDataProc(IntPtr msg);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ZmqMsgInitProc(IntPtr msg);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ZmqMsgInitSizeProc(IntPtr msg, UIntPtr size);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ZmqMsgSizeProc(IntPtr msg);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ZmqErrnoProc();
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate IntPtr ZmqStrErrorProc(int errnum);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ZmqDeviceProc(int device, IntPtr inSocket, IntPtr outSocket);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ZmqVersionProc(IntPtr major, IntPtr minor, IntPtr patch);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ZmqPollProc([In, Out] PollItem[] items, int nItems, long timeout);

        private static ZmqInitProc ZmqInit { get; set; }
        private static ZmqTermProc ZmqTerm { get; set; }
        private static ZmqCloseProc ZmqClose { get; set; }
        private static ZmqSetSockOptProc ZmqSetsockopt { get; set; }
        private static ZmqGetSockOptProc ZmqGetsockopt { get; set; }
        private static ZmqBindProc ZmqBind { get; set; }
        private static ZmqConnectProc ZmqConnect { get; set; }
        private static ZmqRecvProc ZmqRecv { get; set; }
        private static ZmqSendProc ZmqSend { get; set; }
        private static ZmqSocketProc ZmqSocket { get; set; }
        private static ZmqMsgCloseProc ZmqMsgClose { get; set; }
        private static ZmqMsgDataProc ZmqMsgData { get; set; }
        private static ZmqMsgInitProc ZmqMsgInit { get; set; }
        private static ZmqMsgInitSizeProc ZmqMsgInitSize { get; set; }
        private static ZmqMsgSizeProc ZmqMsgSize { get; set; }
        private static ZmqErrnoProc ZmqErrno { get; set; }
        private static ZmqStrErrorProc ZmqStrerror { get; set; }
        private static ZmqDeviceProc ZmqDevice { get; set; }
        private static ZmqVersionProc ZmqVersion { get; set; }
        private static ZmqPollProc ZmqPoll { get; set; }

        public static bool Is64BitProcess()
        {
            return IntPtr.Size == sizeof(long);
        }

        public static IntPtr Init(int threadPoolSize)
        {
            return ZmqInit(threadPoolSize);
        }

        public static int Term(IntPtr context)
        {
            return ZmqTerm(context);
        }

        public static int Close(IntPtr socket)
        {
            return ZmqClose(socket);
        }

        public static int SetSockOpt(IntPtr socket, int option, IntPtr optval, int optvallen)
        {
            return ZmqSetsockopt(socket, option, optval, optvallen);
        }

        public static int GetSockOpt(IntPtr socket, int option, IntPtr optval, IntPtr optvallen)
        {
            return ZmqGetsockopt(socket, option, optval, optvallen);
        }

        public static int Bind(IntPtr socket, string addr)
        {
            return ZmqBind(socket, addr);
        }

        public static int Connect(IntPtr socket, string addr)
        {
            return ZmqConnect(socket, addr);
        }

        public static int Recv(IntPtr socket, IntPtr buf, UIntPtr len, int flags)
        {
            return ZmqRecv(socket, buf, len, flags);
        }

        public static int Send(IntPtr socket, IntPtr buf, UIntPtr len, int flags)
        {
            return ZmqSend(socket, buf, len, flags);
        }

        public static IntPtr Socket(IntPtr context, int type)
        {
            return ZmqSocket(context, type);
        }

        public static int MsgClose(IntPtr msg)
        {
            return ZmqMsgClose(msg);
        }

        public static IntPtr MsgData(IntPtr msg)
        {
            return ZmqMsgData(msg);
        }

        public static int MsgInit(IntPtr msg)
        {
            return ZmqMsgInit(msg);
        }

        public static int MsgInitSize(IntPtr msg, UIntPtr size)
        {
            return ZmqMsgInitSize(msg, size);
        }

        public static int MsgSize(IntPtr msg)
        {
            return ZmqMsgSize(msg);
        }

        public static int Errno()
        {
            return ZmqErrno();
        }

        public static string StrError(int errnum)
        {
            return Marshal.PtrToStringAnsi(ZmqStrerror(errnum));
        }

        public static int Device(int device, IntPtr inSocket, IntPtr outSocket)
        {
            return ZmqDevice(device, inSocket, outSocket);
        }

        public static void Version(IntPtr major, IntPtr minor, IntPtr patch)
        {
            ZmqVersion(major, minor, patch);
        }

        public static int Poll(PollItem[] items, long timeout)
        {
            return ZmqPoll(items, items.Length, timeout);
        }
    }
}
