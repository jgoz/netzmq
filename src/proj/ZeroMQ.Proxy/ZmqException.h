#pragma once

#include <zmq.h>

using namespace System;

namespace ZeroMQ {
namespace Proxy {

    [Serializable]
    public ref class ZmqException : public Exception
    {
        int m_errorCode;

    internal:
        ZmqException(int errorCode, char const* message)
            : m_errorCode(errorCode), Exception(gcnew String(message))
        {
        }

        ZmqException(int errorCode, char const* message, Exception^ inner)
            : m_errorCode(errorCode), Exception(gcnew String(message), inner)
        {
        }

    public:
        property int ErrorCode
        {
            int __clrcall get() { return m_errorCode; }
        }

        static ZmqException^ __clrcall GetLastError()
        {
            int errorCode = zmq_errno();

            return gcnew ZmqException(errorCode, zmq_strerror(errorCode));
        }

        static int __clrcall GetErrorCode()
        {
            return zmq_errno();
        }

        static String^ __clrcall GetErrorMessage(int errorCode)
        {
            return gcnew String(zmq_strerror(errorCode));
        }
    };

} }