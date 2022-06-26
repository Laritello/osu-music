using NAudio.Wave;
using Osu.Music.Common.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Osu.Music.Services.Audio
{
    public class Player : BindableBase, IDisposable
    {
        #region Properties
        private AudioPlayback _playback;
        public AudioPlayback Playback
        {
            get => _playback;
            set => SetProperty(ref _playback, value);
        }

        private ObservableCollection<Beatmap> _beatmaps;
        /// <summary>
        /// Full list of beatmaps from osu library.
        /// </summary>
        public ObservableCollection<Beatmap> Beatmaps
        {
            get => _beatmaps;
            set => SetProperty(ref _beatmaps, value);
        }

        private ObservableCollection<Playlist> _playlists;
        /// <summary>
        /// Collection of user-created playlists.
        /// </summary>
        public ObservableCollection<Playlist> Playlists
        {
            get => _playlists;
            set => SetProperty(ref _playlists, value);
        }

        private ObservableCollection<Collection> _collections;
        /// <summary>
        /// Collection of osu! collections.
        /// </summary>
        public ObservableCollection<Collection> Collections
        {
            get => _collections;
            set => SetProperty(ref _collections, value);
        }

        private ObservableCollection<Beatmap> _selectedBeatmaps;
        /// <summary>
        /// Current list of played beatmaps.
        /// </summary>
        public ObservableCollection<Beatmap> SelectedBeatmaps
        {
            get => _selectedBeatmaps;
            set => SetProperty(ref _selectedBeatmaps, value);
        }

        private Beatmap _playingBeatmap;
        /// <summary>
        /// Beatmap that is currently playing.
        /// </summary>
        public Beatmap PlayingBeatmap
        {
            get => _playingBeatmap;
            set => SetProperty(ref _playingBeatmap, value);
        }

        private Beatmap _selectedBeatmap;
        /// <summary>
        /// Beatmap that is currently selected in ListBox.
        /// </summary>
        public Beatmap SelectedBeatmap
        {
            get => _selectedBeatmap;
            set => SetProperty(ref _selectedBeatmap, value);
        }

        private Stack<Beatmap> _previousBeatmaps;
        /// <summary>
        /// Stack of previously played beatmaps.
        /// </summary>
        public Stack<Beatmap> PreviousBeatmaps
        {
            get => _previousBeatmaps;
            set => SetProperty(ref _previousBeatmaps, value);
        }

        private Collection _selectedCollection;
        /// <summary>
        /// Selected osu! collection.
        /// </summary>
        public Collection SelectedCollection
        {
            get => _selectedCollection;
            set => SetProperty(ref _selectedCollection, value);
        }

        private Playlist _selectedPlaylist;
        /// <summary>
        /// Selected user-created playlist.
        /// </summary>
        public Playlist SelectedPlaylist
        {
            get => _selectedPlaylist;
            set => SetProperty(ref _selectedPlaylist, value);
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
