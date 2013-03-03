using Monitor.Comms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Commands
{
    class WorkDistanceCommand : PM3Command
    {
        public WorkDistanceCommand()
            : base(CSAFE.PM_GET_WORKDISTANCE, 5)
        {
            m_Distance = new Distance();
        }

        override protected void ReadInternal(ResponseReader reader)
        {
            m_Distance.Metres = reader.ReadUInt() / 10;
            m_Distance.Tenths = reader.ReadByte();
        }

        public Distance WorkDistance { get { return m_Distance; } }

        private Distance m_Distance;
    }
}
