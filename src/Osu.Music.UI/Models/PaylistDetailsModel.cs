using Osu.Music.Common.Models;
using Prism.Mvvm;

namespace Osu.Music.UI.Models
{
    public class PaylistDetailsModel : BindableBase
    {
        private Playlist _playlist;
        public Playlist Playlist
        {
            get => _playlist;
            set => SetProperty(ref _playlist, value);
        }
    }
}
