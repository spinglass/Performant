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
            StrokeState = StrokeState.Unknown;
        }

        override protected void ReadInternal(ResponseReader reader)
        {
            StrokeState val = (StrokeState)reader.ReadByte();
            StrokeState = Enum.IsDefined(typeof(StrokeState), val) ? val : StrokeState.Unknown;
        }

        public StrokeState StrokeState { get; private set; }
    }
}
