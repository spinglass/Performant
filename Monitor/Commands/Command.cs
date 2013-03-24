using Monitor.Comms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Commands
{
    abstract class Command
    {
        abstract protected void ReadInternal(ResponseReader reader);

        public Command(CSAFE id, uint rspSize)
        {
            m_Id = id;
            m_RspSize = rspSize;
        }

        public void Write(CommandWriter writer)
        {
            writer.WriteByte((uint)m_Id);
        }

        public void Read(ResponseReader reader)
        {
            uint id = reader.ReadByte();
            uint size = reader.ReadByte();
            if (id == (uint)m_Id && size == m_RspSize)
            {
                ReadInternal(reader);
            }
            else
            {
                Debug.WriteLine("[Command.Read] id/size mismatch");
            }
        }

        private CSAFE m_Id;
        private uint m_RspSize;
    }
}
