using Osu.Music.Common.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Osu.Music.UI.Resources.Converters
{
    public class PlaylistCountToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ICollection<Beatmap>))
                return "Unknown";

            var count = ((ICollection<Beatmap>)value).Count;

            return $"{(count == 0 ? "No" : count.ToString())} song{(count != 1 ? "s" : "")}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
