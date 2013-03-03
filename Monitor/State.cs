using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public class State
    {
        public State Clone()
        {
            return (State)MemberwiseClone();
        }

        public bool Connected = false;
        public uint WorkTime = 0;
        public uint WorkDistance = 0;
        public WorkoutState WorkoutState = WorkoutState.Unknown;
        public StrokeState StrokeState = StrokeState.Unknown;
    }
}
