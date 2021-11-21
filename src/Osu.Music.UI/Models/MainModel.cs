﻿using Osu.Music.Common.Models;
using Osu.Music.UI.ViewModels;
using Prism.Mvvm;
using System;
using System.Collections.Generic;

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

        private BindableBase _songsPage;
        public BindableBase SongsPage
        {
            get => _songsPage;
            set => SetProperty(ref _songsPage, value);
        }

        private BindableBase _settingsPage;
        public BindableBase SettingsPage
        {
            get => _settingsPage;
            set => SetProperty(ref _settingsPage, value);
        }

        private BindableBase _aboutPage;
        public BindableBase AboutPage
        {
            get => _aboutPage;
            set => SetProperty(ref _aboutPage, value);
        }
        #endregion

        public MainModel()
        {
            PreviousBeatmaps = new Stack<Beatmap>(100);
            AboutPage = new AboutViewModel();
            SettingsPage = new SettingsViewModel();

            SongsPage = new SongsViewModel();
        }
    }
}