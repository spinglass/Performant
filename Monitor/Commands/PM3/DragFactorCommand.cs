using Monitor.Comms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Commands
{
    class DragFactorCommand : PM3Command
    {
        public DragFactorCommand()
            : base(CSAFE.PM_GET_DRAGFACTOR, 1)
        {
            DragFactor = 0;
        }

        override protected void ReadInternal(ResponseReader reader)
        {
            DragFactor = (uint)reader.ReadByte();
        }

        public uint DragFactor { get; private set; }
    }
}
