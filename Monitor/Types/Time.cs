using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public struct Time
    {
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

        public uint TotalSeconds
        {
            get { return TotalHundreths / 100; }
        }
        public uint Seconds
        {
            get { return TotalSeconds - 60 * TotalMinutes; }
        }

        public uint TotalTenths
        {
            get { return TotalHundreths / 10; }
        }
        public uint Tenths
        {
            get { return Hundreths / 10; }
        }

        public uint TotalHundreths { get; set; }
        public uint Hundreths
        {
            get { return TotalHundreths - 100 * TotalSeconds; }
        }

        public double Double
        {
            get { return 0.01 * (double)TotalHundreths; }
            set { TotalHundreths = (uint)(100.0 * value); }
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

        public string ToTenths
        {
            get
            {
                if (Hours > 0)
                {
                    return string.Format("{0}:{1:D02}:{2:D02}.{3}", Hours, Minutes, Seconds, Tenths);
                }
                else if (Minutes > 0)
                {
                    return string.Format("{0}:{1:D02}.{2}", Minutes, Seconds, Tenths);
                }
                return string.Format(":{0:D02}.{1}", Seconds, Tenths);
            }
        }
    }
}
