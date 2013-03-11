using Monitor.Comms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Commands
{
    class WorkTimeCommand : PM3Command
    {
        public WorkTimeCommand()
            : base(CSAFE.PM_GET_WORKTIME, 5)
        {
            m_Time = new Time();
        }

        override protected void ReadInternal(ResponseReader reader)
        {
            m_Time.TotalHundreths = reader.ReadUInt() + reader.ReadByte();
        }

        public Time WorkTime { get { return m_Time; } }

        private Time m_Time;
    }
}
