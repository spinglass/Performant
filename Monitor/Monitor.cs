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
            m_frameCount = 0;
 
            m_Thread = new Thread(ThreadProc);
            m_Thread.Name = "Monitor";

            SetupCommandSets();
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

        private void SetupCommandSets()
        {
            m_100HzCommands = new CommandSet();
            m_100HzCommands.Add(m_StrokeStateCommand);
            m_100HzCommands.Prepare();

            m_10HzCommands = new CommandSet();
            m_10HzCommands.Add(m_WorkDistanceCommand);
            m_10HzCommands.Add(m_WorkTimeCommand);
            m_10HzCommands.Prepare();

            m_2HzCommands = new CommandSet();
            m_2HzCommands.Add(m_CadenceCommand);
            m_2HzCommands.Add(m_CaloriesCommand);
            m_2HzCommands.Add(m_DragFactorCommand);
            m_2HzCommands.Add(m_PaceCommand);
            m_2HzCommands.Add(m_PowerCommand);
            m_2HzCommands.Add(m_WorkoutStateCommand);
            m_2HzCommands.Prepare();

            m_1HzCommands = new CommandSet();
            m_1HzCommands.Add(m_HeartRateCommand);
            m_1HzCommands.Prepare();

            m_PerWorkoutCommands = new CommandSet();
            m_PerWorkoutCommands.Add(m_WorkoutTypeCommand);
            m_PerWorkoutCommands.Prepare();
        }

        private void Send(CommandSet cmdSet)
        {
            if (m_Connection.IsOpen)
            {
                if (m_Commander.Send(cmdSet))
                {
                    UpdateState();
                }
                else
                {
                    if (m_Connection.State != m_ConnectionState)
                    {
                        if (m_Connection.State == ConnectionState.SendError)
                        {
                            Debug.WriteLine("Connection: send error");
                        }
                    }
                }
            }
        }

        private void UpdateState()
        {
            lock (m_State)
            {
                m_State.ConnectionState = m_Connection.State;

                // PM3 commands
                m_State.DragFactor = m_DragFactorCommand.DragFactor;
                m_State.StrokeState = m_StrokeStateCommand.StrokeState;
                m_State.WorkDistance = m_WorkDistanceCommand.WorkDistance;
                m_State.WorkoutState = m_WorkoutStateCommand.WorkoutState;
                m_State.WorkoutType = m_WorkoutTypeCommand.WorkoutType;
                m_State.WorkTime = m_WorkTimeCommand.WorkTime;

                // CSAFE commands
                m_State.Calories = m_CaloriesCommand.Calories;
                m_State.HeartRate = m_HeartRateCommand.HeartRate;
                m_State.Power = m_PowerCommand.Power;
                m_State.StrokePace = m_PaceCommand.StrokePace;
                m_State.StrokeRate = m_CadenceCommand.StrokeRate;
            }
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
                        
                        Send(m_100HzCommands);
                        if (m_frameCount % 10 == 0)
                        {
                            Send(m_10HzCommands);
                        }
                        if (m_frameCount % 50 == 0)
                        {
                            Send(m_2HzCommands);
                        }
                        if (m_frameCount % 100 == 0)
                        {
                            Send(m_1HzCommands);
                        }

                        ++m_frameCount;

                        if (!m_Connection.IsOpen)
                        {
                            Debug.WriteLine("Connection: lost");
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
        private int m_frameCount;

        // Commands
        private CadenceCommand m_CadenceCommand = new CadenceCommand();
        private CaloriesCommand m_CaloriesCommand = new CaloriesCommand();
        private DragFactorCommand m_DragFactorCommand = new DragFactorCommand();
        private HeartRateCommand m_HeartRateCommand = new HeartRateCommand();
        private PaceCommand m_PaceCommand = new PaceCommand();
        private PowerCommand m_PowerCommand = new PowerCommand();
        private StrokeStateCommand m_StrokeStateCommand = new StrokeStateCommand();
        private WorkDistanceCommand m_WorkDistanceCommand = new WorkDistanceCommand();
        private WorkoutStateCommand m_WorkoutStateCommand = new WorkoutStateCommand();
        private WorkoutTypeCommand m_WorkoutTypeCommand = new WorkoutTypeCommand();
        private WorkTimeCommand m_WorkTimeCommand = new WorkTimeCommand();

        // Command sets
        private CommandSet m_100HzCommands;
        private CommandSet m_10HzCommands;
        private CommandSet m_2HzCommands;
        private CommandSet m_1HzCommands;
        private CommandSet m_PerWorkoutCommands;
    }
}
