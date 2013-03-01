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

                m_Commander.Add(workDistance);
                m_Commander.Add(workTime);
                m_Commander.Add(strokeState);
                m_Commander.Add(workoutState);
                m_Commander.Prepare();

                if (m_Commander.Send())
                {
                    m_State.WorkDistance = workDistance.Metres;
                    m_State.WorkTime = workTime.Seconds;
                    m_State.StrokeState = strokeState.StrokeState;
                    m_State.WorkoutState = workoutState.WorkoutState;
                }

                m_Connection.Close();
            }
        }

        private Connection m_Connection;
        private Commander m_Commander;
        private State m_State;
    }
}
