using Osu.Music.Common.Enums;
using Osu.Music.Common.Models;
using Osu.Music.Services.Audio;
using Osu.Music.Services.Events;
using Osu.Music.Services.IO;
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

        private HotkeyType? _selectedHotkey;
        public HotkeyType? SelectedHotkey
        {
            get => _selectedHotkey;
            set => SetProperty(ref _selectedHotkey, value);
        }

        public DelegateCommand<HotkeyType?> ToggleEditedKeyCommand { get; set; }

        private KeyboardStateManager Keyboard { get; set; }
        private GlobalKeyboardHook Hook { get; set; }

        public ICollection<Hotkey> Hotkeys { get; set; }

        private readonly AudioPlayback _playback;
        private readonly Settings _settings;

        public HotkeyManager(AudioPlayback playback, SettingsManager settingsManager)
        {
            _playback = playback;
            _settings = settingsManager.Settings;

            Hotkeys = _settings.Hotkeys;

            Keyboard = new KeyboardStateManager();
            ToggleEditedKeyCommand = new DelegateCommand<HotkeyType?>(ToggleEditedKey);

            Hook = new GlobalKeyboardHook();
            Hook.KeyboardPressed += Hook_KeyboardPressed;
        }

        private void ToggleEditedKey(HotkeyType? hotkey)
        {
            SelectedHotkey = hotkey;
        }

        private void Hook_KeyboardPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            bool isModifier = UpdateModifierKeys(e);

            if (!isModifier && e.KeyboardState == KeyboardState.KeyDown)
            {
                if (SelectedHotkey == null)
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
            var hotkey = Hotkeys.Where(x => x.Type == SelectedHotkey).FirstOrDefault();

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
            SelectedHotkey = null;
        }

        private void TriggerEvent(HotkeyType type)
        {
            if (!_settings.HotkeysEnabled)
                return;

            switch (type)
            {
                case HotkeyType.PlayPause:
                    PlayPauseHotkeyHandler();
                    break;
                case HotkeyType.PreviousTrack:
                    PreviousBeatmapHotkeyHandler();
                    break;
                case HotkeyType.NextTrack:
                    NextBeatmapHotkeyHandler();
                    break;
                case HotkeyType.Repeat:
                    RepeatHotkeyHandler();
                    break;
                case HotkeyType.Mute:
                    MuteHotkeyHandler();
                    break;
                case HotkeyType.Shuffle:
                    ShuffleHotkeyHandler();
                    break;
                case HotkeyType.VolumeDown:
                    VolumeDownHotkeyHandler();
                    break;
                case HotkeyType.VolumeUp:
                    VolumeUpHotkeyHandler();
                    break;
            }
        }

        #region Handlers
        private void PlayPauseHotkeyHandler()
        {
            if (_playback.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                _playback.Pause();
            else
                _playback.Resume();
        }

        private void PreviousBeatmapHotkeyHandler() => _playback.Previous();

        private void NextBeatmapHotkeyHandler() => _playback.Next();

        private void RepeatHotkeyHandler() => _playback.Repeat = !_playback.Repeat;

        private void MuteHotkeyHandler() => _playback.Mute = !_playback.Mute;

        private void ShuffleHotkeyHandler() => _playback.Shuffle = !_playback.Shuffle;

        private void VolumeUpHotkeyHandler() => _playback.Volume += 0.05f;

        private void VolumeDownHotkeyHandler() => _playback.Volume -= 0.05f;
        #endregion

        public void Dispose()
        {
            Hook?.Dispose();
        }
    }
}
