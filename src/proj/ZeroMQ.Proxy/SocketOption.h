#pragma once

#include <zmq.h>

namespace ZeroMQ {
namespace Proxy {

    public enum class SocketOption
    {
        Affinity = ZMQ_AFFINITY,
        Identity = ZMQ_IDENTITY,
        Subscribe = ZMQ_SUBSCRIBE,
        Unsubscribe = ZMQ_UNSUBSCRIBE,
        Rate = ZMQ_RATE,
        RecoveryIvl = ZMQ_RECOVERY_IVL,
        SndBuf = ZMQ_SNDBUF,
        RcvBuf = ZMQ_RCVBUF,
        RcvMore = ZMQ_RCVMORE,
        Fd = ZMQ_FD,
        Events = ZMQ_EVENTS,
        Type = ZMQ_TYPE,
        Linger = ZMQ_LINGER,
        ReconnectIvl = ZMQ_RECONNECT_IVL,
        Backlog = ZMQ_BACKLOG,
        ReconnectIvlMax = ZMQ_RECONNECT_IVL_MAX,
        MaxMsgSize = ZMQ_MAXMSGSIZE,
        SndHwm = ZMQ_SNDHWM,
        RcvHwm = ZMQ_RCVHWM,
        MulticastHops = ZMQ_MULTICAST_HOPS,
        RcvTimeo = ZMQ_RCVTIMEO,
        SndTimeo = ZMQ_SNDTIMEO,
        RcvLabel = ZMQ_RCVLABEL,
    };

} }