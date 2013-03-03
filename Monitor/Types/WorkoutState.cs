using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public enum WorkoutState : uint
    {
        WaitingToBegin = 0,
        WorkoutRow = 1,
        CountdownPause = 2,
        IntervalRest = 3,
        WorkTimeInterval = 4,
        WorkDistanceInterval = 5,
        RestIntervalEndToWorkTimeIntervalBegin = 6,
        RestInvervalEndToWorkDistanceIntervalBegin = 7,
        WorkTimeIntervalEndToRestIntervalBegin = 8,
        WorkDistanceIntervalEndToRestIntervalBegin = 9,
        WorkoutEnd = 10,
        WorkoutTerminate = 11,
        WorkoutLogged = 12,
        WorkoutRearm = 13,
        Unknown
    }
}
