using Monitor.Comms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Commands
{
    class PowerCommand : Command
    {
        public PowerCommand()
            : base(CSAFE.GETPOWER_CMD, 3)
        {
            Power = 0;
        }

        override protected void ReadInternal(ResponseReader reader)
        {
            Power = reader.ReadUShort();
            reader.ReadByte(); // Expecting 0x58 - Watts
        }

        public uint Power { get; private set; }
    }
}
