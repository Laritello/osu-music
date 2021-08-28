using Osu.Music.Common.Models;
using Osu.Music.Services.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Osu.Music.Services.IO
{
    public static class LibraryLoader
    {
        public static async Task<IList<Beatmap>> LoadAsync(string path)
        {
            return await Task.Run(() =>
            {
                List<Beatmap> beatmaps = new List<Beatmap>();

                string[] directories = Directory.GetDirectories($@"{path}\Songs");

                foreach (string directory in directories)
                {
                    try
                    {
                        beatmaps.Add(LoadBeatmap(directory));
                    }
                    catch
                    {
                        continue;
                    }
                }

                return beatmaps;
            });
        }

        private static Beatmap LoadBeatmap(string directory)
        {
            string[] files = Directory.GetFiles(directory, "*.osu");

            if (files == null || files.Length == 0)
                throw new Exception("Folder missing beatmap file");

            Beatmap result = BeatmapConverter.Deserialize(files[0]);
            result.Directory = directory;

            return result;
        }
    }
}
