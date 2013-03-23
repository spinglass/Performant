using Common;
using DirectConnection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PerformantServer
{
    enum PM3State
    {
        Connected,
        Disconnected,
    }

    class Server
    {
        public delegate void ConnectionHandler(object sender, PM3State state);
        public event ConnectionHandler ConnectionChanged;

        public Server()
        {
            m_Listener = new Listener(7474);
            m_TcpTimeout = new Stopwatch();
            m_Connection = new Connection();
            m_PM3State = PM3State.Disconnected;
            m_FrameCount = 0;

            m_Thread = new Thread(ThreadProc);
            m_Thread.Name = "Server";
            m_Quit = false;
        }

        public void Start()
        {
            m_Thread.Start();
            m_Listener.Start();
        }

        public void Stop()
        {
            m_Quit = true;
            m_Listener.Stop();
            m_Thread.Join();
        }

        private void SetConnectionState(PM3State state)
        {
            if (m_PM3State != state)
            {
                if (ConnectionChanged != null)
                {
                    ConnectionChanged(this, state);
                }
            }
            m_PM3State = state;
        }

        private bool SendKeepAlive()
        {
            // Send empty command to test connection
            uint[] cmd = new uint[0];
            uint[] rsp = new uint[16];
            int rspLength = rsp.Length;
            return m_Connection.SendCSAFECommand(cmd, cmd.Length, rsp, ref rspLength);
        }

        private void UpdatePM3Connection()
        {
            switch (m_PM3State)
            {
                case PM3State.Disconnected:
                    if (m_FrameCount % 100 == 0)
                    {
                        if (m_Connection.Open())
                        {
                            SetConnectionState(PM3State.Connected);
                            Debug.WriteLine("Server: PM3 connected");
                        }
                    }
                    break;

                case PM3State.Connected:
                    if (m_FrameCount % 100 == 0)
                    {
                        SendKeepAlive();
                        if (!m_Connection.IsOpen)
                        {
                            SetConnectionState(PM3State.Disconnected);
                            Debug.WriteLine("Server: PM3 lost");
                        }
                    }
                    break;
            }
        }

        private void UpdateTcpConnection()
        {
            if (m_Stream == null)
            {
                // No current connection
                m_Stream = m_Listener.Stream;
                if (m_Stream != null)
                {
                    m_Stream.ReadTimeout = 10;
                    m_Stream.WriteTimeout = 10;

                    m_TcpTimeout.Restart();

                    Debug.WriteLine("Server: Client connected");
                }
            }
            else
            {
                if (m_Stream.DataAvailable)
                {
                    m_TcpTimeout.Restart();

                    if (m_PM3State == PM3State.Connected)
                    {
                    }
                    else
                    {
                    }
                }
                else
                {
                    // Ensure the client is still talking to us
                    if (m_TcpTimeout.ElapsedMilliseconds > 1000)
                    {
                        m_Listener.CloseCurrent();
                        m_Stream = null;
                        Debug.WriteLine("Server: Client lost");
                    }
                }
            }
        }

        private void ThreadProc()
        {
            while (!m_Quit)
            {
                UpdatePM3Connection();
                UpdateTcpConnection();

                ++m_FrameCount;
            }
        }

        private IConnection m_Connection;
        private Listener m_Listener;
        private NetworkStream m_Stream;
        private Stopwatch m_TcpTimeout;
        private Thread m_Thread;
        private bool m_Quit;
        private PM3State m_PM3State;
        private int m_FrameCount;
    }
}
