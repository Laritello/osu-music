using DryIoc;
using Osu.Music.Common;
using Osu.Music.Common.Models;
using Osu.Music.Services.Audio;
using Osu.Music.Services.Dialogs;
using Osu.Music.Services.Interfaces;
using Osu.Music.Services.Localization;
using Osu.Music.UI.ViewModels.Dialogs;
using Osu.Music.UI.Views.Dialogs;
using Prism.Commands;
using Prism.Dialogs;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Navigation.Regions;
using System.Diagnostics;
using System.Linq;

namespace Osu.Music.UI.ViewModels
{
	public class PlaylistDetailsViewModel : BindableBase, INavigationAware
	{
		private Playlist _playlist;
		public Playlist Playlist
		{
			get => _playlist;
			set => SetProperty(ref _playlist, value);
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

		public DelegateCommand LaunchPlaylistCommand { get; private set; }
		public DelegateCommand DeleteCommand { get; private set; }
		public DelegateCommand EditNameCommand { get; private set; }
		public DelegateCommand<Beatmap> PlayBeatmapCommand { get; private set; }
		public DelegateCommand<Beatmap> OpenBeatmapInBrowserCommand { get; private set; }
		public DelegateCommand<Beatmap> RemoveFromPlaylistCommand { get; private set; }

		private readonly IPopupDialogService _dialogService;
		private readonly IPlaylistProvider _playlistProvider;
		private readonly IRegionManager _regionManager;
		private readonly LocalizationManager _localizationManager;

		public PlaylistDetailsViewModel(IPopupDialogService dialogService, IContainer container)
		{
			_dialogService = dialogService;

			_playlistProvider = container.Resolve<IPlaylistProvider>();
			_regionManager = container.Resolve<IRegionManager>();
			_playback = container.Resolve<AudioPlayback>();
			_localizationManager = LocalizationManager.Instance;

			InitializeCommands();
		}

		private void InitializeCommands()
		{
			LaunchPlaylistCommand = new DelegateCommand(LaunchPlaylist);
			DeleteCommand = new DelegateCommand(Delete);
			EditNameCommand = new DelegateCommand(EditName);
			PlayBeatmapCommand = new DelegateCommand<Beatmap>(PlayBeatmap);
			OpenBeatmapInBrowserCommand = new DelegateCommand<Beatmap>(OpenBeatmapInBrowser);
			RemoveFromPlaylistCommand = new DelegateCommand<Beatmap>(RemoveFromPlaylist);
		}

		private void LaunchPlaylist()
		{
			if (Playlist != null && Playlist.Beatmaps.Count > 0)
			{
				_playback.Queue = Playlist.Beatmaps;
				_playback.Beatmap = Playlist.Beatmaps.FirstOrDefault();
				_playback.Play();
			}
		}

		private void Delete()
		{
			DialogParameters parameters = new()
			{
				{ "title", _localizationManager.GetLocalizedString("Strings.PlaylistDetailsView.DeleteDialog.Title") },
				{ "message", string.Format(_localizationManager.GetLocalizedString("Strings.PlaylistDetailsView.DeleteDialog.Message"), Playlist.Name) },
				{ "caption", _localizationManager.GetLocalizedString("Strings.PlaylistDetailsView.DeleteDialog.Caption") }
			};

			_dialogService.ShowPopupDialog<GenericConfirmationView, GenericConfirmationViewModel>(parameters, e =>
					 {
						 if (e.Result == ButtonResult.OK)
						 {
							 _playlistProvider.Playlists.Remove(Playlist);
							 _playlistProvider.Remove(Playlist);
							 _regionManager.RequestNavigate(
								 RegionNames.ContentRegion,
								 "PlaylistsView",
								 new NavigationParameters()
								 {
									 { "playlists", _playlistProvider.Playlists }
								 });
						 }
					 });
		}

		private void EditName()
		{
			DialogParameters parameters = new()
			{
				{ "title", _localizationManager.GetLocalizedString("Strings.PlaylistDetailsView.EditDialog.Title") },
				{ "caption", _localizationManager.GetLocalizedString("Strings.PlaylistDetailsView.EditDialog.Caption") },
				{ "name", Playlist.Name },
				{ "names", _playlistProvider.Playlists.Where(x => x != Playlist).Select(x => x.Name) }
			};

			_dialogService.ShowPopupDialog<ManagePlaylistNameView, ManagePlaylistNameViewModel>(parameters, e =>
			{
				if (e.Result == ButtonResult.OK)
				{
					var name = e.Parameters.GetValue<string>("name");
					Playlist.Name = name;
				}
			});
		}

		private void PlayBeatmap(Beatmap beatmap)
		{
			if (_playback.Queue != Playlist.Beatmaps)
				_playback.Queue = Playlist.Beatmaps;

			_playback.Beatmap = beatmap;
			_playback.Play();
		}

		private void OpenBeatmapInBrowser(Beatmap beatmap) => Process.Start(new ProcessStartInfo("cmd", $"/c start https://osu.ppy.sh/beatmapsets/{beatmap.BeatmapSetId}") { CreateNoWindow = true });

		private void RemoveFromPlaylist(Beatmap beatmap) => Playlist.Beatmaps.Remove(beatmap);

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			Playlist = navigationContext.Parameters.GetValue<Playlist>("playlist");
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			var playlist = navigationContext.Parameters.GetValue<Playlist>("playlist");
			return Playlist == playlist;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext) { }
	}
}
