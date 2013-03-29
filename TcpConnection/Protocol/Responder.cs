using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpConnection.Protocol
{
    class Responder
    {
        public Responder()
        {
            m_NetworkStream = null;
            m_Reader = new NetworkReader();
            m_Writer = new NetworkWriter();

            // Messages
            m_Handshake = new HandshakeMessage();
            m_Command = new CommandMessage();
            m_Connected = new ConnectedMessage();
            m_Disconnected = new DisconnectedMessage();
            m_SendError = new SendErrorMessage();
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

            // Expecting a handshake
            MessageType type = m_Reader.ReadHeader();
            if (type == MessageType.Handshake)
            {
                if (m_Reader.ReadMessage(m_Handshake))
                {
                    // Send the response
                    success = m_Writer.Send(m_Handshake);
                }
            }
            else
            {
                if (type == MessageType.None)
                {
                    Debug.WriteLine("[Responder.Handshake] No message");
                }
                else
                {
                    Debug.WriteLine("[Responder.Handshake] Unexpected message (" + type.ToString() + ")");
                }
            }

            return success;
        }

        public MessageType ReadMessage()
        {
            return m_Reader.ReadHeader();
        }

        public bool ReadCommand(uint[] cmdData, ref int cmdDataCount)
        {
            bool success = false;

            if (m_Reader.ReadMessage(m_Command))
            {
                m_Command.Read(cmdData, ref cmdDataCount);
                success = true;
            }

            return success;
        }

        public bool SendResponse(uint[] rspData, int rspDataCount)
        {
            // Prepare the command message
            m_Command.Write(rspData, rspDataCount);

            // Send it
            return m_Writer.Send(m_Command);
        }

        public bool SendConnected()
        {
            return m_Writer.Send(m_Connected);
        }

        public bool SendDisconnected()
        {
            return m_Writer.Send(m_Disconnected);
        }

        public bool SendSendError()
        {
            return m_Writer.Send(m_SendError);
        }

        private NetworkStream m_NetworkStream;
        private NetworkReader m_Reader;
        private NetworkWriter m_Writer;

        private HandshakeMessage m_Handshake;
        private CommandMessage m_Command;
        private ConnectedMessage m_Connected;
        private DisconnectedMessage m_Disconnected;
        private SendErrorMessage m_SendError;
    }
}
