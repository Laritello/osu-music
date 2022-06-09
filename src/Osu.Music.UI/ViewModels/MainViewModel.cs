using Osu.Music.Common.Enums;
using Osu.Music.Common.Models;
using Osu.Music.Services.Audio;
using Osu.Music.Services.Dialog;
using Osu.Music.Services.Events;
using Osu.Music.Services.Hotkeys;
using Osu.Music.Services.IO;
using Osu.Music.Services.UItility;
using Osu.Music.UI.Interfaces;
using Osu.Music.UI.Models;
using Osu.Music.UI.Parameters;
using Osu.Music.UI.Views;
using Osu.Music.UI.Visualization;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace Osu.Music.UI.ViewModels
{
    public class MainViewModel : BindableBase
    {
        #region Properties
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

        private DiscordManager _discordManager;
        public DiscordManager DiscordManager
        {
            get => _discordManager;
            set => SetProperty(ref _discordManager, value);
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
        #endregion

        #region Commands
        public DelegateCommand<bool?> MuteCommand { get; private set; }
        public DelegateCommand<object[]> PlayBeatmapAndUpdateCollectionCommand { get; private set; }
        public DelegateCommand<Beatmap> PlayBeatmapCommand { get; private set; }
        public DelegateCommand<Beatmap> PauseBeatmapCommand { get; private set; }
        public DelegateCommand<Beatmap> StopBeatmapCommand { get; private set; }
        public DelegateCommand<Beatmap> PreviousBeatmapCommand { get; private set; }
        public DelegateCommand<Beatmap> NextBeatmapCommand { get; private set; }
        public DelegateCommand OpenGitHubCommand { get; private set; }
        public DelegateCommand<TimeSpan?> ScrollBeatmapCommand { get; private set; }
        public DelegateCommand<string> OpenPageCommand { get; private set; }
        public DelegateCommand OpenAboutCommand { get; private set; }
        public DelegateCommand OpenSettingsCommand { get; private set; }
        public DelegateCommand<Beatmap> OpenBeatmapInExplorerCommand { get; private set; }
        public DelegateCommand<Playlist> SendBeatmapToPlaylistCommand { get; private set; }
        public DelegateCommand<Playlist> SelectPlaylistCommand { get; private set; }
        public DelegateCommand<Playlist> DeletePlaylistCommand { get; private set; }
        public DelegateCommand<Beatmap> RemoveBeatmapFromPlaylistCommand { get; private set; }
        #endregion

        private DispatcherTimer _audioProgressTimer;

        public MainViewModel(IContainerExtension container)
        {
            Model = new MainModel();
            Visualization = new DefaultVisualization();

            InitializeDialogService(container);
            InitializeSettings();
            InitializeCommands();
            InitializePlayback();
            InitializeAudioProgressTimer();
            InitializeHotkeys();
            InitializeDiscord();
            
            LoadData();
        }

        #region Initialization
        private void InitializeSettings()
        {
            Settings = SettingsManager.Load();

            ResourceDictionary resource = Application.Current.Resources;
            resource.MergedDictionaries.SetMainColor(Settings.MainColor);
        }

        private void InitializeCommands()
        {
            MuteCommand = new DelegateCommand<bool?>(MuteVolume);
            PlayBeatmapAndUpdateCollectionCommand = new DelegateCommand<object[]>(PlayBeatmapAndUpdateCollection);
            PlayBeatmapCommand = new DelegateCommand<Beatmap>(PlayBeatmap);
            PauseBeatmapCommand = new DelegateCommand<Beatmap>(PauseBeatmap);
            StopBeatmapCommand = new DelegateCommand<Beatmap>(StopBeatmap);
            PreviousBeatmapCommand = new DelegateCommand<Beatmap>(PreviousBeatmap);
            NextBeatmapCommand = new DelegateCommand<Beatmap>(NextBeatmap);
            OpenGitHubCommand = new DelegateCommand(OpenGitHub);
            ScrollBeatmapCommand = new DelegateCommand<TimeSpan?>(ScrollBeatmap);
            OpenPageCommand = new DelegateCommand<string>(OpenPage);
            OpenAboutCommand = new DelegateCommand(OpenAbout);
            OpenSettingsCommand = new DelegateCommand(OpenSettings);
            OpenBeatmapInExplorerCommand = new DelegateCommand<Beatmap>(OpenBeatmapInExplorer);
            SendBeatmapToPlaylistCommand = new DelegateCommand<Playlist>(SendBeatmapToPlaylist);
            SelectPlaylistCommand = new DelegateCommand<Playlist>(SelectPlaylist);
            DeletePlaylistCommand = new DelegateCommand<Playlist>(DeletePlaylist);
            RemoveBeatmapFromPlaylistCommand = new DelegateCommand<Beatmap>(RemoveBeatmapFromPlaylist);
        }
        
        private void InitializePlayback()
        {
            Playback = new AudioPlayback();
            Playback.BeatmapEnded += Playback_BeatmapEnded;
            Playback.FftCalculated += Playback_FftCalculated;
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
            HotkeyManager = new HotkeyManager
            {
                Hotkeys = Settings.Hotkeys
            };

            HotkeyManager.HotkeyUsed += HotkeyManager_HotkeyUsed;
            HotkeyManager.HotkeyChanged += HotkeyManager_HotkeyChanged;
        }

        private void InitializeDiscord()
        {
            DiscordManager = new DiscordManager()
            {
                Enabled = Settings.DiscordRpcEnabled
            };

            DiscordManager.Initialize();
        }

        private void InitializeDialogService(IContainerExtension container)
        {
            Model.DialogService = new PopupDialogService(container);
        }
        #endregion

        private async void LoadData()
        {
            try
            {
                if (string.IsNullOrEmpty(Settings.OsuFolder))
                {
                    Settings.OsuFolder = PathHelper.GetOsuInstallationFolder();
                    SettingsManager.Save(Settings);
                }

                Model.Beatmaps = await LibraryManager.LoadAsync(Settings.OsuFolder);
                Model.Playlists = await PlaylistManager.LoadAsync(Model.Beatmaps);

                SelectedPage = new SongsViewModel();
            }
            catch(Exception E)
            {
                MessageBox.Show(E.Message);
            }
        }

        private void MuteVolume(bool? mute)
        {
            Playback.Mute = mute ?? false;
        }

        private void PlayBeatmapAndUpdateCollection(object[] parameters)
        {
            var beatmap = parameters[0] as Beatmap;
            var collection = parameters[1] as ObservableCollection<Beatmap>;

            Model.SelectedBeatmaps = collection;
           
            // If user tried to start playback without selected song
            // Select first song if possible
            CheckBeatmap(ref beatmap);

            // If new song was selected
            // Update playback
            if (Playback.Beatmap != beatmap)
            {
                Model.PlayingBeatmap = beatmap;
                Playback.Beatmap = beatmap;
                Playback.Load();

                DiscordManager.Update(Model.PlayingBeatmap);
            }

            // TODO: Rework this section
            if (Playback.PlaybackState != NAudio.Wave.PlaybackState.Paused)
                Playback.Load();

            if (Playback.PlaybackState == NAudio.Wave.PlaybackState.Paused)
                DiscordManager.Resume(Playback.CurrentTime);

            Playback.Play();
        }

        private void PlayBeatmap(Beatmap beatmap)
        {
            // If user tried to start playback without selected song
            // Select first song if possible
            CheckBeatmap(ref beatmap);

            // If new song was selected
            // Update playback
            if (Playback.Beatmap != beatmap)
            {
                Model.PlayingBeatmap = beatmap;
                Playback.Beatmap = beatmap;
                Playback.Load();

                DiscordManager.Update(Model.PlayingBeatmap);
            }

            // TODO: Rework this section
            if (Playback.PlaybackState != NAudio.Wave.PlaybackState.Paused)
                Playback.Load();

            if (Playback.PlaybackState == NAudio.Wave.PlaybackState.Paused)
                DiscordManager.Resume(Playback.CurrentTime);

            Playback.Play();
        }

        private void CheckBeatmap(ref Beatmap beatmap)
        {
            if (Model.SelectedBeatmaps == null || Model.SelectedBeatmaps.Count == 0)
                return;

            if (beatmap == null)
                beatmap = Model.SelectedBeatmaps[0];
        }

        private void PauseBeatmap(Beatmap beatmap)
        {
            Playback.Pause();
            DiscordManager.Pause();
        }

        private void StopBeatmap(Beatmap beatmap)
        {
            Playback.Stop();
        }

        private void PreviousBeatmap(Beatmap beatmap)
        {
            if (Model.PreviousBeatmaps.Count == 0)
            {
                int index = Model.SelectedBeatmaps.IndexOf(beatmap) - 1;
                index = index < 0 ? Model.SelectedBeatmaps.Count - 1 : index;

                Model.SelectedBeatmap = Model.SelectedBeatmaps[index];
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

            DiscordManager.Update(Model.SelectedBeatmap);
            Playback.Play();
        }

        private void NextBeatmap(Beatmap beatmap)
        {
            int index = GetNextMapIndex(beatmap);

            if (beatmap != null)
                Model.PreviousBeatmaps.Push(Playback.Beatmap);

            Model.SelectedBeatmap = Model.SelectedBeatmaps[index];
            PlayBeatmap(Model.SelectedBeatmap);
        }

        private int GetNextMapIndex(Beatmap beatmap)
        {
            int index;
            if (Random)
            {
                Random rnd = new Random();
                index = rnd.Next(0, Model.SelectedBeatmaps.Count);
            }
            else
            {
                index = Model.SelectedBeatmaps.IndexOf(beatmap) + 1;
            }

            return index >= Model.SelectedBeatmaps.Count ? 0 : index;
        }

        private void ScrollBeatmap(TimeSpan? progress)
        {
            if (progress.HasValue)
                _playback.CurrentTime = progress.Value;

            DiscordManager.Resume(_playback.CurrentTime);
        }

        private void OpenPage(string pageName)
        {
            switch (pageName)
            {
                case "Songs":
                    SelectedPage = new SongsViewModel();
                    break;
                case "Playlists":
                    SelectedPage = new PlaylistsViewModel(Model.Playlists,Model.DialogService);
                    break;
            }
        }

        private void OpenAbout()
        {
            Model.DialogService.ShowPopupDialog<AboutView, AboutViewModel>();
        }

        private void OpenSettings()
        {
            var parameters = new DialogParameters
            {
                { "settings", Settings },
                { "hotkey", HotkeyManager },
                { "discord", DiscordManager }
            };
            Model.DialogService.ShowPopupDialog<SettingsView, SettingsViewModel>(parameters, callback => { });
        }

        private void OpenBeatmapInExplorer(Beatmap beatmap)
        {
            try
            {
                if (beatmap != null && Directory.Exists(beatmap.Directory))
                    Process.Start("explorer.exe", beatmap.Directory);
            }
            catch
            {
                // Ignore
            }
        }

        private void SendBeatmapToPlaylist(Playlist playlist)
        {
            var beatmap = Model.SelectedBeatmap;

            if (!playlist.Beatmaps.Contains(beatmap))
            {
                playlist.Beatmaps.Add(beatmap);
                PlaylistManager.Save(playlist);
            }
        }

        private void SelectPlaylist(Playlist playlist)
        {
            Model.SelectedPlaylist = playlist;
        }

        private void DeletePlaylist(Playlist playlist)
        {
            Model.Playlists.Remove(playlist);

            PlaylistManager.Remove(playlist);
        }

        private void RemoveBeatmapFromPlaylist(Beatmap beatmap)
        {
            Model.SelectedPlaylist.Beatmaps.Remove(beatmap);
            PlaylistManager.Save(Model.SelectedPlaylist);
        }

        private void OpenGitHub()
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start https://github.com/Laritello/osu-music") { CreateNoWindow = true });
        }

        #region Handlers
        private void UpdateBeatmapProgress(object sender, EventArgs e)
        {
            Model.CurrentTime = Playback.CurrentTime;
            Model.TotalTime = Playback.TotalTime;
            Model.Progress = Playback.CurrentTime.TotalSeconds / Playback.TotalTime.TotalSeconds;
        }

        private void Playback_BeatmapEnded(object sender, BeatmapEventArgs e)
        {
            if (Repeat)
                PlayBeatmap(Model.SelectedBeatmap);
            else
                NextBeatmap(Model.SelectedBeatmap);
        }

        private void Playback_FftCalculated(object sender, FftEventArgs e)
        {
            Visualization.OnFftCalculated(e.Result);
        }
        #endregion

        #region Hotkeys
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

        private void HotkeyManager_HotkeyChanged()
        {
            Settings.Hotkeys = HotkeyManager.Hotkeys;
            SettingsManager.Save(Settings);
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
        #endregion
    }
}
