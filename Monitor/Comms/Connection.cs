using PM3Wrapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Comms
{
    class Connection : IConnection
    {
        public Connection()
        {
            m_PM3 = new PM3();
            m_Port = -1;
            m_Open = false;

            m_PM3.Start();
        }

        public bool IsOpen
        {
            get { return m_Open; }
        }

        public bool Open()
        {
            if (!m_Open)
            {
                int numUnits = m_PM3.DiscoverUnits();
                if (numUnits > 0)
                {
                    try
                    {
                        m_Port = 0;
                        m_Open = true;
                    }
                    catch (PM3Exception e)
                    {
                        Debug.WriteLine(string.Format("[Connection.Open] {0}", e.Message));
                    }
                }
            }
            else
            {
                Debug.WriteLine("[Connection.Open] Connection already open");
            }

            return m_Open;
        }

        public void Close()
        {
            m_Open = false;
            m_Port = -1;
        }

        public bool SendCSAFECommand(uint[] cmdData, int cmdDataCount, uint[] rspData, ref int rspDataCount)
        {
            if (m_Open)
            {
                try
                {
                    m_PM3.SendCSAFECommand(m_Port, cmdData, cmdDataCount, rspData, ref rspDataCount);
                    return true;
                }
                catch (WriteFailedException e)
                {
                    Debug.WriteLine(string.Format("[Connection.SendCSAFECommand] {0}", e.Message));
                }
                catch (ReadTimeoutException e)
                {
                    Debug.WriteLine(string.Format("[Connection.SendCSAFECommand] {0}", e.Message));
                }
                catch (DeviceClosedException e)
                {
                    Debug.WriteLine(string.Format("[Connection.SendCSAFECommand] {0}", e.Message));
                    m_Open = false;
                }
            }
            return false;
        }

        private PM3 m_PM3;
        private int m_Port;
        private bool m_Open;
    }
}
