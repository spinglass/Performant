using Common;
using Monitor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Performant
{
    class StateWatcher
    {
        private class Updater
        {
            public delegate Object Getter(State state);
            public delegate void Setter(Object tgt);

            public Updater(Getter getter, Setter setter)
            {
                m_Getter = getter;
                m_Setter = setter;
            }

            public void Update(State state)
            {
                // Read the registered value from the state
                Object value = m_Getter(state);

                // If value has changed from last read...
                if (!value.Equals(m_Value))
                {
                    // ... apply it to the view
                    m_Setter(value);
                    m_Value = value;
                }
            }

            private Setter m_Setter;
            private Getter m_Getter;
            private Object m_Value;
        }

        public StateWatcher(Dispatcher dispatcher, StateView stateView, Controller controller)
        {
            m_Dispatcher = dispatcher;
            m_View = stateView;
            m_Controller = controller;
            m_InvokeTimeout = new TimeSpan(500000); // 50 ms
            m_UpdateTimer = Stopwatch.StartNew();

            CreateUpdaters();

            // Register for state updates
            m_Controller.StateUpdate += OnStateUpdate;
        }

        private void CreateUpdaters()
        {
            // Build the mapping from state to view
            m_Updaters = new List<Updater>();
            m_Updaters.Add(new Updater((State state) => state.ConnectionState, (Object value) => m_View.ConnectionState = (ConnectionState)value));

            // PM3 data
            m_Updaters.Add(new Updater((State state) => state.DragFactor, (Object value) => m_View.DragFactor = (uint)value));
            m_Updaters.Add(new Updater((State state) => state.StrokeState, (Object value) => m_View.StrokeState = (StrokeState)value));
            m_Updaters.Add(new Updater((State state) => state.WorkDistance, (Object value) => m_View.WorkDistance = (Distance)value));
            m_Updaters.Add(new Updater((State state) => state.WorkoutState, (Object value) => m_View.WorkoutState = (WorkoutState)value));
            m_Updaters.Add(new Updater((State state) => state.WorkoutType, (Object value) => m_View.WorkoutType = (WorkoutType)value));
            m_Updaters.Add(new Updater((State state) => state.WorkTime, (Object value) => m_View.WorkTime = (Time)value));

            // CSAFE data
            m_Updaters.Add(new Updater((State state) => state.Calories, (Object value) => m_View.Calories = (uint)value));
            m_Updaters.Add(new Updater((State state) => state.HeartRate, (Object value) => m_View.HeartRate = (uint)value));
            m_Updaters.Add(new Updater((State state) => state.Power, (Object value) => m_View.Power = (uint)value));
            m_Updaters.Add(new Updater((State state) => state.Pace, (Object value) => m_View.Pace = (Time)value));
            m_Updaters.Add(new Updater((State state) => state.StrokeRate, (Object value) => m_View.StrokeRate = (uint)value));
        }

        private void OnStateUpdate(object sender, State state)
        {
            if (m_UpdateTimer.ElapsedMilliseconds > 100)
            {
                m_UpdateTimer.Restart();

                // Send state to UI
                State backupState = state.Clone();
                Action action = () => UpdateUI(backupState);
                m_Dispatcher.Invoke(DispatcherPriority.Normal, m_InvokeTimeout, action);
            }
       }

        private void UpdateUI(State state)
        {
            // Apply all the values of interest from the state to the view
            foreach (Updater updater in m_Updaters)
            {
                updater.Update(state);
            }
        }

        private Dispatcher m_Dispatcher;
        private StateView m_View;
        Controller m_Controller;
        private List<Updater> m_Updaters;
        private TimeSpan m_InvokeTimeout;
        private Stopwatch m_UpdateTimer;
    }
}
