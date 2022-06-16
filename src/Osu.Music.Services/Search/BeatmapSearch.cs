using Osu.Music.Common.Models;
using Osu.Music.Services.Search.Queries;
using System.Collections.Generic;
using System.Linq;

namespace Osu.Music.Services.Search
{
    public static class BeatmapSearch
    {
        public static ICollection<Beatmap> Search(ICollection<Beatmap> source, string query)
        {
            var queries = BuildQueries(query);

            if (queries.Count == 0)
                return new List<Beatmap>();

            return source.Where(x => queries.All(q => q.Check(x))).ToList();
        }
        //dragonforce artist:alan walker title: (Vocal) You

        private static ICollection<SearchQuery> BuildQueries(string query)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                    return new List<SearchQuery>();

                var values = query.Split(' ');
                var requests = new List<string>();

                List<string> tags = new List<string>() { "title", "artist" };

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
