﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpConnection.Protocol
{
    class Sender
    {
        public Sender()
        {
            m_NetworkStream = null;
            m_Reader = new NetworkReader();
            m_Writer = new NetworkWriter();

            // Messages
            m_Handshake = new HandshakeMessage();
        }

        public NetworkStream Stream
        {
            get { return m_NetworkStream; }
            set
            {
                m_NetworkStream = value;
                m_Reader.Stream = value;
                m_Writer.Stream = value;
            }
        }

        public bool Handshake()
        {
            bool success = false;

            // Send the handshake
            if (m_Writer.Send(m_Handshake))
            {
                // Check the response
                MessageType type = m_Reader.ReadHeader();
                if (type == MessageType.Handshake)
                {
                    success = m_Reader.ReadMessage(m_Handshake);
                }
                else
                {
                    if (type == MessageType.None)
                    {
                        Debug.WriteLine("[Sender.Handshake] No response");
                    }
                    else
                    {
                        Debug.WriteLine("[Sender.Handshake] Unexpected response (" + type.ToString() + ")");
                    }
                }
            }
            else
            {
                Debug.WriteLine("[Sender.Handshake] Send failed");
            }

            return success;
        }

        private NetworkStream m_NetworkStream;
        private NetworkReader m_Reader;
        private NetworkWriter m_Writer;

        private HandshakeMessage m_Handshake;
    }
}
