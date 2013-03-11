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

        public ConnectionState ConnectionState = ConnectionState.Idle;

        // PM3 data
        public uint DragFactor = 0;
        public StrokeState StrokeState = StrokeState.Unknown;
        public Distance WorkDistance = new Distance();
        public WorkoutState WorkoutState = WorkoutState.Unknown;
        public WorkoutType WorkoutType = WorkoutType.Unknown;
        public Time WorkTime = new Time();

        // CSAFE data
        public uint Calories = 0;
        public Distance Distance = new Distance();
        public uint HeartRate = 0;
        public uint Power = 0;
        public Time Pace = new Time();
        public uint StrokeRate = 0;
        public Time Time = new Time();
    }
}
