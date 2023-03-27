using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Osu.Music.UI.Resources.Converters
{
    public class ColorWithOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Color color)) return null;
            var opacity = int.Parse((string)parameter, NumberStyles.HexNumber);

            return new SolidColorBrush(Color.FromArgb((byte)opacity, color.R, color.G, color.B));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
