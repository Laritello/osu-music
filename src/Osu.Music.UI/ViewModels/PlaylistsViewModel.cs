using DryIoc;
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
	public class PlaylistsViewModel : BindableBase, INavigationAware
	{
		private ObservableCollection<Playlist> _playlists;
		/// <summary>
		/// Collection of user-created playlists.
		/// </summary>
		public ObservableCollection<Playlist> Playlists
		{
			get => _playlists;
			set => SetProperty(ref _playlists, value);
		}

		public DelegateCommand<Playlist> SelectPlaylistCommand { get; private set; }
		public DelegateCommand<Playlist> LaunchPlaylistCommand { get; private set; }

		private IRegionManager _regionManager;
		private AudioPlayback _playback;

		public PlaylistsViewModel(IContainer container)
		{
			_regionManager = container.Resolve<IRegionManager>();
			_playback = container.Resolve<AudioPlayback>();

			InitializeCommands();
		}

		private void InitializeCommands()
		{
			SelectPlaylistCommand = new DelegateCommand<Playlist>(SelectPlaylist);
			LaunchPlaylistCommand = new DelegateCommand<Playlist>(LaunchPlaylist);
		}

		private void SelectPlaylist(Playlist playlist)
		{
			if (playlist != null)
			{
				_regionManager.RequestNavigate(
					RegionNames.ContentRegion,
					"PlaylistDetailsView",
					new NavigationParameters()
					{
						{ "playlist", playlist }
					});
			}
		}

		private void LaunchPlaylist(Playlist playlist)
		{
			if (playlist != null && playlist.Beatmaps.Count > 0)
			{
				_playback.Queue = playlist.Beatmaps;
				_playback.Beatmap = playlist.Beatmaps.FirstOrDefault();
				_playback.Play();
			}
		}

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			var playlists = navigationContext.Parameters.GetValue<ObservableCollection<Playlist>>("playlists");

			if (Playlists != playlists)
				Playlists = playlists;
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			var playlists = navigationContext.Parameters.GetValue<ObservableCollection<Playlist>>("playlists");
			return playlists.Equals(Playlists);
		}

		public void OnNavigatedFrom(NavigationContext navigationContext) { }
	}
}
