using Osu.Music.Common.Enums;

namespace Osu.Music.Common.Models
{
    public class Hotkey
    {
        public HotkeyType Type { get; set; }
        public KeyCombination Combination { get; set; }
    }
}
