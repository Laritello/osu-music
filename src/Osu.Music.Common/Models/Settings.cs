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

        private string _mainColor;
        public string MainColor
        {
            get => _mainColor;
            set => SetProperty(ref _mainColor, value);
        }

        public Settings()
        {
            MainColor = "#800080";
        }
    }
}
