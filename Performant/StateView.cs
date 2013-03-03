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
        public static readonly DependencyProperty WorkTimeProperty = DependencyProperty.Register(
          "WorkTime", typeof(uint), typeof(StateView), new PropertyMetadata(0u));
        public uint WorkTime
        {
            get { return (uint)GetValue(WorkTimeProperty); }
            set { SetValue(WorkTimeProperty, value); }
        }

        public static readonly DependencyProperty WorkDistanceProperty = DependencyProperty.Register(
          "WorkDistance", typeof(uint), typeof(StateView), new PropertyMetadata(0u));
        public uint WorkDistance
        {
            get { return (uint)GetValue(WorkDistanceProperty); }
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
