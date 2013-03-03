using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Commands
{
    abstract class PM3Command : Command
    {
        public PM3Command(CSAFE id, uint rspSize)
            : base(id, rspSize)
        {
        }
    }
}
