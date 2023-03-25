using Osu.Music.Common.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Osu.Music.Services.Interfaces
{
    public interface IPlaylistProvider
    {
        public ObservableCollection<Playlist> Playlists { get; }

        public Task<ObservableCollection<Playlist>> LoadAsync();
        public ObservableCollection<Playlist> Load();
        public void Save(Playlist playlist);
        public void Save(ICollection<Playlist> playlists);
        public void Remove(Playlist playlist);
        public void RemoveByName(string name);
    }
}
