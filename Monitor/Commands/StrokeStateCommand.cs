using Monitor.Comms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Commands
{
    class StrokeStateCommand : PM3Command
    {
        public StrokeStateCommand()
            : base(CSAFE.PM_GET_STROKESTATE, 1)
        {
        }

        override protected void ReadInternal(ResponseReader reader)
        {
            uint val = reader.ReadByte();
            StrokeState = Enum.IsDefined(typeof(StrokeState), val) ? (StrokeState)val : StrokeState.Unknown;
        }

        public StrokeState StrokeState { get; private set; }
    }
}
