using System.Windows.Forms;

namespace Osu.Music.Services.Hotkeys
{
    public class KeyCombination
    {
        public bool ShiftPressed { get; set; }
        public bool AltPressed { get; set; }
        public bool ControlPressed { get; set; }
        public Keys Key { get; set; }

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
