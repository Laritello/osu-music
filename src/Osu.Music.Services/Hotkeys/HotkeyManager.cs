using Osu.Music.Services.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using static Osu.Music.Services.Hotkeys.GlobalKeyboardHook;

namespace Osu.Music.Services.Hotkeys
{
    public class HotkeyManager : IDisposable
    {
        public delegate void HotkeyEventHandler(object sender, HotkeyEventArgs e);
        public event HotkeyEventHandler HotkeyUsed;

        private KeyboardStateManager Keyboard { get; set; }
        private GlobalKeyboardHook Hook { get; set; }

        public ICollection<Hotkey> Hotkeys { get; set; }

        public HotkeyManager()
        {
            Keyboard = new KeyboardStateManager();
            Hotkeys = new List<Hotkey>
            {
                new Hotkey()
                {
                    Type = HotkeyType.PlayPause,
                    Combination = new KeyCombination()
                    {
                        ControlPressed = true,
                        ShiftPressed = true,
                        Key = Keys.D
                    }
                },

                new Hotkey()
                {
                    Type = HotkeyType.PreviousTrack,
                    Combination = new KeyCombination()
                    {
                        AltPressed = true,
                        Key = Keys.Left
                    }
                },

                new Hotkey()
                {
                    Type = HotkeyType.NextTrack,
                    Combination = new KeyCombination()
                    {
                        AltPressed = true,
                        Key = Keys.Right
                    }
                },

                new Hotkey()
                {
                    Type = HotkeyType.Mute,
                    Combination = new KeyCombination()
                    {
                        ControlPressed = true,
                        Key = Keys.M
                    }
                },

                new Hotkey()
                {
                    Type = HotkeyType.Shuffle,
                    Combination = new KeyCombination()
                    {
                        ControlPressed = true,
                        Key = Keys.S
                    }
                },

                new Hotkey()
                {
                    Type = HotkeyType.Repeat,
                    Combination = new KeyCombination()
                    {
                        ControlPressed = true,
                        Key = Keys.R
                    }
                },

                new Hotkey()
                {
                    Type = HotkeyType.VolumeUp,
                    Combination = new KeyCombination()
                    {
                        ControlPressed = true,
                        Key = Keys.Up
                    }
                },

                new Hotkey()
                {
                    Type = HotkeyType.VolumeDown,
                    Combination = new KeyCombination()
                    {
                        ControlPressed = true,
                        Key = Keys.Down
                    }
                }
            };

            Hook = new GlobalKeyboardHook();
            Hook.KeyboardPressed += Hook_KeyboardPressed;
        }

        private void Hook_KeyboardPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            bool isModifier = UpdateModifierKeys(e);

            if (!isModifier && e.KeyboardState == KeyboardState.KeyDown)
                CheckHotkeys(e);
        }

        private bool UpdateModifierKeys(GlobalKeyboardHookEventArgs e)
        {
            bool isModifier = false;

            switch (e.KeyboardData.Key)
            {
                case Keys.LShiftKey:
                    isModifier = true;
                    Keyboard.LeftShiftPressed = e.KeyboardState == KeyboardState.KeyDown || e.KeyboardState == KeyboardState.SysKeyDown;
                    break;
                case Keys.RShiftKey:
                    isModifier = true;
                    Keyboard.RightShiftPressed = e.KeyboardState == KeyboardState.KeyDown || e.KeyboardState == KeyboardState.SysKeyDown;
                    break;
                case Keys.LMenu:
                    isModifier = true;
                    Keyboard.LeftAltPressed = e.KeyboardState == KeyboardState.KeyDown || e.KeyboardState == KeyboardState.SysKeyDown;
                    break;
                case Keys.RMenu:
                    isModifier = true;
                    Keyboard.RightAltPressed = e.KeyboardState == KeyboardState.KeyDown || e.KeyboardState == KeyboardState.SysKeyDown;
                    break;
                case Keys.LControlKey:
                    isModifier = true;
                    Keyboard.LeftControlPressed = e.KeyboardState == KeyboardState.KeyDown || e.KeyboardState == KeyboardState.SysKeyDown;
                    break;
                case Keys.RControlKey:
                    isModifier = true;
                    Keyboard.RightControlPressed = e.KeyboardState == KeyboardState.KeyDown || e.KeyboardState == KeyboardState.SysKeyDown;
                    break;
            }

            return isModifier;
        }

        private void CheckHotkeys(GlobalKeyboardHookEventArgs e)
        {
            Keyboard.Key = e.KeyboardData.Key;

            var currentCombination = Keyboard.Combination;

            foreach (Hotkey hotkey in Hotkeys)
            {
                if (hotkey.Combination.Equals(currentCombination))
                    TriggerEvent(hotkey.Type);
            }
        }

        private void TriggerEvent(HotkeyType type)
        {
            HotkeyUsed?.Invoke(this, new HotkeyEventArgs(type));
        }

        public void Dispose()
        {
            Hook?.Dispose();
        }
    }
}
