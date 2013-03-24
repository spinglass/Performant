using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpConnection.Protocol
{
    class CommandMessage : Message
    {
        public CommandMessage() :
            base(MessageType.Command)
        {
            m_Data = new byte[64];
            m_DataCount = 0;
        }

        public void Write(uint[] cmdData, int cmdDataCount)
        {
            for (int i = 0; i < cmdDataCount; ++i)
            {
                m_Data[i] = (byte)m_Data[i];
            }
            m_DataCount = cmdDataCount;
        }

        public void Read(uint[] cmdData, ref int cmdDataCount)
        {
            for (int i = 0; i < m_DataCount; ++i)
            {
                m_Data[i] = m_Data[i];
            }
            cmdDataCount = m_DataCount;
        }

        public override void Serialise(BinaryWriter writer)
        {
            writer.Write((byte)m_DataCount);
            writer.Write(m_Data, 0, m_DataCount);
        }

        public override bool Serialise(BinaryReader reader)
        {
            m_DataCount = reader.ReadByte();
            int size = reader.Read(m_Data, 0, m_DataCount);
            return (size == m_DataCount);
        }

        private byte[] m_Data;
        private int m_DataCount;
    }
}
