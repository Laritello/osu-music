using DryIoc;
using Osu.Music.Common.Models;
using Osu.Music.Services.Audio;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation.Regions;
using System.Diagnostics;
using System.Linq;

namespace Osu.Music.UI.ViewModels
{
	internal class CollectionDetailsViewModel : BindableBase, INavigationAware
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

		private AudioPlayback _playback;
		public AudioPlayback Playback
		{
			get => _playback;
			set => SetProperty(ref _playback, value);
		}

		public DelegateCommand LaunchCollectionCommand { get; private set; }
		public DelegateCommand<Beatmap> PlayBeatmapCommand { get; private set; }
		public DelegateCommand<Beatmap> OpenBeatmapInBrowserCommand { get; private set; }

		public CollectionDetailsViewModel(AudioPlayback playback)
		{
			_playback = playback;

			InitializeCommands();
		}

		private void InitializeCommands()
		{
			LaunchCollectionCommand = new DelegateCommand(LaunchCollection);
			PlayBeatmapCommand = new DelegateCommand<Beatmap>(PlayBeatmap);
			OpenBeatmapInBrowserCommand = new DelegateCommand<Beatmap>(OpenBeatmapInBrowser);
		}

		private void LaunchCollection()
		{
			if (Collection != null && Collection.Beatmaps.Count > 0)
			{
				_playback.Queue = Collection.Beatmaps;
				_playback.Beatmap = Collection.Beatmaps.FirstOrDefault();
				_playback.Play();
			}
		}

		private void PlayBeatmap(Beatmap beatmap)
		{
			if (_playback.Queue != Collection.Beatmaps)
				_playback.Queue = Collection.Beatmaps;

			_playback.Beatmap = beatmap;
			_playback.Play();
		}

		private void OpenBeatmapInBrowser(Beatmap beatmap) => Process.Start(new ProcessStartInfo("cmd", $"/c start https://osu.ppy.sh/beatmapsets/{beatmap.BeatmapSetId}") { CreateNoWindow = true });

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			Collection = navigationContext.Parameters.GetValue<Collection>("collection");
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			var collection = navigationContext.Parameters.GetValue<Collection>("collection");
			return Collection == collection;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext) { }
	}
}
