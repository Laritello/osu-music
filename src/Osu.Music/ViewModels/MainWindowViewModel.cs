using Prism.Mvvm;

namespace Osu.Music.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "osu.Music";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
