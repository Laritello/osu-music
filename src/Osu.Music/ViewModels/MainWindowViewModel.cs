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

        private string _title = "osu.Music";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}
