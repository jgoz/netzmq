#pragma once

#include <zmq.h>
#include "SocketContext.h"

// Maximum number of bytes that can be retrieved by zmq_getsockopt
// Limited by the ZMQ_IDENTITY option
#define MAX_BIN_OPT_SIZE 255

using namespace System;
using namespace System::Runtime::InteropServices;

namespace ZeroMQ {
namespace Proxy {

    public ref class Socket
    {
        void *m_socket;

    public:
        Socket(SocketContext^ context, int socketType)
        {
            m_socket = zmq_socket(context->Context, socketType);

            if (m_socket == NULL) {
                throw ZmqException::GetLastError();
            }
        }

        ~Socket()
        {
            this->!Socket();
        }

        int Bind(String^ endpoint)
        {
            IntPtr p = Marshal::StringToHGlobalAnsi(endpoint);
            char *endpointStr = static_cast<char*>(p.ToPointer());

            int rc = zmq_bind(m_socket, endpointStr);

            Marshal::FreeHGlobal(p);

            return rc;
        }

        int Connect(String^ endpoint)
        {
            IntPtr p = Marshal::StringToHGlobalAnsi(endpoint);
            char *endpointStr = static_cast<char*>(p.ToPointer());

            int rc = zmq_connect(m_socket, endpointStr);

            Marshal::FreeHGlobal(p);

            return rc;
        }

        int SetSocketOption(int option, int value)
        {
            return zmq_setsockopt(m_socket, option, &value, sizeof(int));
        }

        int SetSocketOption(int option, unsigned long long value)
        {
            return zmq_setsockopt(m_socket, option, &value, sizeof(unsigned long long));
        }

        int SetSocketOption(int option, array<Byte>^ value)
        {
            if (value->Length == 0) {
                return zmq_setsockopt(m_socket, option, NULL, 0);
            }

            pin_ptr<Byte> valuePtr = &value[0];

            return zmq_setsockopt(m_socket, option, valuePtr, value->Length);
        }

        int GetSocketOption(int option, [Out] int% value)
        {
            int buf;
            size_t length;

            int rc = zmq_getsockopt(m_socket, option, &buf, &length);

            value = buf;

            return rc;
        }

        int GetSocketOption(int option, [Out] unsigned long long% value)
        {
            unsigned long long buf;
            size_t length;

            int rc = zmq_getsockopt(m_socket, option, &buf, &length);

            if (rc == -1)
                return -1;

            value = buf;

            return rc;
        }

        int GetSocketOption(int option, [Out] array<Byte>^% value)
        {
            unsigned char buf[MAX_BIN_OPT_SIZE];
            size_t length;

            int rc = zmq_getsockopt(m_socket, option, buf, &length);

            if (rc == -1)
                return -1;

            value = gcnew array<Byte>((int)length);

            Marshal::Copy((IntPtr)buf, value, 0, (int)length);

            return rc;
        }

        int Receive(int socketFlags, [Out] array<Byte>^% buffer)
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

        int Send(int socketFlags, array<Byte>^ buffer)
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