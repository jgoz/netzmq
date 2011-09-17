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
            int get() { return m_errorCode; }
        }

        static ZmqException^ GetLastError()
        {
            int errorCode = zmq_errno();

            return gcnew ZmqException(errorCode, zmq_strerror(errorCode));
        }

        static int GetErrorCode()
        {
            return zmq_errno();
        }

        static String^ GetErrorMessage(int errorCode)
        {
            return gcnew String(zmq_strerror(errorCode));
        }
    };

} }