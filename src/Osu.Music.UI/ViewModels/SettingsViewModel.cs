using DryIoc;
using Osu.Music.Common.Enums;
using Osu.Music.Common.Models;
using Osu.Music.Services.Dialogs;
using Osu.Music.Services.Hotkeys;
using Osu.Music.Services.IO;
using Osu.Music.Services.Localization;
using Osu.Music.Services.Social;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation.Regions;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Osu.Music.UI.ViewModels
{
	public class SettingsViewModel : BindableBase, INavigationAware
	{
		private Settings _settings;
		public Settings Settings
		{
			get => _settings;
			set => SetProperty(ref _settings, value);
		}

		public IEnumerable<ApplicationTheme> Themes => Enum.GetValues(typeof(ApplicationTheme)).Cast<ApplicationTheme>();

		private LocalizationManager _localizationManager;
		public LocalizationManager LocalizationManager
		{
			get => _localizationManager;
			set => SetProperty(ref _localizationManager, value);
		}

		private HotkeyManager _hotkeyManager;
		public HotkeyManager HotkeyManager
		{
			get => _hotkeyManager;
			set => SetProperty(ref _hotkeyManager, value);
		}

		private DiscordManager _discordManager;
		public DiscordManager DiscordManager
		{
			get => _discordManager;
			set => SetProperty(ref _discordManager, value);
		}

		public DelegateCommand UpdateSourceCommand { get; private set; }
		public DelegateCommand UpdateDiscordCommand { get; private set; }

		private readonly IFileDialogService _fileDialogService;
		private readonly SettingsProvider _settingsManager;

		public SettingsViewModel(IContainer container)
		{
			_settingsManager = container.Resolve<SettingsProvider>();
			_fileDialogService = container.Resolve<IFileDialogService>();
			_localizationManager = LocalizationManager.Instance;

			InitializeCommands();
		}

		private void InitializeCommands()
		{
			UpdateSourceCommand = new DelegateCommand(UpdateSource);
			UpdateDiscordCommand = new DelegateCommand(UpdateDiscord);
		}

		private void UpdateSource()
		{
			var result = _fileDialogService.ShowOpenFolderDialog(out string path);

			if (result)
			{
				Settings.Source = path;
				_settingsManager.Save(Settings);
			}
		}

		private void UpdateDiscord()
		{
			_settingsManager.Save(Settings);
			DiscordManager.Enabled = Settings.DiscordEnabled;

			if (!DiscordManager.Enabled)
				DiscordManager.ClearPresence();
		}

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			Settings = navigationContext.Parameters.GetValue<Settings>("settings");
			DiscordManager = navigationContext.Parameters.GetValue<DiscordManager>("discord");
			HotkeyManager = navigationContext.Parameters.GetValue<HotkeyManager>("hotkey");
		}

		public bool IsNavigationTarget(NavigationContext navigationContext) => true;

		public void OnNavigatedFrom(NavigationContext navigationContext) { }
	}
}
