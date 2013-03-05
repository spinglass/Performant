using Monitor.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.Comms
{
    class StateReader
    {
        public StateReader(Commander commander)
        {
            m_Commander = commander;
            m_State = new State();
            m_UpdateCount = 0;

            SetupCommandSets();
        }

        public bool Update()
        {
            // Expecting this Update to be called at around 100Hz.

            bool success = SendCommands();

            if (success)
            {
                UpdateState();
            }

            ++m_UpdateCount;

            return success;
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

        private bool IsWorkoutActive()
        {
            return WorkoutState.WorkoutRow <= m_WorkoutStateCommand.WorkoutState
                && m_WorkoutStateCommand.WorkoutState < WorkoutState.WorkoutEnd;
        }

        private bool SendCommands()
        {
            bool success = true;

            if (IsWorkoutActive())
            {
                // Workout in progess

                success = m_Commander.Send(m_100HzCommands);

                if (success && m_UpdateCount % 10 == 0)
                {
                    success = m_Commander.Send(m_10HzCommands);
                }

                if (success && m_UpdateCount % 50 == 0)
                {
                    success = m_Commander.Send(m_2HzCommands);
                }

                if (success && m_UpdateCount % 100 == 0)
                {
                    success = m_Commander.Send(m_1HzCommands);
                }
            }
            else
            {
                // Waiting for workout to begin

                if (m_UpdateCount % 50 == 0)
                {
                    success = m_Commander.Send(m_PerWorkoutCommands);
                }
            }

            return success;
        }

        private void SetupCommandSets()
        {
            // Commands

            m_CadenceCommand = new CadenceCommand();
            m_CaloriesCommand = new CaloriesCommand();
            m_DragFactorCommand = new DragFactorCommand();
            m_HeartRateCommand = new HeartRateCommand();
            m_PaceCommand = new PaceCommand();
            m_PowerCommand = new PowerCommand();
            m_StrokeStateCommand = new StrokeStateCommand();
            m_WorkDistanceCommand = new WorkDistanceCommand();
            m_WorkoutStateCommand = new WorkoutStateCommand();
            m_WorkoutTypeCommand = new WorkoutTypeCommand();
            m_WorkTimeCommand = new WorkTimeCommand();

            // Command sets

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
            m_PerWorkoutCommands.Add(m_WorkoutStateCommand);
            m_PerWorkoutCommands.Prepare();
        }

        private void UpdateState()
        {
            lock (m_State)
            {
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

        private Commander m_Commander;
        private State m_State;
        private int m_UpdateCount;

        // Commands
        private CadenceCommand m_CadenceCommand;
        private CaloriesCommand m_CaloriesCommand;
        private DragFactorCommand m_DragFactorCommand;
        private HeartRateCommand m_HeartRateCommand;
        private PaceCommand m_PaceCommand;
        private PowerCommand m_PowerCommand;
        private StrokeStateCommand m_StrokeStateCommand;
        private WorkDistanceCommand m_WorkDistanceCommand;
        private WorkoutStateCommand m_WorkoutStateCommand;
        private WorkoutTypeCommand m_WorkoutTypeCommand;
        private WorkTimeCommand m_WorkTimeCommand;

        // Command sets
        private CommandSet m_100HzCommands;
        private CommandSet m_10HzCommands;
        private CommandSet m_2HzCommands;
        private CommandSet m_1HzCommands;
        private CommandSet m_PerWorkoutCommands;
    }
}
