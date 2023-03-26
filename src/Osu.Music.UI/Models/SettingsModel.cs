using Osu.Music.Common.Enums;
using Osu.Music.Common.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private IEnumerable<ApplicationTheme> _themes;
        public IEnumerable<ApplicationTheme> Themes
        {
            get => _themes;
        }

        public SettingsModel() 
        {
            _themes = Enum.GetValues(typeof(ApplicationTheme)).Cast<ApplicationTheme>();
        }
    }
}
