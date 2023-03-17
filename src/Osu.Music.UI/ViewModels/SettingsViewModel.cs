using DryIoc;
using Osu.Music.Common.Models;
using Osu.Music.Services.Dialog;
using Osu.Music.Services.Hotkeys;
using Osu.Music.Services.IO;
using Osu.Music.Services.Localization;
using Osu.Music.UI.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Osu.Music.UI.ViewModels
{
    public class SettingsViewModel : BindableBase, INavigationAware
    {
        private SettingsModel _model;
        public SettingsModel Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

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

        private IFileDialogService _fileDialogService;
        private SettingsManager _settingsManager;

        public SettingsViewModel(IContainer container, SettingsModel model) 
        {
            _settingsManager = container.Resolve<SettingsManager>();
            _fileDialogService = container.Resolve<IFileDialogService>();
            _localizationManager = container.Resolve<LocalizationManager>();

            _model = model;

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
                Model.Settings.Source = path;
                _settingsManager.Save(Model.Settings);
            }
        }

        private void UpdateDiscord()
        {
            _settingsManager.Save(Model.Settings);
            DiscordManager.Enabled = Model.Settings.DiscordEnabled;

            if (!DiscordManager.Enabled)
                DiscordManager.ClearPresence();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Model.Settings = navigationContext.Parameters.GetValue<Settings>("settings");
            DiscordManager = navigationContext.Parameters.GetValue<DiscordManager>("discord");
            HotkeyManager = navigationContext.Parameters.GetValue<HotkeyManager>("hotkey");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }
    }
}
