#pragma once

#include <zmq.h>
#include "netzmq.h"

using namespace System;

namespace ZeroMQ {
namespace Proxy {

    public ref class ZmqErrorProvider : public IErrorProviderProxy
    {
    public:
        virtual int GetErrorCode()
        {
            return zmq_errno();
        }

        virtual String^ GetErrorMessage(int errorCode)
        {
            return gcnew String(zmq_strerror(errorCode));
        }

    internal:
        static ProxyException^ __clrcall GetLastError()
        {
            int errorCode = zmq_errno();

            return gcnew ProxyException(errorCode, gcnew String(zmq_strerror(errorCode)));
        }
    };

} }