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
        }

        override protected void ReadInternal(ResponseReader reader)
        {
            StrokePace = reader.ReadUShort();
            reader.ReadByte(); // Expecting 0x39 - Seconds per kilometer
        }

        public uint StrokePace { get; private set; }
    }
}
