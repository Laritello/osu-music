using DryIoc;
using Osu.Music.Common;
using Osu.Music.Common.Models;
using Osu.Music.Services.Audio;
using Osu.Music.Services.Dialog;
using Osu.Music.Services.Interfaces;
using Osu.Music.Services.Localization;
using Osu.Music.UI.Models;
using Osu.Music.UI.ViewModels.Dialogs;
using Osu.Music.UI.Views.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Diagnostics;
using System.Linq;

namespace Osu.Music.UI.ViewModels
{
    public class PlaylistDetailsViewModel : BindableBase, INavigationAware
    {
        private PlaylistDetailsModel _model;
        public PlaylistDetailsModel Model
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

        public DelegateCommand LaunchPlaylistCommand { get; private set; }
        public DelegateCommand DeleteCommand { get; private set; }
        public DelegateCommand EditNameCommand { get; private set; }
        public DelegateCommand<Beatmap> PlayBeatmapCommand { get; private set; }
        public DelegateCommand<Beatmap> OpenBeatmapInBrowserCommand { get; private set; }
        public DelegateCommand<Beatmap> RemoveFromPlaylistCommand { get; private set; }

        private readonly IPopupDialogService _dialogService;
        private readonly IPlaylistManager _playlistManager;
        private readonly IRegionManager _regionManager;
        private readonly LocalizationManager _localizationManager;

        public PlaylistDetailsViewModel(IContainer container, PlaylistDetailsModel model)
        {
            _dialogService = container.Resolve<IPopupDialogService>();
            _playlistManager = container.Resolve<IPlaylistManager>();
            _regionManager = container.Resolve<IRegionManager>();
            _playback = container.Resolve<AudioPlayback>();
            _localizationManager = LocalizationManager.Instance;

            _model = model;

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            LaunchPlaylistCommand = new DelegateCommand(LaunchPlaylist);
            DeleteCommand = new DelegateCommand(Delete);
            EditNameCommand = new DelegateCommand(EditName);
            PlayBeatmapCommand = new DelegateCommand<Beatmap>(PlayBeatmap);
            OpenBeatmapInBrowserCommand = new DelegateCommand<Beatmap>(OpenBeatmapInBrowser);
            RemoveFromPlaylistCommand = new DelegateCommand<Beatmap>(RemoveFromPlaylist);
        }

        private void LaunchPlaylist()
        {
            if (Model.Playlist != null && Model.Playlist.Beatmaps.Count > 0)
            {
                _playback.Queue = Model.Playlist.Beatmaps;
                _playback.Beatmap = Model.Playlist.Beatmaps.FirstOrDefault();
                _playback.Play();
            }
        }

        private void Delete()
        {
            DialogParameters parameters = new DialogParameters()
            {
                { "title", _localizationManager.GetLocalizedString("Strings.PlaylistDetailsView.DeleteDialog.Title") },
                { "message", string.Format(_localizationManager.GetLocalizedString("Strings.PlaylistDetailsView.DeleteDialog.Message"), Model.Playlist.Name) },
                { "caption", _localizationManager.GetLocalizedString("Strings.PlaylistDetailsView.DeleteDialog.Caption") }
            };

            _dialogService.ShowPopupDialog<GenericConfirmationView, GenericConfirmationViewModel>(parameters, e =>
            {
                if (e.Result == ButtonResult.OK)
                {
                    _playlistManager.Playlists.Remove(Model.Playlist);
                    _playlistManager.Remove(Model.Playlist);
                    _regionManager.RequestNavigate(
                        RegionNames.ContentRegion, 
                        "PlaylistsView", 
                        new NavigationParameters()
                        {
                            { "playlists", _playlistManager.Playlists }
                        });
                }
            });
        }

        private void EditName()
        {
            DialogParameters parameters = new DialogParameters()
            {
                { "title", _localizationManager.GetLocalizedString("Strings.PlaylistDetailsView.EditDialog.Title") },
                { "caption", _localizationManager.GetLocalizedString("Strings.PlaylistDetailsView.EditDialog.Caption") },
                { "name", Model.Playlist.Name },
                { "names", _playlistManager.Playlists.Where(x => x != Model.Playlist).Select(x => x.Name) }
            };

            _dialogService.ShowPopupDialog<ManagePlaylistNameView, ManagePlaylistNameViewModel>(parameters, e =>
            {
                if (e.Result == ButtonResult.OK)
                {
                    var name = e.Parameters.GetValue<string>("name");
                    Model.Playlist.Name = name;
                }
            });
        }

        private void PlayBeatmap(Beatmap beatmap)
        {
            if (_playback.Queue != Model.Playlist.Beatmaps)
                _playback.Queue = Model.Playlist.Beatmaps;

            _playback.Beatmap = beatmap;
            _playback.Play();
        }

        private void OpenBeatmapInBrowser(Beatmap beatmap) => Process.Start(new ProcessStartInfo("cmd", $"/c start https://osu.ppy.sh/beatmapsets/{beatmap.BeatmapSetId}") { CreateNoWindow = true });

        private void RemoveFromPlaylist(Beatmap beatmap) => Model.Playlist.Beatmaps.Remove(beatmap);

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Model.Playlist = navigationContext.Parameters.GetValue<Playlist>("playlist");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var playlist = navigationContext.Parameters.GetValue<Playlist>("playlist");
            return Model.Playlist == playlist;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext) { }
    }
}
