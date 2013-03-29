using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TcpConnection.Protocol;

namespace TcpConnection
{
    public class Connection : IConnection
    {
        public Connection(string hostname, int port)
        {
            m_HostName = hostname;
            m_Port = port;

            m_Sender = new Sender();
        }

        public bool IsOpen
        {
            get { return (m_Sender.State == ConnectionState.Connected || m_Sender.State == ConnectionState.SendError); }
        }

        public ConnectionState State
        {
            get { return m_Sender.State; }
        }

        public bool Open()
        {
            if (m_Client == null)
            {
                OpenClient();
            }

            if (m_Client != null)
            {
                if (m_Client.Connected)
                {
                    m_Sender.TestConnection();
                }
                else
                {
                    Close();
                }
            }

            return IsOpen;
        }

        public void Close()
        {
            if (m_Client != null)
            {
                m_Client.Close();
                m_Client = null;
                Debug.WriteLine("[Connection.OpenClient] Connection to server closed");
            }
        }

        public bool SendCSAFECommand(uint[] cmdData, int cmdDataCount, uint[] rspData, ref int rspDataCount)
        {
            bool success = false;
            if (IsOpen)
            {
                success = m_Sender.SendCommand(cmdData, cmdDataCount, rspData, ref rspDataCount);
            }
            return success;
        }

        private void OpenClient()
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(m_HostName, m_Port);

                NetworkStream stream = client.GetStream();
                stream.ReadTimeout = 1000;
                stream.WriteTimeout = 10;
                m_Sender.Stream = stream;

                // Handshake with the server
                if (m_Sender.Handshake())
                {
                    m_Client = client;

                    // Much faster timeout now we're connected
                    stream.ReadTimeout = 10;

                    Debug.WriteLine("[Connection.OpenClient] Connected to server opened");
                }
                else
                {
                    client.Close();
                    m_Sender.Stream = null;
                }
            }
            catch (SocketException)
            {
            }
        }

        private string m_HostName;
        private int m_Port;
        private TcpClient m_Client;
        private Sender m_Sender;
    }
}
