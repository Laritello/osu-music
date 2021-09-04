using Osu.Music.Common.Models;
using Osu.Music.Services.Audio;
using Osu.Music.Services.IO;
using Osu.Music.UI.Interfaces;
using Osu.Music.UI.Models;
using Osu.Music.UI.Visualization;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows.Threading;

namespace Osu.Music.UI.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private MainModel _model;
        public MainModel Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        private AudioPlayback _playback;
        public AudioPlayback Playback
        {
            get => _playback;
            set => SetProperty(ref _playback, value);
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

        private IVisualizationPlugin _visualization;
        public IVisualizationPlugin Visualization
        {
            get => _visualization;
            set => SetProperty(ref _visualization, value);
        }

        public DelegateCommand<bool?> MuteCommand { get; private set; }
        public DelegateCommand<Beatmap> PlayBeatmapCommand { get; private set; }
        public DelegateCommand<Beatmap> PauseBeatmapCommand { get; private set; }
        public DelegateCommand<Beatmap> StopBeatmapCommand { get; private set; }
        public DelegateCommand<Beatmap> PreviousBeatmapCommand { get; private set; }
        public DelegateCommand<Beatmap> NextBeatmapCommand { get; private set; }
        public DelegateCommand<long> ScrollAudioCommand { get; private set; }

        private DispatcherTimer _audioProgressTimer;

        public MainViewModel()
        {
            Model = new MainModel();
            //Visualization = new CircleVisualization();
            Visualization = new DefaultVisualization();
            InitializeCommands();
            InitializePlayback();
            InitializeAudioProgressTimer();
            LoadBeatmaps();
        }

        private void InitializeCommands()
        {
            MuteCommand = new DelegateCommand<bool?>(MuteVolume);
            PlayBeatmapCommand = new DelegateCommand<Beatmap>(PlayBeatmap);
            PauseBeatmapCommand = new DelegateCommand<Beatmap>(PauseBeatmap);
            StopBeatmapCommand = new DelegateCommand<Beatmap>(StopBeatmap);
            PreviousBeatmapCommand = new DelegateCommand<Beatmap>(PreviousBeatmap);
            NextBeatmapCommand = new DelegateCommand<Beatmap>(NextBeatmap);
        }
        private void InitializePlayback()
        {
            Playback = new AudioPlayback();
            Playback.BeatmapEnded += Playback_BeatmapEnded;
            Playback.FftCalculated += Playback_FftCalculated;
        }

        private void Playback_FftCalculated(object sender, FftEventArgs e)
        {
            Visualization.OnFftCalculated(e.Result);
        }

        private void InitializeAudioProgressTimer()
        {
            _audioProgressTimer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };

            _audioProgressTimer.Tick += UpdateBeatmapProgress;
            _audioProgressTimer.Start();
        }

        private async void LoadBeatmaps()
        {
            Model.Beatmaps = await LibraryLoader.LoadAsync();
        }

        private void Playback_BeatmapEnded(object sender, BeatmapEventArgs e)
        {
            if (Repeat)
                PlayBeatmap(Model.SelectedBeatmap);
            else
                NextBeatmap(Model.SelectedBeatmap);
        }

        private void MuteVolume(bool? mute)
        {
            Playback.Mute = mute?? false;
        }

        private void PlayBeatmap(Beatmap beatmap)
        {
            if (Playback.Beatmap != beatmap)
            {
                if (Playback.Beatmap != null)
                    Model.PreviousBeatmaps.Push(Playback.Beatmap);

                Model.PlayingBeatmap = beatmap;
                Playback.Beatmap = beatmap;
                Playback.Load();
            }

            Playback.Play();
        }

        private void PauseBeatmap(Beatmap beatmap)
        {
            Playback.Pause();
        }

        private void StopBeatmap(Beatmap beatmap)
        {
            Playback.Stop();
        }

        private void PreviousBeatmap(Beatmap beatmap)
        {
            if (Model.PreviousBeatmaps.Count == 0)
            {
                int index = Model.Beatmaps.IndexOf(beatmap) - 1;
                index = index < 0 ? Model.Beatmaps.Count - 1 : index;

                Model.SelectedBeatmap = Model.Beatmaps[index];
            }
            else
            {
                Model.SelectedBeatmap = Model.PreviousBeatmaps.Pop();
            }

            if (Playback.Beatmap != Model.SelectedBeatmap)
            {
                Model.PlayingBeatmap = Model.SelectedBeatmap;
                Playback.Beatmap = Model.SelectedBeatmap;
                Playback.Load();
            }

            Playback.Play();
        }

        private void NextBeatmap(Beatmap beatmap)
        {
            int index = GetNextMapIndex(beatmap);

            Model.SelectedBeatmap = Model.Beatmaps[index];
            PlayBeatmap(Model.SelectedBeatmap);
        }

        private int GetNextMapIndex(Beatmap beatmap)
        {
            int index;
            if (Random)
            {
                Random rnd = new Random();
                index = rnd.Next(0, Model.Beatmaps.Count);
            }
            else
            {
                index = Model.Beatmaps.IndexOf(beatmap) + 1;
            }

            return index >= Model.Beatmaps.Count ? 0 : index;
        }

        private void UpdateBeatmapProgress(object sender, EventArgs e)
        {
            Model.CurrentTime = Playback.CurrentTime;
            Model.TotalTime = Playback.TotalTime;
            Model.Progress = Playback.CurrentTime.TotalSeconds / Playback.TotalTime.TotalSeconds;
        }
    }
}
