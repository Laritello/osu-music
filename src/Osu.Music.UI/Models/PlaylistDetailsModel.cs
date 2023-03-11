using Osu.Music.Common.Models;
using Prism.Mvvm;

namespace Osu.Music.UI.Models
{
    public class PlaylistDetailsModel : BindableBase
    {
        private Playlist _playlist;
        public Playlist Playlist
        {
            get => _playlist;
            set => SetProperty(ref _playlist, value);
        }

        private Beatmap _selectedBeatmap;
        public Beatmap SelectedBeatmap
        {
            get => _selectedBeatmap;
            set => SetProperty(ref _selectedBeatmap, value);
        }

        public PlaylistDetailsModel() { }
    }
}
