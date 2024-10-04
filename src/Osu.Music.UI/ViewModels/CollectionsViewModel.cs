using Osu.Music.Common;
using Osu.Music.Common.Models;
using Osu.Music.Services.Audio;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Navigation.Regions;
using System.Collections.ObjectModel;
using System.Linq;

namespace Osu.Music.UI.ViewModels
{
	public class CollectionsViewModel : BindableBase, INavigationAware
	{
		private ObservableCollection<Collection> _collections = [];
		/// <summary>
		/// List of collections imported from osu!
		/// </summary>
		public ObservableCollection<Collection> Collections
		{
			get => _collections;
			set => SetProperty(ref _collections, value);
		}

		public DelegateCommand<Collection> SelectCollectionCommand { get; private set; }
		public DelegateCommand<Collection> LaunchCollectionCommand { get; private set; }

		private readonly IRegionManager _regionManager;
		private readonly AudioPlayback _playback;

		public CollectionsViewModel(IRegionManager regionManager, AudioPlayback playback)
		{
			_regionManager = regionManager;
			_playback = playback;

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

			if (Collections != collections)
				Collections = collections;
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			var collections = navigationContext.Parameters.GetValue<ObservableCollection<Collection>>("collections");
			return collections.Equals(Collections);
		}

		public void OnNavigatedFrom(NavigationContext navigationContext) { }
	}
}
