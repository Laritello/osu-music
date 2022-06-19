using Osu.Music.Common.Models;
using Osu.Music.Services.Search.Queries;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Osu.Music.Services.Search
{
    public static class BeatmapSearch
    {
        public static ObservableCollection<Beatmap> Search(ICollection<Beatmap> source, string query)
        {
            var queries = BuildQueries(query);

            if (queries.Count == 0)
                return new ObservableCollection<Beatmap>();

            return new ObservableCollection<Beatmap>(source.Where(x => queries.All(q => q.Check(x))));
        }

        private static ICollection<SearchQuery> BuildQueries(string query)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                    return new List<SearchQuery>();

                var values = query.Split(' ');
                var requests = new List<string>();

                List<string> tags = new List<string>() { "title", "artist", "creator", "mapper" };

                List<int> indexes = new List<int>();

                foreach (var tag in tags)
                {
                    var index = query.IndexOf($"{tag}:");
                    if (index > -1) indexes.Add(index);
                }

                if (!indexes.Contains(0))
                    indexes.Add(0);

                indexes.Sort();

                var result = new List<SearchQuery>();

                for (int i = 0; i < indexes.Count; i++)
                {
                    string request;

                    if (i == indexes.Count - 1)
                    {
                        request = query.Substring(indexes[i]);
                        result.Add(SearchQuery.FromString(request));
                        break;
                    }

                    request = query.Substring(indexes[i], indexes[i + 1] - indexes[i]);
                    result.Add(SearchQuery.FromString(request));
                }

                return result;
            }
            catch
            {
                return new List<SearchQuery>();
            }
        }
    }
}
