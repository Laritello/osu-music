using Osu.Music.Common.Enums;
using Osu.Music.Common.Models;
using Osu.Music.Services.Audio;
using Osu.Music.Services.Events;
using Osu.Music.Services.Hotkeys;
using Osu.Music.Services.IO;
using Osu.Music.Services.UItility;
using Osu.Music.UI.Interfaces;
using Osu.Music.UI.Models;
using Osu.Music.UI.Visualization;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
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

        private Settings _settings;
        public Settings Settings
        {
            get => _settings;
            set => SetProperty(ref _settings, value);
        }

        private HotkeyManager _hotkeyManager;
        public HotkeyManager HotkeyManager
        {
            get => _hotkeyManager;
            set => SetProperty(ref _hotkeyManager, value);
        }

        private AudioPlayback _playback;
        public AudioPlayback Playback
        {
            get => _playback;
            set => SetProperty(ref _playback, value);
        }

        private BindableBase _selectedPage;
        public BindableBase SelectedPage
        {
            get => _selectedPage;
            set => SetProperty(ref _selectedPage, value);
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
        public DelegateCommand OpenGitHubCommand { get; private set; }
        public DelegateCommand<TimeSpan?> ScrollBeatmapCommand { get; private set; }
        public DelegateCommand<BindableBase> OpenPageCommand { get; private set; }
        public DelegateCommand<Popup> ChangePopupStateCommand { get; private set; }
        public DelegateCommand<Color?> UpdateColorCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }

        private DispatcherTimer _audioProgressTimer;

        public MainViewModel()
        {
            Model = new MainModel();
            SelectedPage = Model.SongsPage;
            Visualization = new DefaultVisualization();

            InitializeSettings();
            InitializeCommands();
            InitializePlayback();
            InitializeAudioProgressTimer();
            InitializeHotkeys();
            LoadBeatmaps();
        }

        private void InitializeSettings()
        {
            Settings = SettingsManager.Load();

            ResourceDictionary resource = Application.Current.Resources;
            resource.MergedDictionaries.SetMainColor(Settings.MainColor);
        }

        private void InitializeCommands()
        {
            MuteCommand = new DelegateCommand<bool?>(MuteVolume);
            PlayBeatmapCommand = new DelegateCommand<Beatmap>(PlayBeatmap);
            PauseBeatmapCommand = new DelegateCommand<Beatmap>(PauseBeatmap);
            StopBeatmapCommand = new DelegateCommand<Beatmap>(StopBeatmap);
            PreviousBeatmapCommand = new DelegateCommand<Beatmap>(PreviousBeatmap);
            NextBeatmapCommand = new DelegateCommand<Beatmap>(NextBeatmap);
            OpenGitHubCommand = new DelegateCommand(OpenGitHub);
            ScrollBeatmapCommand = new DelegateCommand<TimeSpan?>(ScrollBeatmap);
            OpenPageCommand = new DelegateCommand<BindableBase>(OpenPage);
            ChangePopupStateCommand = new DelegateCommand<Popup>(ChangePopupState);
            UpdateColorCommand = new DelegateCommand<Color?>(UpdateColor);
            ExitCommand = new DelegateCommand(Exit);
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

        private void InitializeHotkeys()
        {
            HotkeyManager = new HotkeyManager();
            HotkeyManager.Hotkeys = Settings.Hotkeys;
            HotkeyManager.HotkeyUsed += HotkeyManager_HotkeyUsed;
        }

        private async void LoadBeatmaps()
        {
            try
            {
                if (string.IsNullOrEmpty(Settings.OsuFolder))
                {
                    Settings.OsuFolder = PathHelper.GetOsuInstallationFolder();
                    SettingsManager.Save(Settings);
                }

                Model.Beatmaps = await LibraryLoader.LoadAsync(Settings.OsuFolder);
            }
            catch(Exception E)
            {
                MessageBox.Show(E.Message);
            }
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
            Playback.Mute = mute ?? false;
        }

        private void PlayBeatmap(Beatmap beatmap)
        {
            if (Playback.Beatmap != beatmap)
            {
                if (Playback.Beatmap != null)
                    Model.PreviousBeatmaps.Push(Playback.Beatmap);

                Model.PlayingBeatmap = beatmap;
                Playback.Beatmap = beatmap;
            }

            if (Playback.PlaybackState != NAudio.Wave.PlaybackState.Paused)
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

        private void ScrollBeatmap(TimeSpan? progress)
        {
            if (progress.HasValue)
                _playback.CurrentTime = progress.Value;
        }

        private void OpenPage(BindableBase page)
        {
            SelectedPage = page;
        }

        private void ChangePopupState(Popup popup)
        {
            popup.IsOpen = !popup.IsOpen;
        }

        private void UpdateColor(Color? color)
        {
            if (!color.HasValue)
                return;

            Settings.MainColor = color.Value.ToHex();
            ResourceDictionary resource = Application.Current.Resources;
            resource.MergedDictionaries.SetMainColor(Settings.MainColor);

            SettingsManager.Save(Settings);
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        private void OpenGitHub()
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start https://github.com/Laritello/osu-music") { CreateNoWindow = true });
        }

        private void UpdateBeatmapProgress(object sender, EventArgs e)
        {
            Model.CurrentTime = Playback.CurrentTime;
            Model.TotalTime = Playback.TotalTime;
            Model.Progress = Playback.CurrentTime.TotalSeconds / Playback.TotalTime.TotalSeconds;
        }

        private void HotkeyManager_HotkeyUsed(object sender, HotkeyEventArgs e)
        {
            switch (e.Type)
            {
                case HotkeyType.PlayPause:
                    PlayPauseHotkeyHandler();
                    break;
                case HotkeyType.PreviousTrack:
                    PreviousBeatmapHotkeyHandler();
                    break;
                case HotkeyType.NextTrack:
                    NextBeatmapHotkeyHandler();
                    break;
                case HotkeyType.Repeat:
                    RepeatHotkeyHandler();
                    break;
                case HotkeyType.Mute:
                    MuteHotkeyHandler();
                    break;
                case HotkeyType.Shuffle:
                    RandomHotkeyHandler();
                    break;
                case HotkeyType.VolumeDown:
                    VolumeDownHotkeyHandler();
                    break;
                case HotkeyType.VolumeUp:
                    VolumeUpHotkeyHandler();
                    break;
            }
        }

        private void PlayPauseHotkeyHandler()
        {
            if (Playback.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                PauseBeatmap(Model.SelectedBeatmap);
            else
                PlayBeatmap(Model.SelectedBeatmap);
        }

        private void PreviousBeatmapHotkeyHandler()
        {
            PreviousBeatmap(Model.SelectedBeatmap);
        }

        private void NextBeatmapHotkeyHandler()
        {
            NextBeatmap(Model.SelectedBeatmap);
        }

        private void RepeatHotkeyHandler()
        {
            Repeat = !Repeat;
        }

        private void MuteHotkeyHandler()
        {
            MuteVolume(!Playback.Mute);
        }

        private void RandomHotkeyHandler()
        {
            Random = !Random;
        }

        private void VolumeUpHotkeyHandler()
        {
            Playback.Volume += 0.05f;
        }

        private void VolumeDownHotkeyHandler()
        {
            Playback.Volume -= 0.05f;
        }
    }
}
