using Osu.Music.Common.Enums;
using Osu.Music.Common.Structures;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Osu.Music.Common.Models
{
	public class Settings : BindableBase
	{
		#region Backing fields

		private string _source;
		private string _culture;
		private ApplicationTheme _theme;
		private string _color;
		private bool _hotkeysEnabled;
		private ICollection<Hotkey> _hotkeys = [];
		private bool _discordEnabled;
		private PlayerState _state;

		#endregion

		#region Properties

		/// <summary>
		/// Path to the osu! instance.
		/// </summary>
		public string Source
		{
			get => _source;
			set
			{
				SetProperty(ref _source, value);
				SourceChanged?.Invoke(_source);
			}
		}

		/// <summary>
		/// Culture of the application.
		/// </summary>
		public string Culture
		{
			get => _culture;
			set
			{
				SetProperty(ref _culture, value);
				CultureChanged?.Invoke(_culture);
			}
		}

		/// <summary>
		/// Theme of the application.
		/// </summary>
		public ApplicationTheme Theme
		{
			get => _theme;
			set
			{
				SetProperty(ref _theme, value);
				ThemeChanged?.Invoke(_theme);
			}
		}

		/// <summary>
		/// Primary color of the application.
		/// </summary>
		public string Color
		{
			get => _color;
			set
			{
				SetProperty(ref _color, value);
				ColorChanged?.Invoke(_color);
			}
		}

		/// <summary>
		/// Toggle for the hotkeys.
		/// </summary>
		public bool HotkeysEnabled
		{
			get => _hotkeysEnabled;
			set => SetProperty(ref _hotkeysEnabled, value);
		}

		/// <summary>
		/// List of the hotkeys.
		/// </summary>
		public ICollection<Hotkey> Hotkeys
		{
			get => _hotkeys;
			set => SetProperty(ref _hotkeys, value);
		}

		/// <summary>
		/// Toggle for the Discord RPC.
		/// </summary>
		public bool DiscordEnabled
		{
			get => _discordEnabled;
			set
			{
				SetProperty(ref _discordEnabled, value);
				DiscordEnabledChanged?.Invoke(_discordEnabled);
			}
		}

		/// <summary>
		/// Last active state of the player before exit.
		/// </summary>
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

		public delegate void ApplicationThemeChangedEventHandler(ApplicationTheme theme);
		public event ApplicationThemeChangedEventHandler ThemeChanged;

		public delegate void ApplicationColorChangedEventHandler(string color);
		public event ApplicationColorChangedEventHandler ColorChanged;

		public delegate void ApplicationDiscordEnabledEventHandler(bool enabled);
		public event ApplicationDiscordEnabledEventHandler DiscordEnabledChanged;

		#endregion

		#region Constructors

		public Settings()
		{
			Culture = "en-US";
			Theme = ApplicationTheme.Light;
			Color = "#FF800080";
			HotkeysEnabled = true;
			DiscordEnabled = true;
			State = new PlayerState
			{
				Volume = 0.3f,
				Shuffle = false,
				Repeat = false,
				IsPlaying = false
			};

			InitializeHotkeys();
		}

		#endregion

		#region Methods

		private void InitializeHotkeys()
		{
			Hotkeys =
			[
				new Hotkey
				{
					Type = HotkeyType.PlayPause,
					Combination = new KeyCombination
					{
						ControlPressed = true,
						ShiftPressed = true,
						Key = Keys.D
					}
				},

				new Hotkey
				{
					Type = HotkeyType.PreviousTrack,
					Combination = new KeyCombination
					{
						ControlPressed = true,
						ShiftPressed = true,
						Key = Keys.Left
					}
				},

				new Hotkey
				{
					Type = HotkeyType.NextTrack,
					Combination = new KeyCombination
					{
						ControlPressed = true,
						ShiftPressed = true,
						Key = Keys.Right
					}
				},

				new Hotkey
				{
					Type = HotkeyType.Mute,
					Combination = new KeyCombination
					{
						ControlPressed = true,
						Key = Keys.M
					}
				},

				new Hotkey
				{
					Type = HotkeyType.Shuffle,
					Combination = new KeyCombination
					{
						ControlPressed = true,
						Key = Keys.S
					}
				},

				new Hotkey
				{
					Type = HotkeyType.Repeat,
					Combination = new KeyCombination
					{
						ControlPressed = true,
						Key = Keys.R
					}
				},

				new Hotkey
				{
					Type = HotkeyType.VolumeUp,
					Combination = new KeyCombination
					{
						ControlPressed = true,
						Key = Keys.Up
					}
				},

				new Hotkey
				{
					Type = HotkeyType.VolumeDown,
					Combination = new KeyCombination
					{
						ControlPressed = true,
						Key = Keys.Down
					}
				}
			];
		}

		#endregion
	}
}
