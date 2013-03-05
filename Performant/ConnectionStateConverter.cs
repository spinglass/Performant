using Monitor;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Performant
{
    [ValueConversion(typeof(ConnectionState), typeof(Brush))]
    public class ConnectionStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ConnectionState state = (ConnectionState)value;
            Color color = (Color)Application.Current.Resources["ColorIdle"];  // default for ConnectionState.Idle
            switch (state)
            {
                case ConnectionState.Connected:
                    color = (Color)Application.Current.Resources["ColorConnected"];
                    break;
                case ConnectionState.SendError:
                    color = (Color)Application.Current.Resources["ColorSendError"];
                    break;
                case ConnectionState.Lost:
                    color = (Color)Application.Current.Resources["ColorLost"];
                    break;
            }
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
