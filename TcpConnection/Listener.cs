using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpConnection
{
    class Listener
    {
        public Listener(int port)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            m_Listener = new TcpListener(ip, port);
            m_AcceptNext = false;

            m_Thread = new Thread(ThreadProc);
            m_Thread.Name = "Listener";
            m_Quit = false;
        }

        public TcpClient Client
        {
            get { return m_Client; }
        }

        public void Start()
        {
            m_Listener.Start(1);
            m_Thread.Start();
        }

        public void Stop()
        {
            m_Quit = true;
            m_Listener.Stop();
            m_Thread.Join();
        }

        public void AcceptNext()
        {
            m_Client = null;
            m_AcceptNext = true;
        }

        private void ThreadProc()
        {
            while (!m_Quit)
            {
                try
                {
                    m_Client = m_Listener.AcceptTcpClient();
                    
                    // Don't wait for another client, just wait until this one
                    // disconnects, or it's time to quit
                    while (!m_Quit && !m_AcceptNext)
                    {
                        Thread.Sleep(10);
                    }

                    m_AcceptNext = false;
                }
                catch (SocketException e)
                {
                    Debug.WriteLine("[Listener.ThreadProc] " + e.Message);
                }
            }
        }

        private TcpListener m_Listener;
        private TcpClient m_Client;
        private Thread m_Thread;
        private bool m_Quit;
        private bool m_AcceptNext;
    }
}
