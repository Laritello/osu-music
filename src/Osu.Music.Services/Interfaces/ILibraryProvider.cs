using Osu.Music.Common.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Osu.Music.Services.Interfaces
{
    public interface ILibraryProvider
    {
        public ObservableCollection<Beatmap> Beatmaps { get; }
        public Task<ObservableCollection<Beatmap>> LoadAsync();
        public ObservableCollection<Beatmap> Load();
    }
}
