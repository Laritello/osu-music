using Osu.Music.Common.Enums;
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

            return type switch
            {
                HotkeyType.PlayPause => "Play/Pause",
                HotkeyType.NextTrack => "Next Beatmap",
                HotkeyType.PreviousTrack => "Previous Beatmap",
                HotkeyType.Mute => "Mute",
                HotkeyType.Repeat => "Repeat",
                HotkeyType.Shuffle => "Shuffle",
                HotkeyType.VolumeUp => "Volume Up",
                HotkeyType.VolumeDown => "Volume Down",
                _ => "Unknown hotkey",
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
