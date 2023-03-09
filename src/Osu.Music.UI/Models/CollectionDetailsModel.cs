using Osu.Music.Common.Models;
using Prism.Mvvm;

namespace Osu.Music.UI.Models
{
    public class CollectionDetailsModel : BindableBase
    {
        private Collection _collection;
        public Collection Collection
        {
            get => _collection;
            set => SetProperty(ref _collection, value);
        }

        private Beatmap _selectedBeatmap;
        public Beatmap SelectedBeatmap
        {
            get => _selectedBeatmap;
            set => SetProperty(ref _selectedBeatmap, value);
        }
    }
}
