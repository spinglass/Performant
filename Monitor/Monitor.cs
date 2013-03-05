using Monitor.Commands;
using Monitor.Comms;
using PM3Wrapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Monitor
{
    public class Monitor
    {
        public Monitor()
        {
            m_Connection = new Connection();
            m_Commander = new Commander(m_Connection);
            m_StateReader = new StateReader(m_Commander);
 
            m_Thread = new Thread(ThreadProc);
            m_Thread.Name = "Monitor";
            m_Quit = false;

            m_ConnectionState = ConnectionState.Idle;
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

        public State GetState()
        {
            State state = m_StateReader.GetState();

            // Inject connection state
            state.ConnectionState = m_Connection.State;

            return state;
        }

        private void ThreadProc()
        {
            while (!m_Quit)
            {
                switch (m_ConnectionState)
                {
                    case ConnectionState.Idle:
                        // Attempt to start the connection
                        if (m_Connection.Open())
                        {
                            Debug.WriteLine("Connection: opened");
                        }
                        break;

                    case ConnectionState.Connected:
                    case ConnectionState.SendError:
                        bool success = m_StateReader.Update();

                        if (!success)
                        {
                            if (!m_Connection.IsOpen)
                            {
                                Debug.WriteLine("Connection: lost");
                            }
                            else
                            {
                                Debug.WriteLine("Connection: send error");
                            }
                        }
                        break;

                    case ConnectionState.Lost:
                        // Attempt to reconnect
                        if (m_Connection.Reopen())
                        {
                            Debug.WriteLine("Connection: re-opened");
                        }
                        break;
                }
                m_ConnectionState = m_Connection.State;

                Thread.Sleep(10);
            }

            m_Connection.Close();
        }

        private Connection m_Connection;
        private Commander m_Commander;
        private StateReader m_StateReader;
        private Thread m_Thread;
        private bool m_Quit;
        private ConnectionState m_ConnectionState;
    }
}
