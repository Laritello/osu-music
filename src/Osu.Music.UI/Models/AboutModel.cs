using Osu.Music.Common.Models;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Osu.Music.UI.Models
{
    public class AboutModel : BindableBase
    {
        private ObservableCollection<LicenseNotice> _licenses;
        public ObservableCollection<LicenseNotice> Licenses
        {
            get => _licenses;
            set => SetProperty(ref _licenses, value);
        }

        private string _version;
        public string Version
        {
            get => _version;
            set => SetProperty(ref _version, value);
        }

        public AboutModel() { }
    }
}
