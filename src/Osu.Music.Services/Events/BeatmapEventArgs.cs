using Osu.Music.Common.Models;
using System;

namespace Osu.Music.Services.Events
{
    public class BeatmapEventArgs : EventArgs
    {
        public BeatmapEventArgs(Beatmap beatmap)
        {
            Beatmap = beatmap;
        }
        public Beatmap Beatmap { get; private set; }
    }
}
