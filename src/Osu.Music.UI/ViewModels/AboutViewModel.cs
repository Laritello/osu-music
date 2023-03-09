using Osu.Music.Services.UItility;
using Osu.Music.UI.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Diagnostics;
using System.Reflection;
using Osu.Music.Services.Dialog;
using DryIoc;
using System.Collections.ObjectModel;
using Osu.Music.Common.Models;
using System.Linq;

namespace Osu.Music.UI.ViewModels
{
    public class AboutViewModel : BindableBase, INavigationAware
    {
        private AboutModel _model;
        public AboutModel Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        private ObservableCollection<LicenseNotice> _licenseContent;
        public ObservableCollection<LicenseNotice> LicenseContent
        {
            get => _licenseContent;
            set => SetProperty(ref _licenseContent, value);
        }

        public DelegateCommand<string> OpenRepositoryCommand { get; private set; }
        public DelegateCommand OpenReleaseNotesCommand { get; private set; }
        public DelegateCommand OpenLicenseCommand { get; private set; }
        public DelegateCommand OpenNoticesCommand { get; private set; }

        public AboutViewModel(AboutModel model) 
        {
            _model = model;

            InitializeCommands();
            Load();
        }

        private void InitializeCommands()
        {
            OpenRepositoryCommand = new DelegateCommand<string>(OpenRepository);
            OpenReleaseNotesCommand = new DelegateCommand(OpenReleaseNotes);
            OpenLicenseCommand = new DelegateCommand(OpenLicense);
            OpenNoticesCommand = new DelegateCommand(OpenNotices);
        }

        private void Load()
        {
            Model.Version = ReadVersion();
            Model.Licenses = AppDataHelper.GetLicenses();
            LicenseContent = new ObservableCollection<LicenseNotice>();
        }

        private void OpenRepository(string url)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }

        private void OpenReleaseNotes()
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {$"https://github.com/Laritello/osu-music/releases/tag/{Model.Version}"}") { CreateNoWindow = true });
        }

        private void OpenLicense()
        {
            if (Model.Licenses?.Count > 0)
            {
                LicenseContent.Clear();
                LicenseContent.Add(Model.Licenses.First());
            }
        }

        private void OpenNotices()
        {
            if (Model.Licenses?.Count > 0)
            {
                LicenseContent.Clear();
                LicenseContent.AddRange(Model.Licenses.Skip(1).ToList());
            }
        }

        private string ReadVersion()
        {
            var version = Assembly.GetEntryAssembly().GetName().Version;
            return $"{version.Major}.{version.Minor}.{version.Build}";
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }
    }
}
