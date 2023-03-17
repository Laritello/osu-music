using Osu.Music.Common.Models;
using Osu.Music.Services.Hotkeys;
using Osu.Music.Services.IO;
using Osu.Music.Services.Localization;
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

        public SettingsModel() { }
    }
}
