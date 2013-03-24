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

        public void Write(uint[] data, int dataCount)
        {
            for (int i = 0; i < dataCount; ++i)
            {
                m_Data[i] = (byte)data[i];
            }
            m_DataCount = dataCount;
        }

        public void Read(uint[] data, ref int dataCount)
        {
            for (int i = 0; i < m_DataCount; ++i)
            {
                data[i] = m_Data[i];
            }
            dataCount = m_DataCount;
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
