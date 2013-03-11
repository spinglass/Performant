using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public struct Distance
    {
        public uint Metres
        {
            get { return TotalTenths / 10; }
        }

        public uint TotalTenths { get; set; }
        public uint Tenths
        {
            get { return TotalTenths - 10 * Metres; }
        }

        public double Double
        {
            get { return 0.1 * (double)TotalTenths; }
            set { TotalTenths = (uint)(10.0 * value); }
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}", Metres, Tenths);
        }
    }
}
