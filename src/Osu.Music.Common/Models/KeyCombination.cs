using Prism.Mvvm;
using System.Windows.Forms; // Using this from NAudio dependency. Kinda weird?

namespace Osu.Music.Common.Models
{
    public class KeyCombination : BindableBase
    {
        private bool _shiftPressed;
        public bool ShiftPressed
        {
            get => _shiftPressed;
            set => SetProperty(ref _shiftPressed, value);
        }

        private bool _altPressed;
        public bool AltPressed
        {
            get => _altPressed;
            set => SetProperty(ref _altPressed, value);
        }

        private bool _controlPressed;
        public bool ControlPressed
        {
            get => _controlPressed;
            set => SetProperty(ref _controlPressed, value);
        }

        private Keys _key;
        public Keys Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
        }

        public override string ToString()
        {
            return $"{(ShiftPressed ? "Shift+" : "")}{(ControlPressed ? "Ctrl+" : "")}{(AltPressed ? "Alt+" : "")}{Key}";
        }
        public override bool Equals(object obj)
        {
            if (!(obj is KeyCombination))
                return false;

            KeyCombination comb = (KeyCombination)obj;

            return ShiftPressed == comb.ShiftPressed && AltPressed == comb.AltPressed && ControlPressed == comb.ControlPressed && Key == comb.Key;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
