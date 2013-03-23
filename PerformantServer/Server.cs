using Common;
using DirectConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PerformantServer
{
    enum ServerConnectionState
    {
        Connected,
        Disconnected,
    }

    class Server
    {
        public delegate void ConnectionHandler(object sender, ServerConnectionState state);
        public event ConnectionHandler ConnectionChanged;

        public Server()
        {
            m_Connection = new Connection();
            m_ConnectionState = ServerConnectionState.Disconnected;

            m_Thread = new Thread(ThreadProc);
            m_Thread.Name = "Server";
            m_Quit = false;
        }

        public void Start()
        {
            m_Thread.Start();
        }

        public void Stop()
        {
            m_Quit = true;
            m_Thread.Join();
        }

        private void SetConnectionState(ServerConnectionState state)
        {
            if (m_ConnectionState != state)
            {
                if (ConnectionChanged != null)
                {
                    ConnectionChanged(this, state);
                }
            }
            m_ConnectionState = state;
        }

        private void ThreadProc()
        {
            while (!m_Quit)
            {
                switch (m_ConnectionState)
                {
                    case ServerConnectionState.Disconnected:
                        if (m_Connection.Open())
                        {
                            SetConnectionState(ServerConnectionState.Connected);
                        }
                        break;

                    case ServerConnectionState.Connected:
                        break;
                }

                Thread.Sleep(10);
            }
        }

        private IConnection m_Connection;
        private Thread m_Thread;
        private bool m_Quit;
        private ServerConnectionState m_ConnectionState;
    }
}
