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
            m_State = ConnectionState.Disconnected;

            m_PM3.Start();
        }

        public bool IsOpen
        {
            get { return m_State == ConnectionState.Connected || m_State == ConnectionState.SendError; }
        }

        public ConnectionState State { get { return m_State; } }

        public bool Open()
        {
            if (m_State == ConnectionState.Disconnected)
            {
                int numUnits = m_PM3.DiscoverUnits();
                if (numUnits > 0)
                {
                    try
                    {
                        m_UnitInfo = m_PM3.GetUnitInfo(0);

                        m_Port = 0;
                        m_State = ConnectionState.Connected;
                    }
                    catch (PM3Exception e)
                    {
                        Debug.WriteLine(string.Format("[Connection.Open] {0}", e.Message));
                    }
                }
            }
            else
            {
                Debug.WriteLine("[Connection.Open] Can only attempt to open a connection from Disconnected");
            }

            return (m_State == ConnectionState.Connected);
        }

        public void Close()
        {
            m_State = ConnectionState.Disconnected;
            m_Port = -1;
        }

        public bool SendCSAFECommand(uint[] cmdData, int cmdDataCount, uint[] rspData, ref int rspDataCount)
        {
            if (IsOpen)
            {
                try
                {
                    m_PM3.SendCSAFECommand(m_Port, cmdData, cmdDataCount, rspData, ref rspDataCount);
                    m_State = ConnectionState.Connected;
                    return true;
                }
                catch (WriteFailedException e)
                {
                    Debug.WriteLine(string.Format("[Connection.SendCSAFECommand] {0}", e.Message));
                    m_State = ConnectionState.SendError;
                }
                catch (ReadTimeoutException e)
                {
                    Debug.WriteLine(string.Format("[Connection.SendCSAFECommand] {0}", e.Message));
                    m_State = ConnectionState.SendError;
                }
                catch (DeviceClosedException e)
                {
                    Debug.WriteLine(string.Format("[Connection.SendCSAFECommand] {0}", e.Message));
                    m_State = ConnectionState.Disconnected;
                }
            }
            return false;
        }

        private PM3 m_PM3;
        private int m_Port;
        private ConnectionState m_State;
        private UnitInfo m_UnitInfo;
    }
}
