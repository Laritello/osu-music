using Osu.Music.Common.Models;
using Osu.Music.Services.Audio;
using Osu.Music.Services.IO;
using Osu.Music.UI.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            InitializeCommands();
            InitializePlayback();
            InitializeAudioProgressTimer();
            LoadBeatmaps();
        }

        private void InitializeCommands()
        {
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
        }

        private void InitializeAudioProgressTimer()
        {
            _audioProgressTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };

            _audioProgressTimer.Tick += UpdateBeatmapProgress;
            _audioProgressTimer.Start();
        }

        private async void LoadBeatmaps()
        {
            Model.Beatmaps = await LibraryLoader.LoadAsync(@"D:\Games\osu!");
        }

        private void Playback_BeatmapEnded(object sender, BeatmapEventArgs e)
        {
            if (Model.Repeat)
                PlayBeatmap(Model.CurrentBeatmap);
            else
                NextBeatmap(Model.CurrentBeatmap);
        }

        private void PlayBeatmap(Beatmap beatmap)
        {
            if (Playback.Beatmap != beatmap)
            {
                if (Playback.Beatmap != null)
                    Model.PreviousBeatmaps.Push(Playback.Beatmap);

                Playback.Beatmap = beatmap;
            }

            Playback.Load();
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

                Model.CurrentBeatmap = Model.Beatmaps[index];
            }
            else
            {
                Model.CurrentBeatmap = Model.PreviousBeatmaps.Pop();
            }

            if (Playback.Beatmap != Model.CurrentBeatmap)
            {
                Playback.Beatmap = Model.CurrentBeatmap;
                Playback.Load();
            }

            Playback.Play();
        }

        private void NextBeatmap(Beatmap beatmap)
        {
            int index = GetNextMapIndex(beatmap);

            Model.CurrentBeatmap = Model.Beatmaps[index];
            PlayBeatmap(Model.CurrentBeatmap);
        }

        private int GetNextMapIndex(Beatmap beatmap)
        {
            int index;
            if (Model.Random)
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
            Model.Position = Playback.Position;
            Model.Length = Playback.Length;
        }
    }
}
