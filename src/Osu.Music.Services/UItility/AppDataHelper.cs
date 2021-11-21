using Newtonsoft.Json;
using System;
using System.IO;

namespace Osu.Music.Services.UItility
{
    public static class AppDataHelper
    {
        private static readonly string AppName = "osu.Music";
        public static string Path => GetPath();

        private static string GetPath()
        {
            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppName);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        public static dynamic GetLicenses()
        {
            string file = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\Static\licenses.json");

            string json = File.Exists(file) ? File.ReadAllText(file) : null;
            return json != null ? JsonConvert.DeserializeObject(json) : null;
        }
    }
}
