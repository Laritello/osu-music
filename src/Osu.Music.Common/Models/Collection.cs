using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Osu.Music.Common.Models
{
    public class Collection : BindableBase
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
    }
}
