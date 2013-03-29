using Common;
using System;
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
            m_Connected = new ConnectedMessage();
            m_Command = new CommandMessage();

            m_State = ConnectionState.Disconnected;
        }

        public ConnectionState State
        {
            get { return m_State; }
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
            }
            else
            {
                Debug.WriteLine("[Sender.Handshake] Send failed");
            }

            return success;
        }

        public ConnectionState TestConnection()
        {
            m_State = ConnectionState.Disconnected;

            if (m_Writer.Send(m_Connected))
            {
                MessageType type = m_Reader.ReadHeader();
                if (type == MessageType.Connected)
                {
                    m_State = ConnectionState.Connected;
                }
            }

            return m_State;
        }

        public bool SendCommand(uint[] cmdData, int cmdDataCount, uint[] rspData, ref int rspDataCount)
        {
            bool success = false;

            // Prepare the command message
            m_Command.Write(cmdData, cmdDataCount);

            // Send it
            if (m_Writer.Send(m_Command))
            {
                // Check the response
                MessageType type = m_Reader.ReadHeader();
                switch (type)
                {
                    case MessageType.Command:
                        if (m_Reader.ReadMessage(m_Command))
                        {
                            m_Command.Read(rspData, ref rspDataCount);
                            success = true;
                            m_State = ConnectionState.Connected;
                        }
                        else
                        {
                            Debug.WriteLine("[Sender.SendMessage] Failed to read response message");
                        }
                        break;

                    case MessageType.SendError:
                        m_State = ConnectionState.SendError;
                        break;

                    default:
                    case MessageType.Disconnected:
                        m_State = ConnectionState.Disconnected;
                        break;
                }
            }
            else
            {
                m_State = ConnectionState.Disconnected;
                Debug.WriteLine("[Sender.SendMessage] Send failed");
            }
            return success;
        }

        private NetworkStream m_NetworkStream;
        private NetworkReader m_Reader;
        private NetworkWriter m_Writer;

        private HandshakeMessage m_Handshake;
        private ConnectedMessage m_Connected;
        private CommandMessage m_Command;

        private ConnectionState m_State;
    }
}
