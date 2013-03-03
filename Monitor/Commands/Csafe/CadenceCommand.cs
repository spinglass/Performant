using Monitor.Comms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Commands
{
    class CadenceCommand : Command
    {
        public CadenceCommand()
            : base(CSAFE.GETCADENCE_CMD, 3)
        {
        }

        override protected void ReadInternal(ResponseReader reader)
        {
            StrokeRate = reader.ReadUShort();
            reader.ReadByte(); // Expecting 0x54 - StrokesPerMinute
        }

        public uint StrokeRate { get; private set; }
    }
}
