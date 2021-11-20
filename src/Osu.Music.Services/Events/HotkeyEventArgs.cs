using Osu.Music.Common.Enums;

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
