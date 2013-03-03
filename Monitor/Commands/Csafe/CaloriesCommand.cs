using Monitor.Comms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Commands
{
    class CaloriesCommand : Command
    {
        public CaloriesCommand()
            : base(CSAFE.GETCALORIES_CMD, 2)
        {
        }

        override protected void ReadInternal(ResponseReader reader)
        {
            Calories = reader.ReadUShort();
        }

        public uint Calories { get; private set; }
    }
}
