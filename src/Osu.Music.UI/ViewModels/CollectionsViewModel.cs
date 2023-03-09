using DryIoc;
using Osu.Music.Common;
using Osu.Music.Common.Models;
using Osu.Music.Services.Audio;
using Osu.Music.UI.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Osu.Music.UI.ViewModels
{
    public class CollectionsViewModel : BindableBase, INavigationAware
    {
        private CollectionsModel _model;
        public CollectionsModel Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        public DelegateCommand<Collection> SelectCollectionCommand { get; private set; }
        public DelegateCommand<Collection> LaunchCollectionCommand { get; private set; }

        private IRegionManager _regionManager;
        private AudioPlayback _playback;

        public CollectionsViewModel(IContainer container)
        {
            _regionManager = container.Resolve<IRegionManager>();
            _playback = container.Resolve<AudioPlayback>();

            Model = new CollectionsModel();

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            SelectCollectionCommand = new DelegateCommand<Collection>(SelectCollection);
            LaunchCollectionCommand = new DelegateCommand<Collection>(LaunchCollection);
        }

        private void SelectCollection(Collection collection)
        {
            if (collection != null)
            {
                _regionManager.RequestNavigate(
                    RegionNames.ContentRegion,
                    "CollectionDetailsView",
                    new NavigationParameters()
                    {
                        { "collection", collection }
                    });
            }
        }

        private void LaunchCollection(Collection collection)
        {
            if (collection != null && collection.Beatmaps.Count > 0)
            {
                _playback.Queue = collection.Beatmaps;
                _playback.Beatmap = collection.Beatmaps.FirstOrDefault();
                _playback.Play();
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var collections = navigationContext.Parameters.GetValue<ObservableCollection<Collection>>("collections");

            if (Model.Collections != collections)
                Model.Collections = collections;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var collections = navigationContext.Parameters.GetValue<ObservableCollection<Collection>>("collections");
            return collections.Equals(Model.Collections);
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
    }
}
