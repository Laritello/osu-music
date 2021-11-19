using Osu.Music.Services.Hotkeys;

namespace Osu.Music.Services.Events
{
    public class HotkeyEventArgs
    {
        public HotkeyType Type { get; }
        public HotkeyEventArgs(HotkeyType type)
        {
            Type = type;
        }
    }
}
