using Osu.Music.Common.Models;
using Osu.Music.Services.UItility;

namespace Osu.Music.Services.Search.Queries
{
    public class TitleQuery : SearchQuery
    {
        public TitleQuery(string value) : base(value)
        {
        }

        public override bool Check(Beatmap beatmap)
        {
            if (beatmap == null)
                return false;

            return beatmap.Title.ContainsLower(Value)
                || beatmap.TitleUnicode.ContainsLower(Value);
        }
    }
}
