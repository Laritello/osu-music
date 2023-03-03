﻿using Newtonsoft.Json;
using Osu.Music.Common.Models;
using Osu.Music.Services.Interfaces;
using Osu.Music.Services.UItility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Osu.Music.Services.IO
{
    public class PlaylistManager : IPlaylistManager
    {
        public ObservableCollection<Playlist> Playlists { get; private set; }

        public Task<ObservableCollection<Playlist>> LoadAsync(IList<Beatmap> beatmaps)
        {
            return Task.Run(() =>
            {
                try
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

                    Playlists = new ObservableCollection<Playlist>(playlists);
                }
                catch
                {
                    Playlists = new ObservableCollection<Playlist>();
                }

                return Playlists;
            });
        }

        public void Save(ICollection<Playlist> playlists)
        {
            // TODO: More elegant way of tracking removed playlists
            foreach (var file in Directory.GetFiles(AppDataHelper.PlaylistDirectory))
                File.Delete(file);

            foreach (var playlist in playlists)
                Save(playlist);
        }

        public void Save(Playlist playlist)
        {
            try
            {
                var _playlistFile = Path.Combine(AppDataHelper.PlaylistDirectory, $"{playlist.Name}.json");
                string json = JsonConvert.SerializeObject(playlist);
                File.WriteAllText(_playlistFile, json);
            }
            catch { }
        }

        public void Remove(Playlist playlist)
        {
            if (playlist == null)
                return;

            RemoveByName(playlist.Name);
        }

        public void RemoveByName(string name)
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

        private Playlist ConvertPlaylistFromJson(string filePath, IList<Beatmap> beatmaps)
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
