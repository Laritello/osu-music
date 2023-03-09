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

        private ObservableCollection<Playlist> _playlists;
        /// <summary>
        /// List of user playlists.
        /// </summary>
        public ObservableCollection<Playlist> Playlists
        {
            get => _playlists;
            set => SetProperty(ref _playlists, value);
        }

        private ObservableCollection<Collection> _collections;
        /// <summary>
        /// List of osu! collections.
        /// </summary>
        public ObservableCollection<Collection> Collections
        {
            get => _collections;
            set => SetProperty(ref _collections, value);
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
        #endregion

        public MainModel()
        {

        }
    }
}
