using Prism.Mvvm;

namespace Osu.Music.Common.Models
{
    public class Settings : BindableBase
    {
        private string _osuFolder;
        public string OsuFolder
        {
            get => _osuFolder;
            set => SetProperty(ref _osuFolder, value);
        }
    }
}
