using Osu.Music.Common.Models;
using Osu.Music.Services.Interfaces;
using osu_database_reader.BinaryFiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Osu.Music.Services.IO
{
    public class CollectionManager : ICollectionManager
    {
        public ObservableCollection<Collection> Collections { get; private set; }

        public Task<ObservableCollection<Collection>> LoadAsync(string osuFolder, IList<Beatmap> beatmaps)
        {
            return Task.Run(() =>
            {
                try
                {
                    if (!Directory.Exists(osuFolder))
                        throw new ArgumentException("The specified folder does not exist.");

                    ObservableCollection<Collection> collections = new ObservableCollection<Collection>();

                    CollectionDb db;

                    using (FileStream stream = File.OpenRead($@"{osuFolder}\collection.db"))
                        db = CollectionDb.Read(stream);

                    foreach (var record in db.Collections)
                        collections.Add(Convert(record, beatmaps));

                    Collections = collections;
                }
                catch
                {
                    Collections = new ObservableCollection<Collection>();
                }

                return Collections;
            });
        }

        private Collection Convert(osu_database_reader.Components.Beatmaps.Collection collection, IList<Beatmap> beatmaps)
        {
            return new Collection()
            {
                Name = collection.Name,
                Beatmaps = GetCollectionBeatmaps(beatmaps, collection.BeatmapHashes)
            };
        }

        private ObservableCollection<Beatmap> GetCollectionBeatmaps(IList<Beatmap> beatmaps, List<string> hashes)
        {
            return new ObservableCollection<Beatmap>(beatmaps.Where(x => hashes.Any(y => x.Hashes.Contains(y))));
        }
    }
}
