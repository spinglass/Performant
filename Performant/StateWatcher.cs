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
        public StateWatcher(Dispatcher dispatcher, StateView stateView, Monitor.Monitor monitor)
        {
            m_Dispatcher = dispatcher;
            m_StateView = stateView;
            m_Monitor = monitor;
            m_Thread = new Thread(ThreadProc);
            m_Thread.Name = "StateWatcher";
            m_Update = new Action(Update);
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

        private void ThreadProc()
        {
            while (!m_Quit)
            {
                m_Dispatcher.Invoke(DispatcherPriority.Normal, m_Update);

                Thread.Sleep(250);
            }
        }

        private void Update()
        {
            State state = m_Monitor.GetState();
            m_StateView.WorkDistance = state.WorkDistance;
            m_StateView.WorkTime = state.WorkTime;
            m_StateView.StrokeState = state.StrokeState;
            m_StateView.WorkoutState = state.WorkoutState;
        }

        private Dispatcher m_Dispatcher;
        private StateView m_StateView;
        Monitor.Monitor m_Monitor;
        private Thread m_Thread;
        private Action m_Update;
        private bool m_Quit;
    }
}
