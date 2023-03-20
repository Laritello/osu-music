using Osu.Music.Common.Enums;
using Osu.Music.Services.Localization;
using System;
using System.Globalization;
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
            var localization = LocalizationManager.Instance;

            return type switch
            {
                HotkeyType.PlayPause => localization.GetLocalizedString("Strings.SettingsView.Keybinds.PlayPause"),
                HotkeyType.NextTrack => localization.GetLocalizedString("Strings.SettingsView.Keybinds.NextTrack"),
                HotkeyType.PreviousTrack => localization.GetLocalizedString("Strings.SettingsView.Keybinds.PreviousTrack"),
                HotkeyType.Mute => localization.GetLocalizedString("Strings.SettingsView.Keybinds.Mute"),
                HotkeyType.Repeat => localization.GetLocalizedString("Strings.SettingsView.Keybinds.Repeat"),
                HotkeyType.Shuffle => localization.GetLocalizedString("Strings.SettingsView.Keybinds.Shuffle"),
                HotkeyType.VolumeUp => localization.GetLocalizedString("Strings.SettingsView.Keybinds.VolumeUp"),
                HotkeyType.VolumeDown => localization.GetLocalizedString("Strings.SettingsView.Keybinds.VolumeDown"),
                _ => localization.GetLocalizedString("Strings.SettingsView.Keybinds.Unknown"),
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
