using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Osu.Music.UI.Resources.Converters
{
    public class ColorToDarkerShadeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Color))
                return Colors.White;

            Color color = (Color)value;

            // Test version. There is probably the better way to extract darker color
            return new Color()
            {
                R = (byte)Math.Floor(0.75 * color.R),
                G = (byte)Math.Floor(0.75 * color.G),
                B = (byte)Math.Floor(0.75 * color.B)
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
