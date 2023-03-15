using Osu.Music.Services.Localization;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Osu.Music.UI.Resources.Converters
{
    public class StringToLocalizationCulture : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return LocalizationFactory.GetCulture((string)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is LocalizationCulture))
                return null;

            return ((LocalizationCulture)value).Culture;
        }
    }
}
