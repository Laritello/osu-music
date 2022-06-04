using System;
using System.Globalization;
using System.Windows.Data;

namespace Osu.Music.UI.Resources.Converters
{
    public class ProgressToSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value[0] is double) || !(value[1] is double))
                return 0;

            double progress = Math.Min(Math.Max((double)value[1], 0), 1);
            double max = (double)value[0];

            return max * progress;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
