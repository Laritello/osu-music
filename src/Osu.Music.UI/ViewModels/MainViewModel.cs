using DryIoc;
using Osu.Music.Common;
using Osu.Music.Common.Interfaces;
using Osu.Music.Common.Models;
using Osu.Music.Common.Structures;
using Osu.Music.Services.Audio;
using Osu.Music.Services.Dialog;
using Osu.Music.Services.Events;
using Osu.Music.Services.Hotkeys;
using Osu.Music.Services.Interfaces;
using Osu.Music.Services.IO;
using Osu.Music.Services.Localization;
using Osu.Music.Services.Social;
using Osu.Music.Services.UItility;
using Osu.Music.UI.Behaviors;
using Osu.Music.UI.Interfaces;
using Osu.Music.UI.Models;
using Osu.Music.UI.ViewModels.Dialogs;
using Osu.Music.UI.Views.Dialogs;
using Osu.Music.UI.Visualization;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
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
        public DelegateCommand PlayBeatmapCommand { get; private set; }
        public DelegateCommand PauseBeatmapCommand { get; private set; }
        public DelegateCommand StopBeatmapCommand { get; private set; }
        public DelegateCommand PreviousBeatmapCommand { get; private set; }
        public DelegateCommand NextBeatmapCommand { get; private set; }
        public DelegateCommand OpenGitHubCommand { get; private set; }
        public DelegateCommand<TimeSpan?> ScrollBeatmapCommand { get; private set; }
        public DelegateCommand<string> OpenPageCommand { get; private set; }
        public DelegateCommand CreatePlaylistCommand { get; private set; }
        public DelegateCommand SearchCommand { get; private set; }
        public DelegateCommand<RoutedEventArgs> OnLoadedCommand { get; private set; }
        public DelegateCommand OnCloseCommand { get; private set; }
        #endregion

        private readonly IPopupDialogService _dialogService;
        private readonly IRegionManager _regionManager;
        private readonly ILibraryProvider _libraryManager;
        private readonly ICollectionProvider _collectionProvider;
        private readonly IPlaylistProvider _playlistProvider;
        private readonly SettingsProvider _settingsManager;
        private readonly DiscordManager _discordManager;
        private readonly HotkeyManager _hotkeyManager;
        private readonly LocalizationManager _localizationManager;
        private DispatcherTimer _audioProgressTimer;
        private Settings _settings;

        public MainViewModel(IContainer container, MainModel model)
        {
            _dialogService = container.Resolve<IPopupDialogService>();
            _regionManager = container.Resolve<IRegionManager>();
            _libraryManager = container.Resolve<ILibraryProvider>();
            _collectionProvider = container.Resolve<ICollectionProvider>();
            _playlistProvider = container.Resolve<IPlaylistProvider>();
            _playback = container.Resolve<AudioPlayback>();
            _settingsManager = container.Resolve<SettingsProvider>();
            _discordManager = container.Resolve<DiscordManager>();
            _hotkeyManager = container.Resolve<HotkeyManager>();
            _localizationManager = LocalizationManager.Instance;

            _model = model;

            Visualization = new DefaultVisualization();

            InitializeSettings();
            InitializeCommands();
            InitializePlayback();
            InitializeAudioProgressTimer();
            InitializeHotkeys();
            InitializeDiscord();

            LoadState();
        }

        #region Initialization
        private void InitializeSettings()
        {
            _settings = _settingsManager.Settings;
            _settings.SourceChanged += Settings_SourceChanged;
            _settings.ColorChanged += Settings_ColorChanged;
            _settings.DiscordEnabledChanged += Settings_DiscordEnabledChanged;
            _settings.CultureChanged += Settings_CultureChanged;

            ResourceDictionary resource = Application.Current.Resources;
            resource.MergedDictionaries.SetMainColor(_settings.Color);

            _localizationManager.Culture = LocalizationFactory.GetCulture(_settings.Culture);
        }

        private void InitializeCommands()
        {
            MuteCommand = new DelegateCommand<bool?>(MuteVolume);
            PlayBeatmapCommand = new DelegateCommand(PlayBeatmap);
            PauseBeatmapCommand = new DelegateCommand(PauseBeatmap);
            StopBeatmapCommand = new DelegateCommand(StopBeatmap);
            PreviousBeatmapCommand = new DelegateCommand(PreviousBeatmap);
            NextBeatmapCommand = new DelegateCommand(NextBeatmap);
            OpenGitHubCommand = new DelegateCommand(OpenGitHub);
            ScrollBeatmapCommand = new DelegateCommand<TimeSpan?>(ScrollBeatmap);
            OpenPageCommand = new DelegateCommand<string>(OpenPage);
            CreatePlaylistCommand = new DelegateCommand(CreatePlaylist);
            SearchCommand = new DelegateCommand(Search);
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
            _hotkeyManager.HotkeyChanged += HotkeyManager_HotkeyChanged;
        }

        private void InitializeDiscord()
        {
            _discordManager.Enabled = _settings.DiscordEnabled;
            _discordManager.Initialize();
        }
        #endregion

        private void Load()
        {
            try
            {
                if (string.IsNullOrEmpty(_settings.Source))
                {
                    _settings.Source = PathHelper.GetOsuInstallationFolder();
                    _settingsManager.Save(_settings);
                }

                Model.Beatmaps = _libraryManager.Beatmaps;
                Model.Playlists = _playlistProvider.Playlists;
                Model.Collections = _collectionProvider.Collections;
                Playback.Queue = _libraryManager.Beatmaps;

                OpenPage("LibraryView");

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

        private void MuteVolume(bool? mute) => Playback.Mute = mute ?? false;

        private void PlayBeatmap()
        {
            if (Playback.PlaybackState == NAudio.Wave.PlaybackState.Paused)
                Playback.Resume();
            else
                Playback.Play();
        }

        private void PauseBeatmap() => Playback.Pause();

        private void StopBeatmap() => Playback.Stop();

        private void PreviousBeatmap() => Playback.Previous();

        private void NextBeatmap() => Playback.Next();

        private void ScrollBeatmap(TimeSpan? progress)
        {
            if (progress.HasValue)
                _playback.CurrentTime = progress.Value;

            _discordManager.Resume(_playback.CurrentTime);
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
                case "CollectionsView":
                    _regionManager.RequestNavigate(
                        RegionNames.ContentRegion,
                        pageName,
                        new NavigationParameters()
                        {
                            { "collections", Model.Collections }
                        });
                    break;
                case "SettingsView":
                    _regionManager.RequestNavigate(
                        RegionNames.ContentRegion,
                        pageName,
                        new NavigationParameters()
                        {
                            { "settings", _settings },
                            { "hotkey", _hotkeyManager },
                            { "discord", _discordManager }
                        });
                    break;
                default:
                    _regionManager.RequestNavigate(RegionNames.ContentRegion, pageName);
                    break;
            }
        }

        private void CreatePlaylist()
        {
            DialogParameters parameters = new DialogParameters()
            {
                { "title", _localizationManager.GetLocalizedString("Strings.MainView.NewPlaylistDialog.Title") },
                { "caption", _localizationManager.GetLocalizedString("Strings.MainView.NewPlaylistDialog.Create") },
                { "names", Model.Playlists.Select(x => x.Name) }
            };

            _dialogService.ShowPopupDialog<ManagePlaylistNameView, ManagePlaylistNameViewModel>(parameters, e =>
            {
                if (e.Result == ButtonResult.OK)
                {
                    var name = e.Parameters.GetValue<string>("name");

                    Playlist playlist = new Playlist()
                    {
                        Name = name,
                        Updated = DateTime.Now
                    };

                    Model.Playlists.Add(playlist);

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

        private void Search()
        {
            DialogParameters parameters = new DialogParameters()
            {
                { "beatmaps", Model.Beatmaps },
                { "playlists", Model.Playlists },
                { "collections", Model.Collections }
            };

            _dialogService.ShowPopupDialog<SearchView, SearchViewModel>(parameters, e =>
            {
                if (e.Result == ButtonResult.OK)
                {
                    var target = e.Parameters.GetValue<ISearchable>("target");

                    switch (target.GetNavigationView())
                    {
                        case "LibraryView":
                            _regionManager.RequestNavigate(
                                RegionNames.ContentRegion,
                                "LibraryView",
                                new NavigationParameters()
                                {
                                    { "beatmaps", Model.Beatmaps },
                                    { "target", target }
                                });
                            break;
                        case "PlaylistDetailsView":
                            _regionManager.RequestNavigate(
                                RegionNames.ContentRegion,
                                "PlaylistDetailsView",
                                new NavigationParameters()
                                {
                                    { "playlist", target }
                                });
                            break;
                        case "CollectionDetailsView":
                            _regionManager.RequestNavigate(
                                RegionNames.ContentRegion,
                                "CollectionDetailsView",
                                new NavigationParameters()
                                {
                                    { "collection", target }
                                });
                            break;
                    }
                }
            });
        }

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

            Load();
        }

        private void OnClose()
        {
            try
            {
                _settings.State = new PlayerState()
                {
                    Repeat = Playback.Repeat,
                    Shuffle = Playback.Shuffle,
                    Volume = Playback.Volume,
                    SelectedBeatmapId = Playback.Beatmap?.BeatmapSetId,
                    IsPlaying = Playback.PlaybackState == NAudio.Wave.PlaybackState.Playing,
                    Position = Playback.Position
                };

                _settingsManager.Save(_settings);
                _playlistProvider.Save(Model.Playlists);
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
            _discordManager.Enabled = enabled;

            if (!enabled)
                _discordManager.ClearPresence();
        }

        private void Settings_CultureChanged(string culture)
        {
            _localizationManager.Culture = LocalizationFactory.GetCulture(culture);
        }
        #endregion

        #region General Methods
        private void LoadState()
        {
            Playback.Shuffle = _settings.State.Shuffle;
            Playback.Repeat = _settings.State.Repeat;
            Playback.Volume = _settings.State.Volume;

            // TODO: Find a better solution
            Model.PlaybackInitializationRequired = _settings.State.SelectedBeatmapId.HasValue;
        }

        private void LoadSavedPlayback()
        {
            var beatmap = Model.Beatmaps.Where(x => x.BeatmapSetId == _settings.State.SelectedBeatmapId).FirstOrDefault();

            if (beatmap != null)
            {
                Playback.Beatmap = beatmap;
                Playback.Queue = Model.Beatmaps;
                Playback.Load();
                Playback.Position = _settings.State.Position;

                if (_settings.State.IsPlaying)
                    Playback.Resume();
            }
        }
        #endregion

        #region Hotkeys
        private void HotkeyManager_HotkeyChanged()
        {
            _settings.Hotkeys = _hotkeyManager.Hotkeys;
            _settingsManager.Save(_settings);
        }
        #endregion
    }
}
