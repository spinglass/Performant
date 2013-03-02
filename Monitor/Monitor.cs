using Monitor.Commands;
using Monitor.Comms;
using PM3Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            if (m_Connection.Open())
            {
                m_State.Connected = true;

                WorkDistanceCommand workDistance = new WorkDistanceCommand();
                WorkTimeCommand workTime = new WorkTimeCommand();
                StrokeStateCommand strokeState = new StrokeStateCommand();
                WorkoutStateCommand workoutState = new WorkoutStateCommand();

                CommandSet cmdSet = new CommandSet();
                cmdSet.Add(workDistance);
                cmdSet.Add(workTime);
                cmdSet.Add(strokeState);
                cmdSet.Add(workoutState);
                cmdSet.Prepare();

                if (m_Commander.Send(cmdSet))
                {
                    m_State.WorkDistance = workDistance.Metres;
                    m_State.WorkTime = workTime.Seconds;
                    m_State.StrokeState = strokeState.StrokeState;
                    m_State.WorkoutState = workoutState.WorkoutState;
                }

                m_Connection.Close();
            }
            else
            {
                m_State.Connected = false;
            }
        }

        private Connection m_Connection;
        private Commander m_Commander;
        private State m_State;
    }
}
