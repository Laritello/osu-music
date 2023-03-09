using Osu.Music.Common.Models;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Osu.Music.UI.Models
{
    public class CollectionsModel : BindableBase
    {
        private ObservableCollection<Collection> _collections;
        /// <summary>
        /// List of collections imported from osu!
        /// </summary>
        public ObservableCollection<Collection> Collections
        {
            get => _collections;
            set => SetProperty(ref _collections, value);
        }
    }
}
