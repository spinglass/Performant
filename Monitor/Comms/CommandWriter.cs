using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Comms
{
    class CommandWriter
    {
        public CommandWriter(int capacity)
        {
            m_Buffer = new uint[capacity];
            m_Size = 0;
        }

        public int Size { get { return m_Size; } }
        public uint[] Buffer { get { return m_Buffer; } }

        public void Reset()
        {
            m_Size = 0;
        }

        public void WriteByte(uint val)
        {
            if (m_Size >= m_Buffer.Length)
            {
                throw new EndOfStreamException("Write past end of command buffer");
            }
            m_Buffer[m_Size++] = val;
        }

        private uint[] m_Buffer;
        private int m_Size;
    }
}
