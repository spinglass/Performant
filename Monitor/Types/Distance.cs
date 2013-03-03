using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public struct Distance
    {
        public uint Metres { get; set; }
        public uint Tenths { get; set; }

        public override string ToString()
        {
            return string.Format("{0}.{1}", Metres, Tenths);
        }
    }
}
