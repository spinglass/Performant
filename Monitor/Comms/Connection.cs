using PM3Wrapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Comms
{
    class Connection
    {
        public Connection()
        {
            m_PM3 = new PM3();
            m_Port = -1;

            m_PM3.Start();
        }

        public bool IsOpen { get { return m_Open; } }

        public bool Open()
        {
            int numUnits = m_PM3.DiscoverUnits();
            if (numUnits > 0)
            {
                m_Port = 0;
                m_Open = true;

                try
                {
                    UnitInfo info = m_PM3.GetUnitInfo(0);
                }
                catch (PM3Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("[Connection.Start] {0}", e.Message));
                }
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
                catch (PM3Exception e)
                {
                    Debug.WriteLine(string.Format("[Connection.SendCSAFECommand] {0}", e.Message));
                }
            }
            return false;
        }

        private PM3 m_PM3;
        private int m_Port;
        private bool m_Open;
    }
}
