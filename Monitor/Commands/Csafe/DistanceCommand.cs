using Monitor.Comms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Commands
{
    class DistanceCommand : Command
    {
        public DistanceCommand()
            : base(CSAFE.GETTWORK_CMD, 3)
        {
            m_Distance = new Distance();
        }

        override protected void ReadInternal(ResponseReader reader)
        {
            m_Distance.Metres = reader.ReadUShort();
            reader.ReadByte(); // Expecting 0x24 - Metres
        }

        public Distance Distance { get { return m_Distance; } }

        private Distance m_Distance;
    }
}
