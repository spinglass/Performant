using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM3Wrapper
{
    public class DeviceClosedException : PM3Exception
    {
        public DeviceClosedException(string message) :
            base(message)
        {
        }
    }

    public class InvalidPortException : PM3Exception
    {
        public InvalidPortException(string message) :
            base(message)
        {
        }
    }

    public class WriteFailedException : PM3Exception
    {
        public WriteFailedException(string message) :
            base(message)
        {
        }
    }

    public class UnknownPM3Exception : PM3Exception
    {
        public UnknownPM3Exception(string message)
            : base(message)
        {
        }
    }
}
