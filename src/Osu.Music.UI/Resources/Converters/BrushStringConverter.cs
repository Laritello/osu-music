using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Osu.Music.UI.Resources.Converters
{
    public class BrushStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = (Color)ColorConverter.ConvertFromString(value as string);
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
