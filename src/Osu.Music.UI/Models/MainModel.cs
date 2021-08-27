using Osu.Music.Common.Models;
using Prism.Mvvm;
using System.Collections.Generic;

namespace Osu.Music.UI.Models
{
    public class MainModel : BindableBase
    {
        #region Properties
        private IList<Beatmap> _beatmaps;
        public IList<Beatmap> Beatmaps
        {
            get => _beatmaps;
            set => SetProperty(ref _beatmaps, value);
        }

        private Beatmap _currentBeatmap;
        public Beatmap CurrentBeatmap
        {
            get => _currentBeatmap;
            set => SetProperty(ref _currentBeatmap, value);
        }

        private Stack<Beatmap> _previousBeatmaps;
        public Stack<Beatmap> PreviousBeatmaps
        {
            get => _previousBeatmaps;
            set => SetProperty(ref _previousBeatmaps, value);
        }

        private bool _repeat;
        public bool Repeat
        {
            get => _repeat;
            set => SetProperty(ref _repeat, value);
        }

        private bool _random;
        public bool Random
        {
            get => _random;
            set => SetProperty(ref _random, value);
        }

        private long _position;
        public long Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        private long _length;
        public long Length
        {
            get => _length;
            set => SetProperty(ref _length, value);
        }
        #endregion

        public MainModel()
        {
            PreviousBeatmaps = new Stack<Beatmap>(20);
        }
    }
}
