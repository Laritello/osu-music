using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Osu.Music.Common.Models
{
    public class Playlist : BindableBase, IEditableObject
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
            var id = Beatmaps.Select(x => x.BeatmapSetId).ToList();
            var playlistBeatmaps = beatmaps.Where(x => id.Contains(x.BeatmapSetId)).ToList();

            for (int i = 0; i < Beatmaps.Count; i++)
                Beatmaps[i] = playlistBeatmaps.Where(x => x.BeatmapSetId == Beatmaps[i].BeatmapSetId).FirstOrDefault() ?? Beatmaps[i];
        }

        #region IEditableObject Implemantation
        private Playlist _backup;
        private bool inTxn;

        public void BeginEdit()
        {
            if (!inTxn)
            {
                _backup = DeepCopy();
                inTxn = true;
            }
        }

        public void CancelEdit()
        {
            if (inTxn)
            {
                Restrore();
                inTxn = false;
            }
        }

        public void EndEdit()
        {
            if (inTxn)
            {
                _backup = new Playlist();
                inTxn = false;
            }
        }

        private Playlist DeepCopy()
        {
            return new Playlist()
            {
                Name = Name,
                Cover = new PlaylistCover()
                {
                    Icon = Cover.Icon,
                    IconColor = Cover.IconColor,
                    BackgroundColor = Cover.BackgroundColor,
                },
            };
        }

        private void Restrore()
        {
            if (_backup != null)
            {
                Name = _backup.Name;
                Cover.Icon = _backup.Cover.Icon;
                Cover.IconColor = _backup.Cover.IconColor;
                Cover.BackgroundColor = _backup.Cover.BackgroundColor;
            }
        }
        #endregion
    }
}
