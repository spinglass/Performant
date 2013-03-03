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
        }

        override protected void ReadInternal(ResponseReader reader)
        {
            Seconds = reader.ReadUInt() / 100;
            Hundreths = reader.ReadByte();
        }

        public uint Seconds { get; private set; }
        public uint Hundreths { get; private set; }
    }
}
