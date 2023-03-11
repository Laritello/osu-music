using System.Text.RegularExpressions;

namespace Osu.Music.Common.Interfaces
{
    public interface ISearchable
    {
        public bool Match(Regex query);
        public string GetNavigationView();
        public int Matches { get; }
    }
}
