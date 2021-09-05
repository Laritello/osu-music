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
    }
}
