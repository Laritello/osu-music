using Osu.Music.Common.Models;
using System.Collections.Generic;

namespace Osu.Music.UI.Parameters
{
    public class PlayBeatmapAndUpdateCollectionCommandParameters
    {
        public Beatmap Beatmap { get; set; }
        public IList<Beatmap> Collection { get; set; }
    }
}
