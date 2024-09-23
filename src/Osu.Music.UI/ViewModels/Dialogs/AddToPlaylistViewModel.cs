using MaterialDesignThemes.Wpf;
using Osu.Music.Common.Models;
using Osu.Music.Services.Dialogs;
using Osu.Music.Services.Interfaces;
using Prism.Commands;
using Prism.Dialogs;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Osu.Music.UI.ViewModels.Dialogs
{
	public class AddToPlaylistViewModel : BindableBase, IPopupDialogAware
	{
		private string _title;
		public string Title
		{
			get => _title;
			set => SetProperty(ref _title, value);
		}

		private Beatmap _beatmap;
		public Beatmap Beatmap
		{
			get => _beatmap;
			set => SetProperty(ref _beatmap, value);
		}

		private ObservableCollection<Playlist> _playlists;
		public ObservableCollection<Playlist> Playlists
		{
			get => _playlists;
			set => SetProperty(ref _playlists, value);
		}

		public DelegateCommand<Playlist> SendCommand { get; private set; }
		public DelegateCommand CancelCommand { get; private set; }

		private IPlaylistProvider _playlistProvider;

		public event Action<IDialogResult> RequestClose;

		public AddToPlaylistViewModel(IPlaylistProvider playlistProvider)
		{
			_playlistProvider = playlistProvider;

			InitializeCommands();
		}

		private void InitializeCommands()
		{
			SendCommand = new DelegateCommand<Playlist>(Send);
			CancelCommand = new DelegateCommand(Cancel);
		}

		private void Send(Playlist playlist)
		{
			var result = new DialogResult
			{
				Result = ButtonResult.OK,
				Parameters =
				{
					{ "playlist", playlist },
					{ "beatmap", Beatmap }
				}
			};

			RequestClose?.Invoke(result);
		}

		private void Cancel() => RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));

		public bool CanCloseDialog() => true;

		public void OnDialogClosed() 
		{

		}

		public void OnDialogOpened(IDialogParameters parameters)
		{
			Beatmap = parameters.GetValue<Beatmap>("beatmap");
			Playlists = new ObservableCollection<Playlist>(_playlistProvider.Playlists.Where(x => !x.Beatmaps.Contains(Beatmap)));
		}
	}
}
