using Monitor.Comms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Commands
{
    class WorkoutStateCommand : PM3Command
    {
        public WorkoutStateCommand()
            : base(0x8D, 1)
        {
        }

        override protected void ReadInternal(ResponseReader reader)
        {
            uint val = reader.ReadByte();
            WorkoutState = Enum.IsDefined(typeof(WorkoutState), val) ? (WorkoutState)val : WorkoutState.Unknown;
        }

        public WorkoutState WorkoutState { get; private set; }
    }
}
