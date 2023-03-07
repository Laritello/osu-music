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
        /// Full list of user playlists.
        /// </summary>
        public ObservableCollection<Playlist> Playlists
        {
            get => _playlists;
            set => SetProperty(ref _playlists, value);
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
        #endregion

        public MainModel()
        {

        }
    }
}
