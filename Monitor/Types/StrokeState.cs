using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public enum StrokeState : uint
    {
        Idle = 0,
        Catch = 1,
        Drive = 2,
        Dwell = 3,
        Recovery = 4,
        Unknown,
    }
}
