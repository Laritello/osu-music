using MaterialDesignThemes.Wpf;
using Prism.Mvvm;

namespace Osu.Music.Common.Models
{
    public class PlaylistCover : BindableBase
    {
        private PackIconKind? _icon;
        public PackIconKind? Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        private string _iconColor;
        public string IconColor
        {
            get => _iconColor;
            set => SetProperty(ref _iconColor, value);
        }

        private string _backgroundColor;
        public string BackgroundColor
        {
            get => _backgroundColor;
            set => SetProperty(ref _backgroundColor, value);
        }
    }
}
