using Osu.Music.Services.Updates;
using Prism.Commands;
using Prism.Mvvm;

namespace Osu.Music.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private GitHubUpdater _updater;
        public GitHubUpdater Updater
        {
            get => _updater;
            set => SetProperty(ref _updater, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public DelegateCommand UpdateAppCommand { get; private set; }

        public MainWindowViewModel()
        {
            Title = "osu.Music";
            UpdateAppCommand = new DelegateCommand(UpdateApp);
        }

        private void UpdateApp()
        {
            Updater.Update();
        }
    }
}
