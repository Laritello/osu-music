using Osu.Music.Common.Models;
using Osu.Music.Services.Audio;
using Osu.Music.Services.Dialogs;
using Osu.Music.UI.ViewModels.Dialogs;
using Osu.Music.UI.Views.Dialogs;
using Prism.Commands;
using Prism.Dialogs;
using Prism.Mvvm;
using Prism.Navigation.Regions;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Osu.Music.UI.ViewModels
{
	public class LibraryViewModel : BindableBase, INavigationAware
	{
		private ObservableCollection<Beatmap> _beatmaps = [];
		/// <summary>
		/// Displayed beatmaps
		/// </summary>
		public ObservableCollection<Beatmap> Beatmaps
		{
			get => _beatmaps;
			set => SetProperty(ref _beatmaps, value);
		}

		private Beatmap _target;
		/// <summary>
		/// Used for navigation purposes
		/// </summary>
		public Beatmap Target
		{
			get => _target;
			set => SetProperty(ref _target, value);
		}

		private AudioPlayback _playback;
		public AudioPlayback Playback
		{
			get => _playback;
			set => SetProperty(ref _playback, value);
		}

		public DelegateCommand<Beatmap> PlayBeatmapCommand { get; private set; }
		public DelegateCommand<Beatmap> OpenBeatmapInBrowserCommand { get; private set; }
		public DelegateCommand<Beatmap> AddToPlaylistCommand { get; private set; }

		private readonly IPopupDialogService _dialogService;

		public LibraryViewModel(IPopupDialogService dialogService, AudioPlayback playback)
		{
			_dialogService = dialogService;
			_playback = playback;

			InitializeCommands();
		}

		private void InitializeCommands()
		{
			PlayBeatmapCommand = new DelegateCommand<Beatmap>(PlayBeatmap);
			OpenBeatmapInBrowserCommand = new DelegateCommand<Beatmap>(OpenBeatmapInBrowser);
			AddToPlaylistCommand = new DelegateCommand<Beatmap>(AddToPlaylist);
		}

		private void PlayBeatmap(Beatmap beatmap)
		{
			if (Playback.Queue != Beatmaps)
				Playback.Queue = Beatmaps;

			Playback.Beatmap = beatmap;
			Playback.Play();
		}

		private void OpenBeatmapInBrowser(Beatmap beatmap) => Process.Start(new ProcessStartInfo("cmd", $"/c start https://osu.ppy.sh/beatmapsets/{beatmap.BeatmapSetId}") { CreateNoWindow = true });

		private void AddToPlaylist(Beatmap beatmap)
		{
			DialogParameters parameters = new()
			{
				{ "beatmap", beatmap }
			};

			_dialogService.ShowPopupDialog<AddToPlaylistView, AddToPlaylistViewModel>("name", parameters, e =>
			{
				if (e.Result == ButtonResult.OK)
				{
					var playlist = e.Parameters.GetValue<Playlist>("playlist");
					var beatmap = e.Parameters.GetValue<Beatmap>("beatmap");
					playlist.Beatmaps.Add(beatmap);
				}
			});
		}

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			Beatmaps = navigationContext.Parameters.GetValue<ObservableCollection<Beatmap>>("beatmaps");
			Target = navigationContext.Parameters.ContainsKey("target") ? navigationContext.Parameters.GetValue<Beatmap>("target") : null;
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			var collection = navigationContext.Parameters.GetValue<ObservableCollection<Beatmap>>("beatmaps");
			return collection.Equals(Beatmaps);
		}

		public void OnNavigatedFrom(NavigationContext navigationContext) { }
	}
}
