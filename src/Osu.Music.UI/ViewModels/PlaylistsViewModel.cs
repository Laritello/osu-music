using DryIoc;
using Osu.Music.Common;
using Osu.Music.Common.Models;
using Osu.Music.Services.Dialog;
using Osu.Music.Services.Interfaces;
using Osu.Music.Services.IO;
using Osu.Music.UI.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.Generic;

namespace Osu.Music.UI.ViewModels
{
    public class PlaylistsViewModel : BindableBase
    {
        private ICollection<Playlist> _playlists;
        /// <summary>
        /// Collection of user-created playlists.
        /// </summary>
        public ICollection<Playlist> Playlists
        {
            get => _playlists;
            set => SetProperty(ref _playlists, value);
        }

        public DelegateCommand<Playlist> SelectPlaylistCommand { get; private set; }

        private IPlaylistManager _playlistManager;
        private IRegionManager _regionManager;

        public PlaylistsViewModel(IContainer container)
        {
            _playlistManager = container.Resolve<IPlaylistManager>();
            _regionManager = container.Resolve<IRegionManager>();

            Playlists = _playlistManager.Playlists;
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
                var parameters = new NavigationParameters()
                {
                    { "playlist", playlist }
                };

                _regionManager.RequestNavigate(RegionNames.ContentRegion, "PlaylistDetailsView", parameters);
            }
        }
    }
}
