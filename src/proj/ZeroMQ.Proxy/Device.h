#pragma once

#include <zmq.h>
#include "netzmq.h"
#include "ErrorProvider.h"

#ifndef TEMP_FAILURE_RETRY_DEV
#define TEMP_FAILURE_RETRY_DEV(_result_, expression) \
    do { _result_ = (int) (expression); } while (m_running && _result_ == -1 && zmq_errno() == EINTR);
#endif

// If an error occurred, return -1 if the device is still supposed to be running. If it has been stopped, return 0.
#ifndef CHECK_RC
#define CHECK_RC(_result_) \
    if (_result_ == -1) { return ((int)m_running)*-1; }
#endif

using namespace System;

namespace ZeroMQ {
namespace Proxy {
    const int PollingIntervalMsec = 500;

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
                TEMP_FAILURE_RETRY_DEV(rc, zmq_poll(&items[0], 2, PollingIntervalMsec));
                CHECK_RC(rc);

                // The algorithm below asumes ratio of request and replies processed
                // under full load to be 1:1. Although processing requests replies
                // first is tempting it is suspectible to DoS attacks (overloading
                // the system with unsolicited replies).

                // Process a request.
                if (items[0].revents & ZMQ_POLLIN) {
                    rc = Relay(msg);
                    CHECK_RC(rc);
                }

                // Process a reply.
                if (items [1].revents & ZMQ_POLLIN) {
                    rc = Relay(msg);
                    CHECK_RC(rc);
                }
            }

            return 0;
        }

    private:
        // Lifted from pyzmq, used under LGPL
        inline int Relay(zmq_msg_t &msg)
        {
            long long more = 0;
            long long label = 0;
            size_t flagsz = sizeof(more);
            int flags = 0;
            int rc;

            while (true) {

                TEMP_FAILURE_RETRY_DEV(rc, zmq_recvmsg(m_inSocket, &msg, 0));
                CHECK_RC(rc);

                TEMP_FAILURE_RETRY_DEV(rc, zmq_getsockopt(m_inSocket, ZMQ_RCVMORE, &more, &flagsz));
                CHECK_RC(rc);

                TEMP_FAILURE_RETRY_DEV(rc, zmq_getsockopt(m_inSocket, ZMQ_RCVLABEL, &label, &flagsz));
                CHECK_RC(rc);

                flags = 0;
                if (more) {
                    flags |= ZMQ_SNDMORE;
                }
                if (label) {
                    flags |= ZMQ_SNDLABEL;
                }

                TEMP_FAILURE_RETRY_DEV(rc, zmq_sendmsg(m_outSocket, &msg, flags));
                CHECK_RC(rc);

                if (!more)
                    break;
            }

            return 0;
        }
    };

} }