using Osu.Music.Common.Models;
using Prism.Mvvm;
using System.Collections.Generic;

namespace Osu.Music.UI.ViewModels
{
    public class CollectionsViewModel : BindableBase
    {
        private ICollection<Collection> _collections;
        /// <summary>
        /// Collection of user-created playlists.
        /// </summary>
        public ICollection<Collection> Collections
        {
            get => _collections;
            set => SetProperty(ref _collections, value);
        }

        public CollectionsViewModel(ICollection<Collection> collections)
        {
            Collections = collections;
        }
    }
}
