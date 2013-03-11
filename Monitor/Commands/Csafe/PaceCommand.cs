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
            m_Pace = new Time();
        }

        override protected void ReadInternal(ResponseReader reader)
        {
            uint pace = reader.ReadUShort();
            reader.ReadByte(); // Expecting 0x39 - Seconds per kilometer

            m_Pace.TotalHundreths = 50 * pace;
        }

        public Time Pace { get { return m_Pace; } }

        private Time m_Pace;
    }
}
