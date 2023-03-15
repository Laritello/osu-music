using Osu.Music.Services.UItility;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Osu.Music.UI.Resources.Converters
{
    public class CountToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int))
                return "Unknown";

            var count = (int) value;
            var localization = Application.Current.Resources.GetLocalizationDictionary();

            return count switch
            {
                0 => localization["Strings.DataTemplate.SongsCount.Empty"],
                1 => localization["Strings.DataTemplate.SongsCount.Single"],
                _ => string.Format((string)localization["Strings.DataTemplate.SongsCount.Multiple"], count)
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
