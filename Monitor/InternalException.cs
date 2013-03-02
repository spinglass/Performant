using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public class CommandSetException : Exception
    {
        public CommandSetException(string message) :
            base(message)
        {
        }
    }

    public class BufferExceededException : Exception
    {
        public BufferExceededException(string message) :
            base(message)
        {
        }
    }
}
