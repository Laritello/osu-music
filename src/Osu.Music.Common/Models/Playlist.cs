using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Osu.Music.Common.Models
{
    public class Playlist : BindableBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private ObservableCollection<Beatmap> _beatmaps;
        public ObservableCollection<Beatmap> Beatmaps
        {
            get => _beatmaps;
            set => SetProperty(ref _beatmaps, value);
        }

        private PlaylistCover _cover;
        public PlaylistCover Cover
        {
            get => _cover;
            set => SetProperty(ref _cover, value);
        }

        public Playlist()
        {
            Beatmaps = new ObservableCollection<Beatmap>();
            Cover = new PlaylistCover();
        }

        public void UpdateMaps(ICollection<Beatmap> beatmaps)
        {
            var id = Beatmaps.Select(x => x.BeatmapSetID).ToList();
            var playlistBeatmaps = beatmaps.Where(x => id.Contains(x.BeatmapSetID)).ToList();

            for (int i = 0; i < Beatmaps.Count; i++)
                Beatmaps[i] = playlistBeatmaps.Where(x => x.BeatmapSetID == Beatmaps[i].BeatmapSetID).FirstOrDefault() ?? Beatmaps[i];
        }
    }
}
