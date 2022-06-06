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
        public static async Task<ICollection<Playlist>> LoadAsync(ICollection<Beatmap> beatmaps)
        {
            return await Task.Run(() =>
            {
                var playlistDirectory = AppDataHelper.PlaylistDirectory;

                if (!Directory.Exists(playlistDirectory))
                    throw new ArgumentException("The specified folder does not exist.");

                var playlists = Directory.GetFiles(playlistDirectory)
                .Where(x => x.Contains("json"))
                .Select(file => ConvertPlaylistFromJson(file, beatmaps))
                .Where(x => x != null)
                .ToList();

                return playlists;
            });
        }

        public static void Save(ICollection<Playlist> playlists)
        {
            foreach (var playlist in playlists)
            {
                try
                {
                    var _playlistFile = Path.Combine(AppDataHelper.PlaylistDirectory, $"{playlist.Name}.json");
                    string json = JsonConvert.SerializeObject(playlist);
                    File.WriteAllText(_playlistFile, json);
                }
                catch { }
            }
        }

        private static Playlist ConvertPlaylistFromJson(string filePath, ICollection<Beatmap> beatmaps)
        {
            try
            {
                var playlist = JsonConvert.DeserializeObject<Playlist>(filePath);
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
