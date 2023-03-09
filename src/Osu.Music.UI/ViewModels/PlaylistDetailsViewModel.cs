using DryIoc;
using Osu.Music.Common;
using Osu.Music.Common.Models;
using Osu.Music.Services.Audio;
using Osu.Music.Services.Dialog;
using Osu.Music.Services.Interfaces;
using Osu.Music.UI.Models;
using Osu.Music.UI.ViewModels.Dialogs;
using Osu.Music.UI.Views.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Diagnostics;
using System.Linq;

namespace Osu.Music.UI.ViewModels
{
    public class PlaylistDetailsViewModel : BindableBase, INavigationAware
    {
        private PaylistDetailsModel _model;
        public PaylistDetailsModel Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        public DelegateCommand DeleteCommand { get; private set; }
        public DelegateCommand EditNameCommand { get; private set; }
        public DelegateCommand<Beatmap> PlayBeatmapCommand { get; private set; }
        public DelegateCommand<Beatmap> OpenBeatmapInBrowserCommand { get; private set; }
        public DelegateCommand<Beatmap> RemoveFromPlaylistCommand { get; private set; }

        private IPopupDialogService _dialogService;
        private IPlaylistManager _playlistManager;
        private IRegionManager _regionManager;
        private AudioPlayback _playback;

        public PlaylistDetailsViewModel(IContainer container)
        {
            _dialogService = container.Resolve<IPopupDialogService>();
            _playlistManager = container.Resolve<IPlaylistManager>();
            _regionManager = container.Resolve<IRegionManager>();
            _playback = container.Resolve<AudioPlayback>();

            Model = new PaylistDetailsModel();

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            DeleteCommand = new DelegateCommand(Delete);
            EditNameCommand = new DelegateCommand(EditName);
            PlayBeatmapCommand = new DelegateCommand<Beatmap>(PlayBeatmap);
            OpenBeatmapInBrowserCommand = new DelegateCommand<Beatmap>(OpenBeatmapInBrowser);
            RemoveFromPlaylistCommand = new DelegateCommand<Beatmap>(RemoveFromPlaylist);
        }

        private void Delete()
        {
            DialogParameters parameters = new DialogParameters()
            {
                { "title", "Delete playlist" },
                { "message", $"Are you sure you want to delete {Model.Playlist.Name}?\r\nThis action cannot be undone." },
                { "caption", "DELETE" }
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
                { "title", "Edit playlist" },
                { "caption", "EDIT" },
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

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var playlist = navigationContext.Parameters.GetValue<Playlist>("playlist");
            return Model.Playlist == playlist;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Model.Playlist = navigationContext.Parameters.GetValue<Playlist>("playlist");
        }
    }
}
