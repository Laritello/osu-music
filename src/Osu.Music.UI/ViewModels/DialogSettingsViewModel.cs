using DryIoc;
using Osu.Music.Common.Models;
using Osu.Music.Services.Dialog;
using Osu.Music.Services.Hotkeys;
using Osu.Music.Services.IO;
using Osu.Music.Services.UItility;
using Osu.Music.UI.Resources.Converters;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Osu.Music.UI.ViewModels
{
    public class DialogSettingsViewModel : BindableBase, IDialogAware
    {
        #region Properties
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _color;
        public string Color
        {
            get => _color;
            set
            {
                SetProperty(ref _color, value);
                UpdateColor((Color)_colorConverter.Convert(value, typeof(Color), null, null));
            }
        }

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
        #endregion

        #region Commands
        public DelegateCommand<Color?> UpdateColorCommand { get; private set; }
        public DelegateCommand UpdateDiscordRpcCommand { get; private set; }
        public DelegateCommand UpdateOsuFolderCommand { get; private set; }
        public DelegateCommand ConfirmCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }
        #endregion

        #region Variables
        private IValueConverter _colorConverter;
        private IFileDialogService _fileDialogService;
        #endregion

        #region Events
        public event Action<IDialogResult> RequestClose;
        #endregion

        private SettingsManager _settingsManager;

        public DialogSettingsViewModel(IContainer container)
        {
            _settingsManager = container.Resolve<SettingsManager>();
            
            _colorConverter = new ColorStringConverter();
            _fileDialogService = new FileDialogService();

            InitializeColor();
            InitializeCommands();
        }

        private void InitializeColor()
        {
            ResourceDictionary resource = Application.Current.Resources;
            Color = resource.MergedDictionaries.GetMainColor().ToHex();
        }

        private void InitializeCommands()
        {
            UpdateColorCommand = new DelegateCommand<Color?>(UpdateColor);
            UpdateDiscordRpcCommand = new DelegateCommand(UpdateDiscordRpc);
            UpdateOsuFolderCommand = new DelegateCommand(UpdateOsuFolder);
            ConfirmCommand = new DelegateCommand(Confirm);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void UpdateColor(Color? color)
        {
            if (!color.HasValue || Settings == null)
                return;

            Settings.Color = color.Value.ToHex();
            ResourceDictionary resource = Application.Current.Resources;
            resource.MergedDictionaries.SetMainColor(Settings.Color);

            _settingsManager.Save(Settings);
        }

        private void UpdateDiscordRpc()
        {
            _settingsManager.Save(Settings);
            DiscordManager.Enabled = Settings.DiscordEnabled;

            if (!DiscordManager.Enabled)
                DiscordManager.ClearPresence();
        }

        private void UpdateOsuFolder()
        {
            var result = _fileDialogService.ShowOpenFolderDialog(out string path);

            if (result)
            {
                Settings.Source = path;
                _settingsManager.Save(Settings);
            }
        }

        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        private void Confirm()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }

        #region IDialogAware Implementation
        public void OnDialogClosed()
        {
            if (Settings != null)
                _settingsManager.Save(Settings);
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Settings = parameters.GetValue<Settings>("settings");
            HotkeyManager = parameters.GetValue<HotkeyManager>("hotkey");
            DiscordManager = parameters.GetValue<DiscordManager>("discord");

            _color = Settings.Color;
        }

        public bool CanCloseDialog() => true;
        #endregion
    }
}
