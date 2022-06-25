using Osu.Music.Common.Models;
using osu_database_reader.BinaryFiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Osu.Music.Services.IO
{
    public static class CollectionManager
    {
        public static async Task<ObservableCollection<Collection>> LoadAsync(string osuFolder, IList<Beatmap> beatmaps)
        {
            return await Task.Run(() =>
            {
                if (!Directory.Exists(osuFolder))
                    throw new ArgumentException("The specified folder does not exist.");

                ObservableCollection<Collection> collections = new ObservableCollection<Collection>();

                CollectionDb db;

                using (FileStream stream = File.OpenRead($@"{osuFolder}\collection.db"))
                    db = CollectionDb.Read(stream);

                foreach (var record in db.Collections)
                    collections.Add(Convert(record, beatmaps));

                return new ObservableCollection<Collection>(collections);
            });
        }

        private static Collection Convert(osu_database_reader.Components.Beatmaps.Collection collection, IList<Beatmap> beatmaps)
        {
            return new Collection()
            {
                Name = collection.Name,
                Beatmaps = GetCollectionBeatmaps(beatmaps, collection.BeatmapHashes)
            };
        }

        private static ObservableCollection<Beatmap> GetCollectionBeatmaps(IList<Beatmap> beatmaps, List<string> hashes)
        {
            return new ObservableCollection<Beatmap>(beatmaps.Where(x => hashes.Any(y => x.Hashes.Contains(y))));
        }
    }
}
