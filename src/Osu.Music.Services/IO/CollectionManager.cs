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

        private readonly ILibraryManager _libraryManager;
        private readonly Settings _settings;

        public CollectionManager(ILibraryManager libraryManager, SettingsManager settingsManager)
        {
            _libraryManager = libraryManager;
            _settings = settingsManager.Settings;
        }

        public Task<ObservableCollection<Collection>> LoadAsync()
        {
            return Task.Run(async () =>
            {
                try
                {
                    var source = _settings.Source;

                    if (!Directory.Exists(source))
                        throw new ArgumentException("The specified folder does not exist.");

                    var beatmaps = await _libraryManager.LoadAsync();

                    List<Collection> collections = new List<Collection>();
                    CollectionDb db;

                    using (FileStream stream = File.OpenRead($@"{source}\collection.db"))
                        db = CollectionDb.Read(stream);

                    foreach (var record in db.Collections)
                        collections.Add(Convert(record, beatmaps));

                    Collections = new ObservableCollection<Collection>(collections);
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
