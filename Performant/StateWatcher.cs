using Monitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Performant
{
    class StateWatcher
    {
        class Updater
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
            m_Thread = new Thread(ThreadProc);
            m_Thread.Name = "StateWatcher";
            m_Update = new Action(Update);
            m_InvokeTimeout = new TimeSpan(1000000); // 100 ms

            CreateUpdaters();
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

        private void ThreadProc()
        {
            while (!m_Quit)
            {
                m_Dispatcher.Invoke(DispatcherPriority.Normal, m_InvokeTimeout, m_Update);

                Thread.Sleep(100);
            }
        }

        private void Update()
        {
            // Get the current (latest) state from the monitor
            State state = m_Controller.GetState();
            
            // Apply all the values of interest from the state to the view
            foreach (Updater updater in m_Updaters)
            {
                updater.Update(state);
            }
       }

        private Dispatcher m_Dispatcher;
        private StateView m_View;
        Controller m_Controller;
        private Thread m_Thread;
        private Action m_Update;
        private bool m_Quit;
        private List<Updater> m_Updaters;
        private TimeSpan m_InvokeTimeout;
    }
}
