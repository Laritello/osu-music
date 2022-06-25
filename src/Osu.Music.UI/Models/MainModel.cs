using Osu.Music.Common.Models;
using Osu.Music.Services.Dialog;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Osu.Music.UI.Models
{
    public class MainModel : BindableBase
    {
        #region Properties
        private ObservableCollection<Beatmap> _beatmaps;
        /// <summary>
        /// Full list of beatmaps from osu library.
        /// </summary>
        public ObservableCollection<Beatmap> Beatmaps
        {
            get => _beatmaps;
            set => SetProperty(ref _beatmaps, value);
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

        private TimeSpan currentTime;
        public TimeSpan CurrentTime
        {
            get => currentTime;
            set => SetProperty(ref currentTime, value);
        }

        private TimeSpan _totalTime;
        public TimeSpan TotalTime
        {
            get => _totalTime;
            set => SetProperty(ref _totalTime, value);
        }

        private double _progress;
        public double Progress
        {
            get => _progress;
            set => SetProperty(ref _progress, value);
        }

        private bool _playbackInitializationRequired;
        public bool PlaybackInitializationRequired
        {
            get => _playbackInitializationRequired;
            set => SetProperty(ref _playbackInitializationRequired, value);
        }

        private IPopupDialogService _dialogService;
        public IPopupDialogService DialogService
        {
            get => _dialogService;
            set => SetProperty(ref _dialogService, value);
        }
        #endregion

        public MainModel()
        {
            PreviousBeatmaps = new Stack<Beatmap>(100);
            Playlists = new ObservableCollection<Playlist>();
            Collections = new ObservableCollection<Collection>();
        }
    }
}
