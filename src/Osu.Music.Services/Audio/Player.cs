using NAudio.Wave;
using Osu.Music.Common.Models;
using Prism.Mvvm;
using System;

namespace Osu.Music.Services.Audio
{
    public class Player : BindableBase, IDisposable
    {
        private AudioPlayback _playback;
        public AudioPlayback Playback
        {
            get => _playback;
            set => SetProperty(ref _playback, value);
        }

        #region Properties
        private Beatmap _beatmap;
        public Beatmap Beatmap
        {
            get => _beatmap;
            set => SetProperty(ref _beatmap, value);
        }

        private bool _mute = false;
        public bool Mute
        {
            get => _mute;
            set
            {
                SetProperty(ref _mute, value);
                Playback.SetMute();
            }
        }

        private float _volume = 0.3f;
        public float Volume
        {
            get => _volume;
            set
            {
                SetProperty(ref _volume, Math.Min(Math.Max(value, 0), 1)); // Volume should be in range [0;1]
                Playback.SetVolume();
            }
        }

        private bool _repeat;
        public bool Repeat
        {
            get => _repeat;
            set => SetProperty(ref _repeat, value);
        }

        private bool _shuffle;
        public bool Shuffle
        {
            get => _shuffle;
            set => SetProperty(ref _shuffle, value);
        }

        public TimeSpan CurrentTime
        {
            get => Playback.Stream?.CurrentTime ?? TimeSpan.Zero;
            set
            {
                if (Playback.Stream != null)
                    Playback.Stream.CurrentTime = value;
            }
        }

        public TimeSpan TotalTime => Playback.Stream?.TotalTime ?? TimeSpan.Zero;

        public long Position
        {
            get => Playback.Stream?.Position ?? 0;
            set
            {
                if (Playback.Stream != null)
                    Playback.Stream.Position = value;
            }
        }
        public long Length => Playback.Stream?.Length ?? 0;

        public PlaybackState PlaybackState => Playback.Device?.PlaybackState ?? PlaybackState.Stopped;
        #endregion

        public Player()
        {
            Mute = false;
            Repeat = false;
            Shuffle = false;
            Volume = 0.3f;
        }

        public void Dispose()
        {
            Playback?.Dispose();
        }
    }
}
