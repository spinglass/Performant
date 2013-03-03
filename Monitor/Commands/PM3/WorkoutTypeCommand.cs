using Monitor.Comms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Commands
{
    class WorkoutTypeCommand : PM3Command
    {
        public WorkoutTypeCommand()
            : base(CSAFE.PM_GET_WORKOUTTYPE, 1)
        {
        }

        override protected void ReadInternal(ResponseReader reader)
        {
            uint val = reader.ReadByte();
            WorkoutType = Enum.IsDefined(typeof(WorkoutType), val) ? (WorkoutType)val : WorkoutType.Unknown;
        }

        public WorkoutType WorkoutType { get; private set; }
    }
}
