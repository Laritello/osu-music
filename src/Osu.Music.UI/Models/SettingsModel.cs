using Osu.Music.Common.Models;
using Osu.Music.Services.Hotkeys;
using Osu.Music.Services.IO;
using Prism.Mvvm;

namespace Osu.Music.UI.Models
{
    public class SettingsModel : BindableBase
    {
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

        public SettingsModel() { }
    }
}
