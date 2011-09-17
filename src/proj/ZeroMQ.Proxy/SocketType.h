#pragma once

#include <zmq.h>

namespace ZeroMQ {
namespace Proxy {

    public enum class SocketType
    {
        Pair = ZMQ_PAIR,
        Pub = ZMQ_PUB,
        Sub = ZMQ_SUB,
        Req = ZMQ_REQ,
        Rep = ZMQ_REP,
        Xreq = ZMQ_XREQ,
        Xrep = ZMQ_XREP,
        Pull = ZMQ_PULL,
        Push = ZMQ_PUSH,
        Xpub = ZMQ_XPUB,
        Xsub = ZMQ_XSUB,
        Router = ZMQ_ROUTER,
        Dealer = ZMQ_DEALER,
    };

} }

