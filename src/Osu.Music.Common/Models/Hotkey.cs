using Osu.Music.Common.Enums;
using Prism.Mvvm;

namespace Osu.Music.Common.Models
{
    public class Hotkey : BindableBase
    {
        private HotkeyType _type;
        public HotkeyType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        private KeyCombination _combination;
        public KeyCombination Combination
        {
            get => _combination;
            set => SetProperty(ref _combination, value);
        }
    }
}
