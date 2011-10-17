#pragma once

#include <zmq.h>
#include "netzmq.h"

namespace ZeroMQ {
namespace Proxy {

    private ref class PollSet : public IPollSetProxy
    {
    private:
        int m_itemCount;
        zmq_pollitem_t *m_items;

    public:
        PollSet(int itemCount)
            : m_itemCount(itemCount)
        {
            m_items = new zmq_pollitem_t[itemCount];
        }

        ~PollSet()
        {
            delete m_items;

            this->!PollSet();
        }

        virtual int Poll(array<IPollItem^>^ items, int timeoutMilliseconds)
        {
            for (int i = 0; i < m_itemCount; i++) {
                m_items[i].socket = (void*)items[i]->Socket;
                m_items[i].fd = NULL;
                m_items[i].events = (short)items[i]->Events;
                m_items[i].revents = 0;
            }

            int rc = zmq_poll(m_items, m_itemCount, (long)timeoutMilliseconds);

            if (rc == -1)
                return -1;

            for (int i = 0; i < m_itemCount; i++) {
                items[i]->REvents = (PollFlags)m_items[i].revents;
            }

            return rc;
        }

    private:
        !PollSet()
        {
        }
    };

} }