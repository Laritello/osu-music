using Newtonsoft.Json;
using Osu.Music.Common.Models;
using Osu.Music.Services.UItility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Osu.Music.Services.IO
{
    public static class PlaylistManager
    {
        public static async Task<ObservableCollection<Playlist>> LoadAsync(IList<Beatmap> beatmaps)
        {
            return await Task.Run(() =>
            {
                var playlistDirectory = AppDataHelper.PlaylistDirectory;

                if (!Directory.Exists(playlistDirectory))
                    throw new ArgumentException("The specified folder does not exist.");

                var s = Directory.GetFiles(playlistDirectory);
                var playlists = Directory.GetFiles(playlistDirectory)
                .Where(x => x.Contains("json"))
                .Select(file => ConvertPlaylistFromJson(file, beatmaps))
                .Where(x => x != null)
                .ToList();

                return new ObservableCollection<Playlist>(playlists);
            });
        }

        public static void Save(ICollection<Playlist> playlists)
        {
            // TODO: More elegant way of tracking removed playlists
            foreach (var file in Directory.GetFiles(AppDataHelper.PlaylistDirectory))
                File.Delete(file);

            foreach (var playlist in playlists)
                Save(playlist);
        }

        public static void Save(Playlist playlist)
        {
            try
            {
                var _playlistFile = Path.Combine(AppDataHelper.PlaylistDirectory, $"{playlist.Name}.json");
                string json = JsonConvert.SerializeObject(playlist);
                File.WriteAllText(_playlistFile, json);
            }
            catch { }
        }

        public static void Remove(Playlist playlist)
        {
            if (playlist == null)
                return;

            RemoveByName(playlist.Name);
        }

        public static void RemoveByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            try
            {
                var _playlistFile = Path.Combine(AppDataHelper.PlaylistDirectory, $"{name}.json");
                File.Delete(_playlistFile);
            }
            catch { }
        }

        private static Playlist ConvertPlaylistFromJson(string filePath, IList<Beatmap> beatmaps)
        {
            try
            {
                string json = File.Exists(filePath) ? File.ReadAllText(filePath) : null;
                var playlist = json != null ? JsonConvert.DeserializeObject<Playlist>(json) : null;
                playlist?.UpdateMaps(beatmaps);

                return playlist;
            }
            catch
            {
                return null;
            }
        }
    }
}
