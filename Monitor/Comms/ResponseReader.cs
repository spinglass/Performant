using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Comms
{
    class ResponseReader
    {
        public ResponseReader(int capacity)
        {
            m_Buffer = new uint[capacity];
            m_Size = 0;
            m_Position = 0;
        }

        public int Capacity { get { return m_Buffer.Length; } }
        public int Size { get { return m_Size; } }
        public int Position { get { return m_Position; } }
        public uint[] Buffer { get { return m_Buffer; } }

        public void Reset(int size)
        {
            m_Size = Math.Min(size, m_Buffer.Length);
            m_Position = 0;
        }

        public uint ReadByte()
        {
            if (m_Position >= m_Size)
            {
                throw new BufferExceededException("Attempted to read past end of response buffer.");
            }
            return m_Buffer[m_Position++];
        }

        public uint ReadUShort()
        {
            uint val = (ReadByte() << 0) + (ReadByte() << 8);
            return val;
        }

        public uint ReadUInt()
        {
            uint val = (ReadByte() << 0) + (ReadByte() << 8) + (ReadByte() << 16) + (ReadByte() << 24);
            return val;
        }

        private uint[] m_Buffer;
        private int m_Size;
        private int m_Position;
    }
}
