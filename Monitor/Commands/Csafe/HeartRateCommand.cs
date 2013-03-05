using Monitor.Comms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Commands
{
    class HeartRateCommand : Command
    {
        public HeartRateCommand()
            : base(CSAFE.GETHRCUR_CMD, 1)
        {
            HeartRate = 0;
        }

        override protected void ReadInternal(ResponseReader reader)
        {
            HeartRate = reader.ReadByte();
        }

        public uint HeartRate { get; private set; }
    }
}
