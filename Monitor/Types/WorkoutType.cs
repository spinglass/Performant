using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public enum WorkoutType
    {
        JustRowNoSplits = 0,
        JustRowSplits = 1,
        FixedDistanceNoSplits = 2,
        FixedDistanceSplits = 3,
        FixedTimeNoSplits = 4,
        FixedTimeSplits = 5,
        FixedTimeInterval = 6,
        FixedDistanceInterval = 7,
        VariableInterval = 8,

        Unknown,
    }
}
