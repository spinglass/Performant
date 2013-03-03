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
        public Time WorkTime = new Time();
        public Distance WorkDistance = new Distance();
        public WorkoutState WorkoutState = WorkoutState.Unknown;
        public StrokeState StrokeState = StrokeState.Unknown;
    }
}
