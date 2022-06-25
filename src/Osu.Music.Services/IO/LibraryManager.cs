using Osu.Music.Common.Models;
using osu_database_reader.BinaryFiles;
using osu_database_reader.Components.Beatmaps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Osu.Music.Services.IO
{
    public static class LibraryManager
    {
        public static async Task<ObservableCollection<Beatmap>> LoadAsync(string osuFolder)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (!Directory.Exists(osuFolder))
                        throw new ArgumentException("The specified folder does not exist.");

                    ObservableCollection<Beatmap> beatmaps = new ObservableCollection<Beatmap>();

                    OsuDb db;

                    using (FileStream stream = File.OpenRead($@"{osuFolder}\osu!.db"))
                        db = OsuDb.Read(stream);

                    foreach (var beatmapSet in db.Beatmaps.GroupBy(x => x.BeatmapSetId))
                    {
                        if (beatmapSet.Count() > 0)
                            beatmaps.Add(Convert(osuFolder, beatmapSet));
                    }

                    return beatmaps;
                }
                catch
                {
                    return new ObservableCollection<Beatmap>();
                }
            });
        }

        private static Beatmap Convert(string osuFolder, IGrouping<int, BeatmapEntry> set)
        {
            var entry = set.First();

            return new Beatmap()
            {
                BeatmapSetId = entry.BeatmapSetId,
                Title = entry.Title ?? string.Empty,
                TitleUnicode = entry.TitleUnicode ?? string.Empty,
                Artist = entry.Artist ?? string.Empty,
                ArtistUnicode = entry.ArtistUnicode ?? string.Empty,
                Creator = entry.Creator ?? string.Empty,
                AudioFileName = entry.AudioFileName ?? string.Empty,
                TotalTime = TimeSpan.FromMilliseconds(entry.TotalTime),
                Tags = entry.SongTags ?? string.Empty,
                Directory = $@"{osuFolder}\Songs\{entry.FolderName}",
                FileName = entry.BeatmapFileName ?? string.Empty,
                Hashes = GetSetHashes(set),
            };
        }

        private static List<string> GetSetHashes(IGrouping<int, BeatmapEntry> set) => set.Select(x => x.BeatmapChecksum).ToList();

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
