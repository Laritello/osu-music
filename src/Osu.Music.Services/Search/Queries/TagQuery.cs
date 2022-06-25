using Osu.Music.Common.Models;
using Osu.Music.Services.UItility;
using System.Collections.Generic;

namespace Osu.Music.Services.Search.Queries
{
    public class TagQuery : SearchQuery
    {
        private ICollection<string> _tags;

        public TagQuery(string value) : base(value)
        {
            _tags = new List<string>();

            foreach (var tag in value.Split(','))
                _tags.Add(tag.Trim());
        }

        public override bool Check(Beatmap beatmap)
        {
            if (beatmap == null)
                return false;

            foreach (var tag in _tags)
            {
                if (beatmap.Tags.ContainsLower(tag))
                    return true;
            }

            return false;
        }
    }
}
