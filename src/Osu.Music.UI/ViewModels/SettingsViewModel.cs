using Osu.Music.Common.Models;
using Osu.Music.Services.Hotkeys;
using Osu.Music.Services.IO;
using Osu.Music.Services.UItility;
using Osu.Music.UI.UserControls;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Windows;
using System.Windows.Media;

namespace Osu.Music.UI.ViewModels
{
    public class SettingsViewModel : BindableBase, IDialogAware
    {
        public string Title => "Settings";
        public bool CanCloseDialog() => true;

        private Settings _settings;
        public Settings Settings
        {
            get => _settings;
            set => SetProperty(ref _settings, value);
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

        public event Action<IDialogResult> RequestClose;
        public DelegateCommand<Color?> UpdateColorCommand { get; private set; }
        public DelegateCommand UpdateDiscordRpcCommand { get; private set; }

        public SettingsViewModel()
        {
            UpdateColorCommand = new DelegateCommand<Color?>(UpdateColor);
            UpdateDiscordRpcCommand = new DelegateCommand(UpdateDiscordRpc);
        }

        private void UpdateColor(Color? color)
        {
            if (!color.HasValue)
                return;

            Settings.MainColor = color.Value.ToHex();
            ResourceDictionary resource = Application.Current.Resources;
            resource.MergedDictionaries.SetMainColor(Settings.MainColor);

            SettingsManager.Save(Settings);
        }

        private void UpdateDiscordRpc()
        {
            SettingsManager.Save(Settings);
            DiscordManager.Enabled = Settings.DiscordRpcEnabled;

            if (!DiscordManager.Enabled)
                DiscordManager.ClearPresence();
        }

        public void OnDialogClosed() 
        {
            if (Settings != null)
                SettingsManager.Save(Settings);
        }

        public void OnDialogOpened(IDialogParameters parameters) 
        {
            Settings = parameters.GetValue<Settings>("settings");
            HotkeyManager = parameters.GetValue<HotkeyManager>("hotkey");
            DiscordManager = parameters.GetValue<DiscordManager>("discord");
        }
    }
}
