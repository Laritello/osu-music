using Osu.Music.Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Osu.Music.Services.Interfaces
{
    public interface IPlaylistManager
    {
        public ObservableCollection<Playlist> Playlists { get; }

        public Task<ObservableCollection<Playlist>> LoadAsync(IList<Beatmap> beatmaps);
        public void Save(Playlist playlist);
        public void Save(ICollection<Playlist> playlists);
        public void Remove(Playlist playlist);
        public void RemoveByName(string name);
    }
}
