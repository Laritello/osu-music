using Osu.Music.Common.Models;
using Osu.Music.Services.UItility;

namespace Osu.Music.Services.Search.Queries
{
    public class GeneralQuery : SearchQuery
    {
        public GeneralQuery(string value) : base(value)
        {
        }

        public override bool Check(Beatmap beatmap)
        {
            if (beatmap == null)
                return false;

            return beatmap.Title.ContainsLower(Value)
                || beatmap.TitleUnicode.ContainsLower(Value)
                || beatmap.Artist.ContainsLower(Value)
                || beatmap.ArtistUnicode.ContainsLower(Value);
        }
    }
}
