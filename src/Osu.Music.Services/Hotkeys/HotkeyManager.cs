using Osu.Music.Common.Enums;
using Osu.Music.Common.Models;
using Osu.Music.Services.Events;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Osu.Music.Services.Hotkeys
{
    public class HotkeyManager : BindableBase, IDisposable
    {
        public delegate void HotkeyEventHandler(object sender, HotkeyEventArgs e);
        public event HotkeyEventHandler HotkeyUsed;

        public delegate void HotkeyChangedEventHandler();
        public event HotkeyChangedEventHandler HotkeyChanged;

        private HotkeyType? _editedHotkey;
        public HotkeyType? EditedHotkey
        {
            get => _editedHotkey;
            set => SetProperty(ref _editedHotkey, value);
        }

        public DelegateCommand<HotkeyType?> SetEditedHotkeyCommand { get; set; }

        private KeyboardStateManager Keyboard { get; set; }
        private GlobalKeyboardHook Hook { get; set; }

        public ICollection<Hotkey> Hotkeys { get; set; }

        public HotkeyManager()
        {
            Keyboard = new KeyboardStateManager();
            Hotkeys = new List<Hotkey>();
            SetEditedHotkeyCommand = new DelegateCommand<HotkeyType?>(SetEditedCommand);

            Hook = new GlobalKeyboardHook();
            Hook.KeyboardPressed += Hook_KeyboardPressed;
        }

        private void SetEditedCommand(HotkeyType? hotkey)
        {
            EditedHotkey = EditedHotkey == hotkey ? null : hotkey;
        }

        private void Hook_KeyboardPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            bool isModifier = UpdateModifierKeys(e);

            if (!isModifier && e.KeyboardState == KeyboardState.KeyDown)
            {
                if (EditedHotkey == null)
                    CheckHotkeys(e);
                else
                    SaveHotkey(e);
            }
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

            foreach (Hotkey hotkey in Hotkeys.Where(x => x.Combination != null))
            {
                if (hotkey.Combination.Equals(currentCombination))
                    TriggerEvent(hotkey.Type);
            }
        }

        private void SaveHotkey(GlobalKeyboardHookEventArgs e)
        {
            var hotkey = Hotkeys.Where(x => x.Type == EditedHotkey).FirstOrDefault();

            Keyboard.Key = e.KeyboardData.Key;
            var currentCombination = Keyboard.Combination;

            if (hotkey != null)
            {
                foreach (Hotkey hk in Hotkeys.Where(x => x.Combination != null))
                {
                    if (hk.Combination.Equals(currentCombination))
                        hk.Combination = null;
                }

                hotkey.Combination = new KeyCombination()
                {
                    AltPressed = currentCombination.AltPressed,
                    ControlPressed = currentCombination.ControlPressed,
                    ShiftPressed = currentCombination.ShiftPressed,
                    Key = currentCombination.Key
                };
            }
            HotkeyChanged?.Invoke();
            EditedHotkey = null;
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
