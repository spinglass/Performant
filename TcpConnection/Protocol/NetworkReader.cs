using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpConnection.Protocol
{
    class NetworkReader
    {
        public NetworkReader()
        {
            m_Buffer = new byte[255];
            m_Reader = new BinaryReader(new MemoryStream(m_Buffer));
            m_PendingType = MessageType.Invalid;
        }

        public NetworkStream Stream
        {
            get { return m_NetworkStream; }
            set { m_NetworkStream = value; }
        }

        public MessageType ReadHeader()
        {
            MessageType readType = MessageType.None;
            m_PendingType = MessageType.None;

            if (m_NetworkStream != null)
            {
                try
                {
                    byte type = (byte)m_NetworkStream.ReadByte();
                    if (Enum.IsDefined(typeof(MessageType), (MessageType)type))
                    {
                        int size = (int)m_NetworkStream.ReadByte();
                        if (size > 0)
                        {
                            int readSize = m_NetworkStream.Read(m_Buffer, 0, size);
                            if (size == readSize)
                            {
                                readType = (MessageType)type;
                                m_PendingType = readType;
                                m_Reader.BaseStream.Position = 0;
                            }
                            else
                            {
                                Debug.WriteLine("[NetworkReader.ReadHeader] Failed to read full message");
                            }
                        }
                        else
                        {
                            readType = (MessageType)type;
                        }
                    }
                    else
                    {
                        readType = MessageType.Invalid;
                        m_PendingType = MessageType.Invalid;
                        Debug.WriteLine("[NetworkReader.ReadHeader] Invalid message type");
                    }
                }
                catch (IOException e)
                {
                    Debug.WriteLine("[NetworkReader.ReadHeader] " + e.Message);
                }
            }

            return readType;
        }

        public bool ReadMessage(Message msg)
        {
            bool success = false;
            if (m_PendingType == msg.Type)
            {
                success = msg.Serialise(m_Reader);
                m_PendingType = MessageType.Invalid;
            }
            return success;
        }

        private NetworkStream m_NetworkStream;
        private byte[] m_Buffer;
        private BinaryReader m_Reader;
        private MessageType m_PendingType;
    }
}
