﻿using Osu.Music.Common.Models;
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
        private IList<Beatmap> _beatmaps;
        /// <summary>
        /// Full list of beatmaps from osu library.
        /// </summary>
        public IList<Beatmap> Beatmaps
        {
            get => _beatmaps;
            set => SetProperty(ref _beatmaps, value);
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

        private IList<Playlist> _playlists;
        /// <summary>
        /// Collection of user-created playlists.
        /// </summary>
        public IList<Playlist> Playlists
        {
            get => _playlists;
            set => SetProperty(ref _playlists, value);
        }

        private Playlist _selectedPlaylist;
        /// <summary>
        /// Collection of user-created playlists.
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
        }
    }
}
