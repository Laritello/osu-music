using Osu.Music.Common.Models;

namespace Osu.Music.Services.Search.Queries
{
    public abstract class SearchQuery
    {
        public string Value { get; set; }

        public SearchQuery(string value)
        {
            Value = value;
        }

        public abstract bool Check(Beatmap beatmap);

        public static SearchQuery FromString(string query)
        {
            var values = query.Trim().Split(':');

            if (values.Length < 2)
                return new GeneralQuery(values[0]);

            var type = values[0].ToLower();
            var value = values[1].Trim();

            return type switch
            {
                "title" => new TitleQuery(value),
                "artist" => new ArtistQuery(value),
                "creator" => new CreatorQuery(value),
                "mapper" => new CreatorQuery(value),
                "tag" => new TagQuery(value),
                "tags" => new TagQuery(value),
                _ => new GeneralQuery(query),
            };
        }
    }
}
