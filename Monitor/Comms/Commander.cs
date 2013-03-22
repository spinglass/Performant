using Monitor.Commands;
using PM3Wrapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Comms
{
    class Commander
    {
        public Commander(IConnection connection)
        {
            m_Connection = connection;
            m_RspReader = new ResponseReader(64);
        }

        public bool Send(CommandSet cmdSet)
        {
            bool success = false;

            if (cmdSet.CanSend)
            {
                int rspDataCount = m_RspReader.Capacity;
                if (m_Connection.SendCSAFECommand(cmdSet.Buffer, cmdSet.Size, m_RspReader.Buffer, ref rspDataCount))
                {
                    // Reset the reader to the correct length
                    m_RspReader.Reset(rspDataCount);

                    // Read the response
                    success = cmdSet.Read(m_RspReader);
                }
            }
            else
            {
                Debug.WriteLine("[Commander.Send] Nothing to send");
            }

            return success;
        }

        IConnection m_Connection;
        ResponseReader m_RspReader;
    }
}
