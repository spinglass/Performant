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
        public uint Centimetres { get; set; }

        public uint RoundedMetres
        {
            get { return (Centimetres < 50) ? Metres : Metres + 1; }
        }

        public override string ToString()
        {
           return string.Format("{0}.{1:D02}", Metres, Centimetres);
        }
    }
}
