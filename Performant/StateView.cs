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

        // PM3 data

        public static readonly DependencyProperty DragFactorProperty = DependencyProperty.Register(
          "DragFactor", typeof(uint), typeof(StateView), new PropertyMetadata(0u));
        public uint DragFactor
        {
            get { return (uint)GetValue(DragFactorProperty); }
            set { SetValue(DragFactorProperty, value); }
        }

        public static readonly DependencyProperty StrokeStateProperty = DependencyProperty.Register(
          "StrokeState", typeof(StrokeState), typeof(StateView), new PropertyMetadata(StrokeState.Unknown));
        public StrokeState StrokeState
        {
            get { return (StrokeState)GetValue(StrokeStateProperty); }
            set { SetValue(StrokeStateProperty, value); }
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

        public static readonly DependencyProperty WorkoutTypeProperty = DependencyProperty.Register(
          "WorkoutType", typeof(WorkoutType), typeof(StateView), new PropertyMetadata(WorkoutType.Unknown));
        public WorkoutType WorkoutType
        {
            get { return (WorkoutType)GetValue(WorkoutTypeProperty); }
            set { SetValue(WorkoutTypeProperty, value); }
        }

        public static readonly DependencyProperty WorkTimeProperty = DependencyProperty.Register(
          "WorkTime", typeof(Time), typeof(StateView), new PropertyMetadata(new Time()));
        public Time WorkTime
        {
            get { return (Time)GetValue(WorkTimeProperty); }
            set { SetValue(WorkTimeProperty, value); }
        }

        // CSAFE data

        public static readonly DependencyProperty CaloriesProperty = DependencyProperty.Register(
          "Calories", typeof(uint), typeof(StateView), new PropertyMetadata(0u));
        public uint Calories
        {
            get { return (uint)GetValue(CaloriesProperty); }
            set { SetValue(CaloriesProperty, value); }
        }

        public static readonly DependencyProperty HeartRateProperty = DependencyProperty.Register(
          "HeartRate", typeof(uint), typeof(StateView), new PropertyMetadata(0u));
        public uint HeartRate
        {
            get { return (uint)GetValue(HeartRateProperty); }
            set { SetValue(HeartRateProperty, value); }
        }

        public static readonly DependencyProperty PowerProperty = DependencyProperty.Register(
          "Power", typeof(uint), typeof(StateView), new PropertyMetadata(0u));
        public uint Power
        {
            get { return (uint)GetValue(PowerProperty); }
            set { SetValue(PowerProperty, value); }
        }

        public static readonly DependencyProperty StrokePaceProperty = DependencyProperty.Register(
          "StrokePace", typeof(Time), typeof(StateView), new PropertyMetadata(new Time()));
        public Time StrokePace
        {
            get { return (Time)GetValue(StrokePaceProperty); }
            set { SetValue(StrokePaceProperty, value); }
        }

        public static readonly DependencyProperty StrokeRateProperty = DependencyProperty.Register(
          "StrokeRate", typeof(uint), typeof(StateView), new PropertyMetadata(0u));
        public uint StrokeRate
        {
            get { return (uint)GetValue(StrokeRateProperty); }
            set { SetValue(StrokeRateProperty, value); }
        }
    }
}
