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
            m_State = new State();

            m_Quit = false;
            m_Thread = new Thread(ThreadProc);
            m_Thread.Name = "Monitor";
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
            State state;
            lock (m_State)
            {
                state = m_State.Clone();
            }
            return state;
        }

        private void Send()
        {
            CommandSet cmdSet = new CommandSet();
            cmdSet.Add(m_WorkDistance);
            cmdSet.Add(m_WorkTime);
            cmdSet.Add(m_StrokeState);
            cmdSet.Add(m_WorkoutState);
            cmdSet.Prepare();

            if (m_Commander.Send(cmdSet))
            {
                m_State.WorkDistance = m_WorkDistance.WorkDistance;
                m_State.WorkTime = m_WorkTime.WorkTime;
                m_State.StrokeState = m_StrokeState.StrokeState;
                m_State.WorkoutState = m_WorkoutState.WorkoutState;
            }
        }

        private void ThreadProc()
        {
            while (!m_Quit)
            {
                lock (m_State)
                {
                    switch (m_ConnectionState)
                    {
                        case ConnectionState.Idle:
                            // Attempt to start the connection
                            if (m_Connection.Open())
                            {
                                Debug.WriteLine("Connection: opened");

                                m_State.ConnectionState = m_Connection.State;
                            }
                            break;

                        case ConnectionState.Connected:
                        case ConnectionState.SendError:
                            Send();

                            if (m_Connection.IsOpen)
                            {
                                if (m_Connection.State != m_ConnectionState)
                                {
                                    if (m_Connection.State == ConnectionState.SendError)
                                    {
                                        Debug.WriteLine("Connection: send error");
                                    }

                                    m_State.ConnectionState = m_Connection.State;
                                }
                            }
                            else
                            {
                                Debug.WriteLine("Connection: lost");

                                m_State.ConnectionState = m_Connection.State;
                            }
                            break;

                        case ConnectionState.Lost:
                            // Attempt to reconnect
                            if (m_Connection.Reopen())
                            {
                                Debug.WriteLine("Connection: re-opened");

                                m_State.ConnectionState = m_Connection.State;
                            }
                            break;
                    }
                }
                m_ConnectionState = m_Connection.State;

                Thread.Sleep(10);
            }

            if (m_Connection.IsOpen)
            {
                m_Connection.Close();
            }
        }

        private Connection m_Connection;
        private ConnectionState m_ConnectionState;
        private Commander m_Commander;
        private State m_State;
        private Thread m_Thread;
        private bool m_Quit;

        // Commands
        private WorkDistanceCommand m_WorkDistance = new WorkDistanceCommand();
        private WorkTimeCommand m_WorkTime = new WorkTimeCommand();
        private StrokeStateCommand m_StrokeState = new StrokeStateCommand();
        private WorkoutStateCommand m_WorkoutState = new WorkoutStateCommand();
    }
}
