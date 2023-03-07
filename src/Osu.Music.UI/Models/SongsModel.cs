using Osu.Music.Common.Models;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Osu.Music.UI.Models
{
    public class SongsModel : BindableBase
    {
        private ObservableCollection<Beatmap> _beatmaps;
        /// <summary>
        /// Displayed beatmaps
        /// </summary>
        public ObservableCollection<Beatmap> Beatmaps
        {
            get => _beatmaps;
            set => SetProperty(ref _beatmaps, value);
        }
        public SongsModel() { }
    }
}
