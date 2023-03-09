using DryIoc;
using Osu.Music.Common;
using Osu.Music.Common.Models;
using Osu.Music.Services.Audio;
using Osu.Music.UI.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;

namespace Osu.Music.UI.ViewModels
{
    public class PlaylistsViewModel : BindableBase, INavigationAware
    {
        private PlaylistsModel _model;
        public PlaylistsModel Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        public DelegateCommand<Playlist> SelectPlaylistCommand { get; private set; }
        public DelegateCommand<Playlist> LaunchPlaylistCommand { get; private set; }

        private IRegionManager _regionManager;
        private AudioPlayback _playback;

        public PlaylistsViewModel(IContainer container, PlaylistsModel model)
        {
            _regionManager = container.Resolve<IRegionManager>();
            _playback = container.Resolve<AudioPlayback>();
            _model = model;

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            SelectPlaylistCommand = new DelegateCommand<Playlist>(SelectPlaylist);
            LaunchPlaylistCommand = new DelegateCommand<Playlist>(LaunchPlaylist);
        }

        private void SelectPlaylist(Playlist playlist)
        {
            if (playlist != null)
            {
                _regionManager.RequestNavigate(
                    RegionNames.ContentRegion, 
                    "PlaylistDetailsView", 
                    new NavigationParameters()
                    {
                        { "playlist", playlist }
                    });
            }
        }

        private void LaunchPlaylist(Playlist playlist)
        {
            if (playlist != null && playlist.Beatmaps.Count > 0)
            {
                _playback.Queue = playlist.Beatmaps;
                _playback.Beatmap = playlist.Beatmaps.FirstOrDefault();
                _playback.Play();
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var playlists = navigationContext.Parameters.GetValue<ObservableCollection<Playlist>>("playlists");

            if (Model.Playlists != playlists)
                Model.Playlists = playlists;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var playlists = navigationContext.Parameters.GetValue<ObservableCollection<Playlist>>("playlists");
            return playlists.Equals(Model.Playlists);
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
    }
}
