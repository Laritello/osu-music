using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Osu.Music.UI.Resources.Converters
{
    public class WindowStateToTrayVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is WindowState state))
                return Visibility.Collapsed;

            return state == WindowState.Minimized ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
