using DryIoc;
using Osu.Music.Common;
using Osu.Music.Common.Models;
using Osu.Music.UI.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;

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

        private IRegionManager _regionManager;

        public PlaylistsViewModel(IContainer container)
        {
            _regionManager = container.Resolve<IRegionManager>();

            Model = new PlaylistsModel();

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            SelectPlaylistCommand = new DelegateCommand<Playlist>(SelectPlaylist);
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

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var playlists = navigationContext.Parameters.GetValue<ObservableCollection<Playlist>>("playlists");
            return playlists.Equals(Model.Playlists);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Model.Playlists = navigationContext.Parameters.GetValue<ObservableCollection<Playlist>>("playlists");
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
    }
}
