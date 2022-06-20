namespace Osu.Music.Common.Structures
{
    public struct PlayerState
    {
        public float Volume { get; set; }
        public bool Shuffle { get; set; }
        public bool Repeat { get; set; }

        public int? SelectedBeatmapId { get; set; }
        public bool IsPlaying { get; set; }
        public long Position { get; set; }
    }
}
