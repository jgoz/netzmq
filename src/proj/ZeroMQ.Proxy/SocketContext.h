#pragma once

#include <zmq.h>
#include "netzmq.h"
#include "ZmqErrorProvider.h"

namespace ZeroMQ {
namespace Proxy {

    public ref class SocketContext : public IContextProxy
    {
        void *m_context;

    public:
        SocketContext(int threadPoolSize)
        {
            m_context = zmq_init(threadPoolSize);

            if (m_context == NULL) {
                throw ZmqErrorProvider::GetLastError();
            }
        }

        ~SocketContext()
        {
            this->!SocketContext();
        }

        virtual property IntPtr Handle
        {
            IntPtr get() { return (IntPtr)m_context; }
        }

        virtual void Terminate()
        {
            if (m_context == NULL)
                return;

            while (zmq_term(m_context) != 0) {
                int errorCode = zmq_errno();

                // If zmq_term fails, valid return codes are EFAULT or EINTR. If EINTR is set, termination
                // was interrupted by a signal and may be safely retried.
                if (errorCode == EFAULT) {

                    // This indicates an invalid context was passed in. There's nothing we can do about it here.
                    // It's arguably not a fatal error, so throwing an exception would be bad seeing as this may
                    // run inside a finalizer.
                    break;
                }
            }

            m_context = NULL;
        }

    protected:
        !SocketContext()
        {
            this->Terminate();
        }
    };

} }