using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Comms
{
    class TcpConnection : IConnection
    {
        public TcpConnection(string hostname, int port)
        {
        }

        public bool IsOpen
        {
            get { return false; }
        }

        public bool Open()
        {
            return false;
        }

        public void Close()
        {
        }

        public bool SendCSAFECommand(uint[] cmdData, int cmdDataCount, uint[] rspData, ref int rspDataCount)
        {
            return false;
        }
    }
}
