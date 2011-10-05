#pragma once

#include <zmq.h>
#include "netzmq.h"
#include "ErrorProvider.h"

using namespace System;

namespace ZeroMQ {
namespace Proxy {

    public ref class Device : public IDeviceProxy
    {
        void* m_inSocket;
        void* m_outSocket;

        volatile bool m_running;

    public:
        Device(IntPtr inSocket, IntPtr outSocket)
            : m_inSocket((void*)inSocket), m_outSocket((void*)outSocket)
        {
        }

        virtual property bool IsRunning
        {
            bool get() { return m_running; }
            void set(bool value) { m_running = value; }
        }

        // Copied from zmq_device in zeromq release-2.1.11
        // Used under LGPL
        virtual int Run()
        {
            zmq_msg_t msg;
            int rc = zmq_msg_init(&msg);

            if (rc != 0) {
                return -1;
            }

            zmq_pollitem_t items[2];
            items[0].socket = m_inSocket;
            items[0].fd = 0;
            items[0].events = ZMQ_POLLIN;
            items[0].revents = 0;
            items[1].socket = m_outSocket;
            items[1].fd = 0;
            items[1].events = ZMQ_POLLIN;
            items[1].revents = 0;

            while (m_running) {
                // Wait while there are either requests or replies to process.
                TEMP_FAILURE_RETRY(rc, zmq_poll(&items[0], 2, -1));
                if (rc == -1) {
                    return -1;
                }

                // The algorithm below asumes ratio of request and replies processed
                // under full load to be 1:1. Although processing requests replies
                // first is tempting it is suspectible to DoS attacks (overloading
                // the system with unsolicited replies).

                // Process a request.
                if (items[0].revents & ZMQ_POLLIN) {
                    rc = Relay(m_inSocket, m_outSocket, msg);

                    if (rc == -1) {
                        return -1;
                    }
                }

                // Process a reply.
                if (items [1].revents & ZMQ_POLLIN) {
                    rc = Relay(m_outSocket, m_inSocket, msg);

                    if (rc == -1) {
                        return -1;
                    }
                }
            }

            return 0;
        }

    private:
        // Lifted from pyzmq, used under LGPL
        inline static int Relay(void *insocket, void *outsocket, zmq_msg_t &msg)
        {
            long long more = 0;
            long long label = 0;
            size_t flagsz = sizeof(more);
            int flags = 0;
            int rc;

            while (true) {

                TEMP_FAILURE_RETRY(rc, zmq_recvmsg(insocket, &msg, 0));
                if (rc == -1) {
                    return -1;
                }

                TEMP_FAILURE_RETRY(rc, zmq_getsockopt(insocket, ZMQ_RCVMORE, &more, &flagsz));
                if (rc == -1) {
                    return -1;
                }

                TEMP_FAILURE_RETRY(rc, zmq_getsockopt(insocket, ZMQ_RCVLABEL, &label, &flagsz));
                if (rc == -1) {
                    return -1;
                }

                flags = 0;
                if (more) {
                    flags |= ZMQ_SNDMORE;
                }
                if (label) {
                    flags |= ZMQ_SNDLABEL;
                }

                TEMP_FAILURE_RETRY(rc, zmq_sendmsg(outsocket, &msg, flags));
                if (rc == -1) {
                    return -1;
                }

                if (!more)
                    break;
            }

            return 0;
        }
    };

} }