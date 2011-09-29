#pragma once

#include <zmq.h>
#include "netzmq.h"

#ifndef TEMP_FAILURE_RETRY
#define TEMP_FAILURE_RETRY(_result_, expression) \
    do { _result_ = (int) (expression); } while (_result_ == -1 && zmq_errno() == EINTR);
#endif

using namespace System;

namespace ZeroMQ {
namespace Proxy {

    public ref class ErrorProvider : public IErrorProviderProxy
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