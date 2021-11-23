﻿using Osu.Music.Common.Enums;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Osu.Music.Common.Models
{
    public class Settings : BindableBase
    {
        private string _osuFolder;
        public string OsuFolder
        {
            get => _osuFolder;
            set => SetProperty(ref _osuFolder, value);
        }

        private string _mainColor;
        public string MainColor
        {
            get => _mainColor;
            set => SetProperty(ref _mainColor, value);
        }

        private ICollection<Hotkey> _hotkeys;
        public ICollection<Hotkey> Hotkeys
        {
            get => _hotkeys;
            set => SetProperty(ref _hotkeys, value);
        }

        public Settings()
        {
            MainColor = "#800080";
            InitializeHotkeys();
        }

        private void InitializeHotkeys()
        {
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
                        ControlPressed = true,
                        ShiftPressed = true,
                        Key = Keys.Left
                    }
                },

                new Hotkey()
                {
                    Type = HotkeyType.NextTrack,
                    Combination = new KeyCombination()
                    {
                        ControlPressed = true,
                        ShiftPressed = true,
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
        }
    }
}
