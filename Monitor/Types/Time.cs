using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public struct Time
    {
        public uint TotalSeconds { get; set; }
        public uint Hundreths { get; set; }

        public uint Hours
        {
            get { return TotalSeconds / 3600;  }
        }

        public uint TotalMinutes
        {
            get { return TotalSeconds / 60; }
        }
        public uint Minutes
        {
            get { return TotalMinutes - 60 * Hours; }
        }

        public uint Seconds
        {
            get { return TotalSeconds - 60 * TotalMinutes; }
        }

        public override string ToString()
        {
            return string.Format("{0}:{1:D02}:{2:D02}.{3:D02}", Hours, Minutes, Seconds, Hundreths);
        }

        public string Concise
        {
            get
            {
                if (Hours > 0)
                {
                    return string.Format("{0}:{1:D02}:{2:D02}", Hours, Minutes, Seconds);
                }
                else if (Minutes > 0)
                {
                    return string.Format("{0}:{1:D02}", Minutes, Seconds);
                }
                return string.Format(":{0:D02}", Seconds);
            }
        }
    }
}
