﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Osu.Music.Common.Models
{
    public class Playlist
    {
        public string Name { get; set; }
        public ObservableCollection<Beatmap> Beatmaps { get; set; }

        public Playlist()
        {
            Beatmaps = new ObservableCollection<Beatmap>();
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
