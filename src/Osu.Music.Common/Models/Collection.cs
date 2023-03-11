using Newtonsoft.Json;
using Osu.Music.Common.Interfaces;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Osu.Music.Common.Models
{
    public class Collection : BindableBase, ISearchable
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private ObservableCollection<Beatmap> _beatmaps;
        public ObservableCollection<Beatmap> Beatmaps
        {
            get => _beatmaps;
            set => SetProperty(ref _beatmaps, value);
        }

        private int _matches;
        /// <summary>
        /// The amount of found matches during search.
        /// </summary>
        [JsonIgnore]
        public int Matches
        {
            get => _matches;
            private set => SetProperty(ref _matches, value);
        }

        public bool Match(Regex query)
        {
            Matches = query.Matches(Name).Count;
            return Matches > 0;
        }

        public string GetNavigationView() => "CollectionDetailsView";
    }
}
