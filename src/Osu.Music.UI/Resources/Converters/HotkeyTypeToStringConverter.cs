using Osu.Music.Common.Enums;
using Osu.Music.Services.UItility;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Osu.Music.UI.Resources.Converters
{
    public class HotkeyTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is HotkeyType))
                return "Unknown hotkey";

            HotkeyType type = (HotkeyType)value;
            var localization = Application.Current.Resources.GetLocalizationDictionary();

            return type switch
            {
                HotkeyType.PlayPause => localization["Strings.SettingsView.Keybinds.PlayPause"],
                HotkeyType.NextTrack => localization["Strings.SettingsView.Keybinds.NextTrack"],
                HotkeyType.PreviousTrack => localization["Strings.SettingsView.Keybinds.PreviousTrack"],
                HotkeyType.Mute => localization["Strings.SettingsView.Keybinds.Mute"],
                HotkeyType.Repeat => localization["Strings.SettingsView.Keybinds.Repeat"],
                HotkeyType.Shuffle => localization["Strings.SettingsView.Keybinds.Shuffle"],
                HotkeyType.VolumeUp => localization["Strings.SettingsView.Keybinds.VolumeUp"],
                HotkeyType.VolumeDown => localization["Strings.SettingsView.Keybinds.VolumeDown"],
                _ => localization["Strings.SettingsView.Keybinds.Unknown"],
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
