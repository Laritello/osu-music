using Osu.Music.Common.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Osu.Music.Services.Interfaces
{
    public interface ICollectionManager
    {
        public ObservableCollection<Collection> Collections { get; }
        public Task<ObservableCollection<Collection>> LoadAsync();
        public ObservableCollection<Collection> Load();
    }
}
