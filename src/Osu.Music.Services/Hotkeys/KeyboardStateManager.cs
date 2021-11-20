using Osu.Music.Common.Models;
using System.Windows.Forms;

namespace Osu.Music.Services.Hotkeys
{
    public class KeyboardStateManager
    {
        private bool _leftShiftPressed;
        public bool LeftShiftPressed
        {
            get => _leftShiftPressed;
            set
            {
                _leftShiftPressed = value;
                UpdateCombination();
            }
        }

        private bool _rightShiftPressed;
        public bool RightShiftPressed
        {
            get => _rightShiftPressed;
            set
            {
                _rightShiftPressed = value;
                UpdateCombination();
            }
        }

        private bool _leftAltPressed;
        public bool LeftAltPressed
        {
            get => _leftAltPressed;
            set
            {
                _leftAltPressed = value;
                UpdateCombination();
            }
        }

        private bool _rightAltPressed;
        public bool RightAltPressed
        {
            get => _rightAltPressed;
            set
            {
                _rightAltPressed = value;
                UpdateCombination();
            }
        }

        private bool _leftControlPressed;
        public bool LeftControlPressed
        {
            get => _leftControlPressed;
            set
            {
                _leftControlPressed = value;
                UpdateCombination();
            }
        }

        private bool _rightControlPressed;
        public bool RightControlPressed
        {
            get => _rightControlPressed;
            set
            {
                _rightControlPressed = value;
                UpdateCombination();
            }
        }

        private Keys _key;
        public Keys Key
        {
            get => _key;
            set
            {
                _key = value;
                UpdateCombination();
            }
        }

        public bool ShiftPressed { get => LeftShiftPressed || RightShiftPressed; }
        public bool AltPressed { get => LeftAltPressed || RightAltPressed; }
        public bool ControlPressed { get => LeftControlPressed || RightControlPressed; }

        public KeyCombination Combination { get; set; }

        public KeyboardStateManager()
        {
            Combination = new KeyCombination();
        }

        private void UpdateCombination()
        {
            Combination.AltPressed = AltPressed;
            Combination.ShiftPressed = ShiftPressed;
            Combination.ControlPressed = ControlPressed;
            Combination.Key = Key;
        }
    }
}
