using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Osu.Music.UI.Resources.Converters
{
	public class ColorStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ColorConverter.ConvertFromString(value as string);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is Color))
				return "#AAAAAA";

			Color color = (Color)value;

			return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", color.A, color.R, color.G, color.B);
		}
	}
}
