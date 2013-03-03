using Monitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Performant
{
    public class StateView : DependencyObject
    {
        public static readonly DependencyProperty ConnectionStateProperty = DependencyProperty.Register(
          "ConnectionState", typeof(ConnectionState), typeof(StateView), new PropertyMetadata(ConnectionState.Idle));
        public ConnectionState ConnectionState
        {
            get { return (ConnectionState)GetValue(ConnectionStateProperty); }
            set { SetValue(ConnectionStateProperty, value); }
        }

        public static readonly DependencyProperty WorkTimeProperty = DependencyProperty.Register(
          "WorkTime", typeof(Time), typeof(StateView), new PropertyMetadata(new Time()));
        public Time WorkTime
        {
            get { return (Time)GetValue(WorkTimeProperty); }
            set { SetValue(WorkTimeProperty, value); }
        }

        public static readonly DependencyProperty WorkDistanceProperty = DependencyProperty.Register(
          "WorkDistance", typeof(Distance), typeof(StateView), new PropertyMetadata(new Distance()));
        public Distance WorkDistance
        {
            get { return (Distance)GetValue(WorkDistanceProperty); }
            set { SetValue(WorkDistanceProperty, value); }
        }

        public static readonly DependencyProperty WorkoutStateProperty = DependencyProperty.Register(
          "WorkoutState", typeof(WorkoutState), typeof(StateView), new PropertyMetadata(WorkoutState.Unknown));
        public WorkoutState WorkoutState
        {
            get { return (WorkoutState)GetValue(WorkoutStateProperty); }
            set { SetValue(WorkoutStateProperty, value); }
        }

        public static readonly DependencyProperty StrokeStateProperty = DependencyProperty.Register(
          "StrokeState", typeof(StrokeState), typeof(StateView), new PropertyMetadata(StrokeState.Unknown));
        public StrokeState StrokeState
        {
            get { return (StrokeState)GetValue(StrokeStateProperty); }
            set { SetValue(StrokeStateProperty, value); }
        }
    }
}
