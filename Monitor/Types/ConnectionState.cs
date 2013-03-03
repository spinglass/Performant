using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public enum ConnectionState
    {
        Idle,
        Connected,
        SendError,
        Lost,
    }
}
