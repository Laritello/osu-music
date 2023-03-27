using Osu.Music.Common.Enums;
using Osu.Music.Services.UItility;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Osu.Music.UI.Resources.Converters
{
    public class ApplicationThemeToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ApplicationTheme theme))
                return "Unknown";

            var localization = Application.Current.Resources.GetLocalizationDictionary();

            return theme switch
            {
                ApplicationTheme.Light => localization["Strings.SettingsView.Appearance.Theme.Light"],
                ApplicationTheme.Dark => localization["Strings.SettingsView.Appearance.Theme.Dark"],
                _ => "Unknown"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
