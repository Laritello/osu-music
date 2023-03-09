using Osu.Music.Common.Models;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Osu.Music.UI.Models
{
    public class PlaylistsModel : BindableBase
    {
        private ObservableCollection<Playlist> _playlists;
        /// <summary>
        /// Collection of user-created playlists.
        /// </summary>
        public ObservableCollection<Playlist> Playlists
        {
            get => _playlists;
            set => SetProperty(ref _playlists, value);
        }

        public PlaylistsModel() { }
    }
}
