using Osu.Music.Common.Models;
using Osu.Music.Services.UItility;

namespace Osu.Music.Services.Search.Queries
{
    public class ArtistQuery : SearchQuery
    {
        public ArtistQuery(string value) : base(value)
        {
        }

        public override bool Check(Beatmap beatmap)
        {
            if (beatmap == null)
                return false;

            return beatmap.Artist.ContainsLower(Value)
                || beatmap.ArtistUnicode.ContainsLower(Value);
        }
    }
}
