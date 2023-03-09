using DryIoc;
using Osu.Music.Common;
using Osu.Music.Common.Enums;
using Osu.Music.Common.Models;
using Osu.Music.Common.Structures;
using Osu.Music.Services.Audio;
using Osu.Music.Services.Dialog;
using Osu.Music.Services.Events;
using Osu.Music.Services.Hotkeys;
using Osu.Music.Services.Interfaces;
using Osu.Music.Services.IO;
using Osu.Music.Services.UItility;
using Osu.Music.UI.Interfaces;
using Osu.Music.UI.Models;
using Osu.Music.UI.Utility;
using Osu.Music.UI.ViewModels.Dialogs;
using Osu.Music.UI.Views;
using Osu.Music.UI.Views.Dialogs;
using Osu.Music.UI.Visualization;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
        public DelegateCommand PlayBeatmapCommand { get; private set; }
        public DelegateCommand PauseBeatmapCommand { get; private set; }
        public DelegateCommand StopBeatmapCommand { get; private set; }
        public DelegateCommand PreviousBeatmapCommand { get; private set; }
        public DelegateCommand NextBeatmapCommand { get; private set; }
        public DelegateCommand OpenGitHubCommand { get; private set; }
        public DelegateCommand<TimeSpan?> ScrollBeatmapCommand { get; private set; }
        public DelegateCommand<string> OpenPageCommand { get; private set; }
        public DelegateCommand<Playlist> SelectPlaylistAndPlayCommand { get; private set; }
        public DelegateCommand CreatePlaylistCommand { get; private set; }
        public DelegateCommand<Beatmap> RemoveBeatmapFromPlaylistCommand { get; private set; }
        public DelegateCommand<Collection> SelectCollectionCommand { get; private set; }
        public DelegateCommand<Collection> SelectCollectionAndPlayCommand { get; private set; }
        public DelegateCommand<string> SearchCommand { get; private set; }
        public DelegateCommand<RoutedEventArgs> OnLoadedCommand { get; private set; }
        public DelegateCommand OnCloseCommand { get; private set; }
        #endregion

        private IContainer _container;

        private IPopupDialogService _dialogService;
        private IRegionManager _regionManager;
        private ILibraryManager _libraryManager;
        private ICollectionManager _collectionManager;
        private IPlaylistManager _playlistManager;
        private SettingsManager _settingsManager;
        private DispatcherTimer _audioProgressTimer;

        public MainViewModel(IContainer container)
        {
            _container = container;
            _dialogService = container.Resolve<IPopupDialogService>();
            _regionManager = container.Resolve<IRegionManager>();
            _libraryManager = container.Resolve<ILibraryManager>();
            _collectionManager = container.Resolve<ICollectionManager>();
            _playlistManager = container.Resolve<IPlaylistManager>();
            _playback = container.Resolve<AudioPlayback>();
            _settingsManager = container.Resolve<SettingsManager>();

            Model = new MainModel();
            Visualization = new DefaultVisualization();

            InitializeSettings();
            InitializeCommands();
            InitializePlayback();
            InitializeAudioProgressTimer();
            InitializeHotkeys();
            InitializeDiscord();

            LoadState();
            Load();
        }

        #region Initialization
        private void InitializeSettings()
        {
            Settings = _settingsManager.Settings;
            Settings.SourceChanged += Settings_SourceChanged;
            Settings.ColorChanged += Settings_ColorChanged;
            Settings.DiscordEnabledChanged += Settings_DiscordEnabledChanged;

            ResourceDictionary resource = Application.Current.Resources;
            resource.MergedDictionaries.SetMainColor(Settings.Color);
        }

        private void InitializeCommands()
        {
            MuteCommand = new DelegateCommand<bool?>(MuteVolume);
            PlayBeatmapAndUpdateCollectionCommand = new DelegateCommand<object[]>(PlayBeatmapAndUpdateCollection);
            PlayBeatmapCommand = new DelegateCommand(PlayBeatmap);
            PauseBeatmapCommand = new DelegateCommand(PauseBeatmap);
            StopBeatmapCommand = new DelegateCommand(StopBeatmap);
            PreviousBeatmapCommand = new DelegateCommand(PreviousBeatmap);
            NextBeatmapCommand = new DelegateCommand(NextBeatmap);
            OpenGitHubCommand = new DelegateCommand(OpenGitHub);
            ScrollBeatmapCommand = new DelegateCommand<TimeSpan?>(ScrollBeatmap);
            OpenPageCommand = new DelegateCommand<string>(OpenPage);
            SelectPlaylistAndPlayCommand = new DelegateCommand<Playlist>(SelectPlaylistAndPlay);
            CreatePlaylistCommand = new DelegateCommand(CreatePlaylist);
            RemoveBeatmapFromPlaylistCommand = new DelegateCommand<Beatmap>(RemoveBeatmapFromPlaylist);
            SelectCollectionCommand = new DelegateCommand<Collection>(SelectCollection);
            SelectCollectionAndPlayCommand = new DelegateCommand<Collection>(SelectCollectionAndPlay);
            SearchCommand = new DelegateCommand<string>(Search);
            OnLoadedCommand = new DelegateCommand<RoutedEventArgs>(OnLoaded);
            OnCloseCommand = new DelegateCommand(OnClose);
        }

        private void InitializePlayback()
        {
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

        // TODO: Switch to DI
        private void InitializeDiscord()
        {
            DiscordManager = new DiscordManager()
            {
                Enabled = Settings.DiscordEnabled
            };

            DiscordManager.Initialize();
        }
        #endregion

        private async void Load()
        {
            try
            {
                if (string.IsNullOrEmpty(Settings.Source))
                {
                    Settings.Source = PathHelper.GetOsuInstallationFolder();
                    _settingsManager.Save(Settings);
                }

                Model.Beatmaps = await _libraryManager.LoadAsync();
                Model.Playlists = await _playlistManager.LoadAsync();
                OpenPage("LibraryView");

                Playback.Queue = Model.Beatmaps;

                if (Model.PlaybackInitializationRequired)
                {
                    Model.PlaybackInitializationRequired = false;
                    LoadSavedPlayback();
                }
            }
            catch (Exception E)
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

            Playback.Queue = collection;
            Playback.Beatmap = beatmap;
            Playback.Play();
        }

        private void PlayBeatmap() => Playback.Play();

        private void PauseBeatmap() => Playback.Pause();

        private void StopBeatmap() => Playback.Stop();

        private void PreviousBeatmap() => Playback.Previous();

        private void NextBeatmap() => Playback.Next();

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
                case "LibraryView":
                    _regionManager.RequestNavigate(
                        RegionNames.ContentRegion, 
                        pageName, 
                        new NavigationParameters()
                        {
                            { "beatmaps", Model.Beatmaps }
                        });
                    break;
                case "PlaylistsView":
                    _regionManager.RequestNavigate(
                        RegionNames.ContentRegion, 
                        pageName, 
                        new NavigationParameters()
                        {
                            { "playlists", Model.Playlists }
                        });
                    break;
                case "SettingsView":
                    _regionManager.RequestNavigate(
                        RegionNames.ContentRegion,
                        pageName,
                        new NavigationParameters()
                        {
                            { "settings", Settings },
                            { "hotkey", HotkeyManager },
                            { "discord", DiscordManager }
                        });
                    break;
                default:
                    _regionManager.RequestNavigate(RegionNames.ContentRegion, pageName);
                    break;
            }
        }

        private void SelectPlaylistAndPlay(Playlist playlist)
        {
            Model.SelectedPlaylist = playlist;

            if (playlist.Beatmaps == null || playlist.Beatmaps.Count == 0)
                return;

            PlayBeatmapAndUpdateCollection(new object[2] { playlist.Beatmaps[0], playlist.Beatmaps });
        }

        private void SelectCollection(Collection collection) => Model.SelectedCollection = collection;

        private void SelectCollectionAndPlay(Collection collection)
        {
            Model.SelectedCollection = collection;

            if (collection.Beatmaps == null || collection.Beatmaps.Count == 0)
                return;

            PlayBeatmapAndUpdateCollection(new object[2] { collection.Beatmaps[0], collection.Beatmaps });
        }

        private void CreatePlaylist()
        {
            DialogParameters parameters = new DialogParameters()
            {
                { "names", Model.Playlists.Select(x => x.Name) }
            };

            _dialogService.ShowPopupDialog<NewPlaylistView, NewPlaylistViewModel>(parameters, e =>
            {
                if (e.Result == ButtonResult.OK)
                {
                    var playlist = e.Parameters.GetValue<Playlist>("playlist");
                    Model.Playlists.Add(playlist);
                    _playlistManager.Save(playlist);

                    _regionManager.RequestNavigate(
                        RegionNames.ContentRegion,
                        "PlaylistDetailsView",
                        new NavigationParameters()
                        {
                            { "playlist", playlist }
                        });
                }
            });
        }

        private void RemoveBeatmapFromPlaylist(Beatmap beatmap)
        {
            Model.SelectedPlaylist.Beatmaps.Remove(beatmap);
            _playlistManager.Save(Model.SelectedPlaylist);
        }

        private void Search(string request) => _regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(SearchView));

        private void OnLoaded(RoutedEventArgs args)
        {
            if (args.Source is UserControl view)
            {
                Binding b = new Binding()
                {
                    Source = view.DataContext,
                    Path = new PropertyPath("OnCloseCommand"),
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };

                var window = Window.GetWindow(view);
                window.SetBinding(WindowClosingBehavior.ClosingProperty, b);
            }
        }

        private void OnClose()
        {
            try
            {
                Settings.State = new PlayerState()
                {
                    Repeat = Playback.Repeat,
                    Shuffle = Playback.Shuffle,
                    Volume = Playback.Volume,
                    SelectedBeatmapId = Playback.Beatmap?.BeatmapSetId,
                    IsPlaying = Playback.PlaybackState == NAudio.Wave.PlaybackState.Playing,
                    Position = Playback.Position
                };

                _settingsManager.Save(Settings);
            }
            catch { }
        }

        private void OpenGitHub() => Process.Start(new ProcessStartInfo("cmd", $"/c start https://github.com/Laritello/osu-music") { CreateNoWindow = true });

        #region Handlers
        private void UpdateBeatmapProgress(object sender, EventArgs e)
        {
            Model.CurrentTime = Playback.CurrentTime;
            Model.TotalTime = Playback.TotalTime;
            Model.Progress = Playback.CurrentTime.TotalSeconds / Playback.TotalTime.TotalSeconds;
        }

        private void Playback_FftCalculated(object sender, FftEventArgs e) => Visualization.OnFftCalculated(e.Result);

        private void Settings_SourceChanged(string path) => Load();

        private void Settings_ColorChanged(string hex)
        {
            if (string.IsNullOrEmpty(hex))
                return;

            ResourceDictionary resource = Application.Current.Resources;
            resource.MergedDictionaries.SetMainColor(hex);
        }

        private void Settings_DiscordEnabledChanged(bool enabled)
        {
            DiscordManager.Enabled = enabled;

            if (!enabled)
                DiscordManager.ClearPresence();
        }
        #endregion

        #region General Methods
        private void LoadState()
        {
            Playback.Shuffle = Settings.State.Shuffle;
            Playback.Repeat = Settings.State.Repeat;
            Playback.Volume = Settings.State.Volume;

            Model.PlaybackInitializationRequired = Settings.State.SelectedBeatmapId.HasValue;
        }

        private void LoadSavedPlayback()
        {
            var beatmap = Model.Beatmaps.Where(x => x.BeatmapSetId == Settings.State.SelectedBeatmapId).FirstOrDefault();

            if (beatmap != null)
            {
                Playback.Beatmap = beatmap;
                Playback.Queue = Model.Beatmaps;
                Playback.Load();
                Playback.Position = Settings.State.Position;

                if (Settings.State.IsPlaying)
                    Playback.Resume();
            }
        }
        #endregion

        #region Hotkeys
        private void HotkeyManager_HotkeyUsed(object sender, HotkeyEventArgs e)
        {
            if (!Settings.HotkeysEnabled)
                return;

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
                    ShuffleHotkeyHandler();
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
            _settingsManager.Save(Settings);
        }

        private void PlayPauseHotkeyHandler()
        {
            if (Playback.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                Playback.Pause();
            else
                Playback.Play();
        }

        private void PreviousBeatmapHotkeyHandler() => Playback.Previous();

        private void NextBeatmapHotkeyHandler() => Playback.Next();

        private void RepeatHotkeyHandler() => Playback.Repeat = !Playback.Repeat;

        private void MuteHotkeyHandler() => MuteVolume(!Playback.Mute);

        private void ShuffleHotkeyHandler() => Playback.Shuffle = !Playback.Shuffle;

        private void VolumeUpHotkeyHandler() => Playback.Volume += 0.05f;

        private void VolumeDownHotkeyHandler() => Playback.Volume -= 0.05f;
        #endregion
    }
}
