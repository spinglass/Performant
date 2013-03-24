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
            get { return (m_Client != null && m_Client.Connected); }
        }

        public bool Open()
        {
            if (m_Client == null)
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
                        Debug.WriteLine("Connection: Opened");

                        // Much faster timeout now we're connected
                        stream.ReadTimeout = 10;
                    }
                    else
                    {
                        m_Sender.Stream = null;
                    }
                }
                catch (SocketException e)
                {
                    Debug.WriteLine("[Connection.Open] " + e.Message);
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

        private string m_HostName;
        private int m_Port;
        private TcpClient m_Client;
        private Sender m_Sender;
    }
}
