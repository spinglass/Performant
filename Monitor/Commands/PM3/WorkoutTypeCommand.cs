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
            WorkoutType val = (WorkoutType)reader.ReadByte();
            WorkoutType = Enum.IsDefined(typeof(WorkoutType), val) ? val : WorkoutType.Unknown;
        }

        public WorkoutType WorkoutType { get; private set; }
    }
}
