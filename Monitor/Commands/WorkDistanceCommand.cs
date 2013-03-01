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
            : base(0xA3, 5)
        {
        }

        override protected void ReadInternal(ResponseReader reader)
        {
            Metres = reader.ReadUInt() / 10;
            Hundreths = reader.ReadByte();
        }

        public uint Metres { get; private set; }
        public uint Hundreths { get; private set; }
    }
}
