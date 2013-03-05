using Monitor.Comms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Commands
{
    class PaceCommand : Command
    {
        public PaceCommand()
            : base(CSAFE.GETPACE_CMD, 3)
        {
            m_StrokePace = new Time();
        }

        override protected void ReadInternal(ResponseReader reader)
        {
            uint pace = reader.ReadUShort();
            reader.ReadByte(); // Expecting 0x39 - Seconds per kilometer

            m_StrokePace.TotalSeconds = pace / 2;
        }

        public Time StrokePace { get { return m_StrokePace; } }

        private Time m_StrokePace;
    }
}
