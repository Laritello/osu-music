using Osu.Music.Services.UItility;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Osu.Music.UI.ViewModels
{
    public class AboutViewModel : BindableBase, IDialogAware
    {
        private dynamic _licenses;
        public dynamic Licenses
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

        public event Action<IDialogResult> RequestClose;
        public DelegateCommand<dynamic> OpenRepositoryCommand { get; private set; }

        public string Title => throw new NotImplementedException();

        public AboutViewModel()
        {
            Version = ReadVersion();
            Licenses = AppDataHelper.GetLicenses();
            OpenRepositoryCommand = new DelegateCommand<dynamic>(OpenRepository);
        }

        private string ReadVersion()
        {
            var version = Assembly.GetEntryAssembly().GetName().Version;
            return $"{version.Major}.{version.Minor}.{version.Build}";
        }

        private void OpenRepository(dynamic url)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {(string)url}") { CreateNoWindow = true });
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters) { }
    }
}
