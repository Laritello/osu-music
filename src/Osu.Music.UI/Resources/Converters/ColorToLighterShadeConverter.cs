using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Osu.Music.UI.Resources.Converters
{
    public class ColorToLighterShadeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Color))
                return Colors.White;

            Color color = (Color)value;

            // Test version. There is probably the better way to extract darker color
            return new Color()
            {
                R = (byte)Math.Min(Math.Floor(1.25 * color.R), 128),
                G = (byte)Math.Min(Math.Floor(1.25 * color.G), 128),
                B = (byte)Math.Min(Math.Floor(1.25 * color.B), 128)
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
