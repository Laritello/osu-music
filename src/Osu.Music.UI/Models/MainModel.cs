using Osu.Music.Common.Models;
using Osu.Music.Services.IO;
using Prism.Mvvm;
using System.Collections.Generic;

namespace Osu.Music.UI.Models
{
    public class MainModel : BindableBase
    {
        private IEnumerable<Beatmap> _beatmaps;
        public IEnumerable<Beatmap> Beatmaps
        {
            get => _beatmaps;
            set => SetProperty(ref _beatmaps, value);
        }

        public MainModel()
        {
            LoadBeatmaps();
        }

        private async void LoadBeatmaps()
        {
            var beatmaps = await LibraryLoader.LoadAsync(@"D:\Games\osu!");
            Beatmaps = beatmaps;
        }
    }
}
