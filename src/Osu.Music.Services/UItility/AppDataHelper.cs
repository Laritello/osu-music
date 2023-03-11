﻿using Newtonsoft.Json;
using Osu.Music.Common.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Osu.Music.Services.UItility
{
    public static class AppDataHelper
    {
        private static readonly string AppName = "osu.Music";
        public static string Path => GetPath();
        public static string PlaylistDirectory => GetPlaylistsDirectory();

        private static string GetPath()
        {
            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        public static ObservableCollection<LicenseNotice> GetLicenses()
        {
            string file = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\Static\licenses.json");

            string json = File.Exists(file) ? File.ReadAllText(file) : null;
            return json != null ? JsonConvert.DeserializeObject<ObservableCollection<LicenseNotice>>(json) : null;
        }

        public static string GetPlaylistsDirectory()
        {
            string path = System.IO.Path.Combine(Path, "playlists");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }
    }
}
