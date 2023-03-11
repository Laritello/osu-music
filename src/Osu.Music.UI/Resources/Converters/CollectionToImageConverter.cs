using Osu.Music.Common.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Osu.Music.UI.Resources.Converters
{
    public class CollectionToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            var collection = value as Collection;
            if (collection.Beatmaps.Count == 0) return null;
            return collection.Beatmaps[0].BackgroundFilePath == string.Empty
                ? null
                : collection.Beatmaps[0].BackgroundFilePath;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
