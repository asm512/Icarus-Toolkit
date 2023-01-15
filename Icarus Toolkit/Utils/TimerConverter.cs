using Avalonia.Data.Converters;
using Avalonia.Threading;
using System;
using System.Globalization;

namespace Icarus_Toolkit.Utils
{
    public class TimerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timer = (DispatcherTimer)value;
            var text = parameter as string;
            if (timer.IsEnabled)
            {
                return text;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
