using Prism.Mvvm;
using Squirrel;
using System.Threading.Tasks;

namespace Osu.Music.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "osu.Music";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}
