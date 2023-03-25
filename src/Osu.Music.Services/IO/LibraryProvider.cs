using Osu.Music.Common.Models;
using Osu.Music.Services.Interfaces;
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
    public class LibraryProvider : ILibraryProvider
    {
        private readonly Settings _settings;

        public LibraryProvider(SettingsProvider settingsProvider)
        {
            _settings = settingsProvider.Settings;
        }

        public ObservableCollection<Beatmap> Beatmaps { get; private set; }

        public Task<ObservableCollection<Beatmap>> LoadAsync() => Task.Run(() => Load());

        public ObservableCollection<Beatmap> Load()
        {
            try
            {
                var source = _settings.Source;

                if (!Directory.Exists(source))
                    throw new ArgumentException("The specified folder does not exist.");

                List<Beatmap> beatmaps = new List<Beatmap>();

                OsuDb db;

                using (FileStream stream = File.OpenRead($@"{source}\osu!.db"))
                    db = OsuDb.Read(stream);

                foreach (var beatmapSet in db.Beatmaps.GroupBy(x => x.BeatmapSetId))
                {
                    if (beatmapSet.Count() > 0)
                        beatmaps.Add(Convert(source, beatmapSet));
                }

                Beatmaps = new ObservableCollection<Beatmap>(beatmaps.OrderBy(x => x.Title));
            }
            catch
            {
                Beatmaps = new ObservableCollection<Beatmap>();
            }

            return Beatmaps;
        }

        private Beatmap Convert(string osuFolder, IGrouping<int, BeatmapEntry> set)
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

        private List<string> GetSetHashes(IGrouping<int, BeatmapEntry> set) => set.Select(x => x.BeatmapChecksum).ToList();
    }
}
