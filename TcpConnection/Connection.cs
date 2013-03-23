using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpConnection
{
    public class Connection : IConnection
    {
        public Connection(string hostname, int port)
        {
            m_HostName = hostname;
            m_Port = port;
            m_Client = new TcpClient();
        }

        public bool IsOpen
        {
            get { return m_Client.Connected; }
        }

        public bool Open()
        {
            try
            {
                m_Client.Connect(m_HostName, m_Port);
            }
            catch (SocketException)
            {
            }

            return m_Client.Connected;
        }

        public void Close()
        {
            m_Client.Close();
        }

        public bool SendCSAFECommand(uint[] cmdData, int cmdDataCount, uint[] rspData, ref int rspDataCount)
        {
            return false;
        }

        private string m_HostName;
        private int m_Port;
        private TcpClient m_Client;
    }
}
