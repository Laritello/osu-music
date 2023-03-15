using Osu.Music.Common.Enums;
using Osu.Music.Common.Structures;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Osu.Music.Common.Models
{
    public class Settings : BindableBase
    {
        #region Properties
        private string _source;
        public string Source
        {
            get => _source;
            set
            {
                SetProperty(ref _source, value);
                SourceChanged?.Invoke(_source);
            }
        }

        private string _culture;
        public string Culture
        {
            get => _culture;
            set
            {
                SetProperty(ref _culture, value);
                CultureChanged?.Invoke(_culture);
            }
        }

        private string _color;
        public string Color
        {
            get => _color;
            set
            {
                SetProperty(ref _color, value);
                ColorChanged?.Invoke(_color);
            }
        }

        private bool _hotkeysEnabled;
        public bool HotkeysEnabled
        {
            get => _hotkeysEnabled;
            set => SetProperty(ref _hotkeysEnabled, value);
        }

        private ICollection<Hotkey> _hotkeys;
        public ICollection<Hotkey> Hotkeys
        {
            get => _hotkeys;
            set => SetProperty(ref _hotkeys, value);
        }

        private bool _discordEnabled;
        public bool DiscordEnabled
        {
            get => _discordEnabled;
            set
            {
                SetProperty(ref _discordEnabled, value);
                DiscordEnabledChanged?.Invoke(_discordEnabled);
            }
        }

        private PlayerState _state;
        public PlayerState State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }
        #endregion

        #region Events
        public delegate void ApplicationSourceChangedEventHander(string path);
        public event ApplicationSourceChangedEventHander SourceChanged;

        public delegate void ApplicationCultureChangedEventHandler(string culture);
        public event ApplicationCultureChangedEventHandler CultureChanged;

        public delegate void ApplicationColorChangedEventHandler(string color);
        public event ApplicationColorChangedEventHandler ColorChanged;

        public delegate void ApplicationDiscordEnabledEventHandler(bool enabled);
        public event ApplicationDiscordEnabledEventHandler DiscordEnabledChanged;
        #endregion

        public Settings()
        {
            Culture = "en-US";
            Color = "#FF800080";
            HotkeysEnabled = true;
            DiscordEnabled = true;
            State = new PlayerState()
            {
                Volume = 0.3f,
                Shuffle = false,
                Repeat = false,
                IsPlaying = false
            };

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
