// ZeroMQ.Proxy.h

#pragma once

#include "zmq.h"

using namespace System;
using namespace System::Runtime::InteropServices;

namespace ZeroMQ {
namespace Proxy {

	public ref class Socket
	{
		void *m_context;
		void *m_socket;
		int	m_socketType;

	public:
		Socket(void *context, int socketType)
			: m_context(context), m_socketType(socketType)
		{
			if (context == NULL) {
				throw gcnew ArgumentNullException("context");
			}

			m_socket = zmq_socket(m_context, m_socketType);

			if (m_socket == NULL) {
				// TODO: Use a custom exception type
				throw gcnew Exception("Unable to initialize socket.");
			}
		}

		~Socket()
		{
			this->!Socket();
		}

		void Bind(String^ endpoint)
		{
			IntPtr p = Marshal::StringToHGlobalAnsi(endpoint);
			char *endpointStr = static_cast<char*>(p.ToPointer());

			if (zmq_bind(m_socket, endpointStr) == -1) {
				throw gcnew Exception("Unable to bind socket.");
			}

			Marshal::FreeHGlobal(p);
		}

		void Connect(String^ endpoint)
		{
			IntPtr p = Marshal::StringToHGlobalAnsi(endpoint);
			char *endpointStr = static_cast<char*>(p.ToPointer());

			if (zmq_connect(m_socket, endpointStr) == -1) {
				throw gcnew Exception("Unable to connect socket.");
			}

			Marshal::FreeHGlobal(p);
		}

        int Receive(int socketFlags, [Out] array<Byte>^% buffer)
		{
			return 0;
		}

		int Send(int socketFlags, array<Byte>^ buffer)
		{
			return 0;
		}

	protected:
		!Socket()
		{
			if (m_socket == NULL) return;

			if (zmq_close(m_socket) == -1) {
				// TODO: Use a custom exception type
				throw gcnew Exception("Error closing socket.");
			}
		}
	};
}
}