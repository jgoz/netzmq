#pragma once

#include <zmq.h>
#include "netzmq.h"
#include "SocketContext.h"
#include "ZmqErrorProvider.h"

// Maximum number of bytes that can be retrieved by zmq_getsockopt
// Limited by the ZMQ_IDENTITY option
#define MAX_BIN_OPT_SIZE 255

using namespace System;
using namespace System::Runtime::InteropServices;

namespace ZeroMQ {
namespace Proxy {

    public ref class Socket : public ISocketProxy
    {
        void *m_socket;

    public:
        Socket(IntPtr context, int socketType)
        {
            m_socket = zmq_socket((void*)context, socketType);

            if (m_socket == NULL) {
                throw ZmqErrorProvider::GetLastError();
            }
        }

        ~Socket()
        {
            this->!Socket();
        }

        virtual property IntPtr Handle
        {
            IntPtr get() { return (IntPtr)m_socket; }
        }

        virtual int __clrcall Bind(String^ endpoint)
        {
            IntPtr p = Marshal::StringToHGlobalAnsi(endpoint);
            char *endpointStr = static_cast<char*>(p.ToPointer());

            int rc = zmq_bind(m_socket, endpointStr);

            Marshal::FreeHGlobal(p);

            return rc;
        }

        virtual int __clrcall Connect(String^ endpoint)
        {
            IntPtr p = Marshal::StringToHGlobalAnsi(endpoint);
            char *endpointStr = static_cast<char*>(p.ToPointer());

            int rc = zmq_connect(m_socket, endpointStr);

            Marshal::FreeHGlobal(p);

            return rc;
        }

        virtual int __clrcall SetSocketOption(SocketOption option, int value)
        {
            return zmq_setsockopt(m_socket, (int)option, &value, sizeof(int));
        }

        virtual int __clrcall SetSocketOption(SocketOption option, long long value)
        {
            return zmq_setsockopt(m_socket, (int)option, &value, sizeof(long long));
        }

        virtual int __clrcall SetSocketOption(SocketOption option, unsigned long long value)
        {
            return zmq_setsockopt(m_socket, (int)option, &value, sizeof(unsigned long long));
        }

        virtual int __clrcall SetSocketOption(SocketOption option, array<Byte>^ value)
        {
            if (value->Length == 0) {
                return zmq_setsockopt(m_socket, (int)option, NULL, 0);
            }

            pin_ptr<Byte> valuePtr = &value[0];

            return zmq_setsockopt(m_socket, (int)option, valuePtr, value->Length);
        }

        virtual int __clrcall GetSocketOption(SocketOption option, [Out] int% value)
        {
            int buf;
            size_t length = sizeof(buf);

            int rc = zmq_getsockopt(m_socket, (int)option, &buf, &length);

            value = buf;

            return rc;
        }

        virtual int __clrcall GetSocketOption(SocketOption option, [Out] long long% value)
        {
            long long buf;
            size_t length = sizeof(buf);

            int rc = zmq_getsockopt(m_socket, (int)option, &buf, &length);

            if (rc == -1)
                return -1;

            value = buf;

            return rc;
        }

        virtual int __clrcall GetSocketOption(SocketOption option, [Out] unsigned long long% value)
        {
            unsigned long long buf;
            size_t length = sizeof(buf);

            int rc = zmq_getsockopt(m_socket, (int)option, &buf, &length);

            if (rc == -1)
                return -1;

            value = buf;

            return rc;
        }

        virtual int __clrcall GetSocketOption(SocketOption option, [Out] array<Byte>^% value)
        {
            unsigned char buf[MAX_BIN_OPT_SIZE];
            size_t length = sizeof(buf);

            int rc = zmq_getsockopt(m_socket, (int)option, buf, &length);

            if (rc == -1)
                return -1;

            value = gcnew array<Byte>((int)length);

            Marshal::Copy((IntPtr)buf, value, 0, (int)length);

            return rc;
        }

        virtual int __clrcall Receive(int socketFlags, [Out] array<Byte>^% buffer)
        {
            zmq_msg_t msg;

            if (zmq_msg_init(&msg) == -1)
                return -1;

            int bytesReceived = zmq_recvmsg(m_socket, &msg, socketFlags);

            if (bytesReceived == -1)
                return -1;

            buffer = gcnew array<Byte>(bytesReceived);

            // memcpy is faster than Marshal.Copy for message sizes < 256,
            // but is less predictable at sizes above that
            if (bytesReceived < 256) {
                pin_ptr<Byte> bufferPtr = &buffer[buffer->GetLowerBound(0)];
                memcpy(bufferPtr, zmq_msg_data(&msg), bytesReceived);
            }
            else {
                Marshal::Copy((IntPtr)zmq_msg_data(&msg), buffer, 0, bytesReceived);
            }

            zmq_msg_close(&msg);

            return bytesReceived;
        }

        virtual int __clrcall Send(int socketFlags, array<Byte>^ buffer)
        {
            zmq_msg_t msg;

            int messageLength = buffer->Length;

            if (zmq_msg_init_size(&msg, messageLength) == -1)
                return -1;

            if (messageLength < 256) {
                pin_ptr<Byte> bufferPtr = &buffer[buffer->GetLowerBound(0)];
                memcpy(zmq_msg_data(&msg), bufferPtr, messageLength);
            }
            else {
                Marshal::Copy(buffer, 0, (IntPtr)zmq_msg_data(&msg), messageLength);
            }

            return zmq_sendmsg(m_socket, &msg, socketFlags);
        }

    protected:
        !Socket()
        {
            if (m_socket == NULL)
                return;

            zmq_close(m_socket);
        }
    };
}
}