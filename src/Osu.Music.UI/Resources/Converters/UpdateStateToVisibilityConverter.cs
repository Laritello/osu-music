using Osu.Music.Common.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Osu.Music.UI.Resources.Converters
{
    public class UpdateStateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;

            UpdateState state = (UpdateState)value;

            return state switch
            {
                UpdateState.Available => Visibility.Visible,
                UpdateState.InProgress => Visibility.Collapsed,
                UpdateState.Latest => Visibility.Collapsed,
                _ => Visibility.Collapsed
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
