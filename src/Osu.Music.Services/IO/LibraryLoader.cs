using Osu.Music.Common.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using osu_database_reader.BinaryFiles;
using osu_database_reader.Components.Beatmaps;
using System;
using System.Linq;

namespace Osu.Music.Services.IO
{
    public static class LibraryLoader
    {

        public static async Task<IList<Beatmap>> LoadAsync()
        {
            return await Task.Run(() =>
            {
                List<Beatmap> beatmaps = new List<Beatmap>();

                OsuDb db;

                using (var stream = File.OpenRead(@"D:\Games\osu!\osu!.db"))
                    db = OsuDb.Read(stream);

                foreach (var beatmap in db.Beatmaps.DistinctBy(x => x.BeatmapSetId).OrderBy(x => x.Title))
                    beatmaps.Add(Convert(beatmap));

                return beatmaps;
            });
        }

        private static Beatmap Convert(BeatmapEntry entry)
        {
            return new Beatmap()
            {
                BeatmapSetID = entry.BeatmapSetId,
                Title = entry.Title,
                TitleUnicode = entry.TitleUnicode,
                Artist = entry.Artist,
                ArtistUnicode = entry.ArtistUnicode,
                Creator = entry.Creator,
                AudioFileName = entry.AudioFileName,
                TotalTime = TimeSpan.FromMilliseconds(entry.TotalTime),
                Tags = entry.SongTags,
                Directory = $@"D:\Games\osu!\Songs\{entry.FolderName}"
            };
        }

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
