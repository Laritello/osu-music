using Osu.Music.Common.Models;
using Osu.Music.Services.Dialog;
using Osu.Music.Services.IO;
using Osu.Music.UI.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Collections.Generic;

namespace Osu.Music.UI.ViewModels
{
    public class PlaylistsViewModel : BindableBase
    {
        private IPopupDialogService _dialogService;
        public IPopupDialogService DialogService
        {
            get => _dialogService;
            set => SetProperty(ref _dialogService, value);
        }

        private ICollection<Playlist> _playlists;
        /// <summary>
        /// Collection of user-created playlists.
        /// </summary>
        public ICollection<Playlist> Playlists
        {
            get => _playlists;
            set => SetProperty(ref _playlists, value);
        }

        public DelegateCommand ShowCreatePlaylistDialogCommand { get; private set; }
        public DelegateCommand<Playlist> ShowEditPlaylistDialogCommand { get; private set; }

        public PlaylistsViewModel(ICollection<Playlist> playlists, IPopupDialogService dialogService)
        {
            Playlists = playlists;
            DialogService = dialogService;

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            ShowCreatePlaylistDialogCommand = new DelegateCommand(ShowCreatePlaylistDialog);
            ShowEditPlaylistDialogCommand = new DelegateCommand<Playlist>(ShowEditPlaylistDialog);
        }

        private void ShowCreatePlaylistDialog()
        {
            DialogParameters parameters = new DialogParameters()
            {
                { "playlists", Playlists }
            };

            DialogService.ShowPopupDialog<DialogCreatePlaylistView, DialogCreatePlaylistViewModel>(parameters, e =>
            {
                if (e.Result == ButtonResult.OK)
                {
                    var playlist = e.Parameters.GetValue<Playlist>("playlist");
                    Playlists.Add(playlist);
                    PlaylistManager.Save(playlist);
                }
            });
        }

        private void ShowEditPlaylistDialog(Playlist playlist)
        {
            if (playlist == null)
                return;

            DialogParameters parameters = new DialogParameters()
            {
                { "playlist", playlist },
                { "playlists", Playlists }
            };

            DialogService.ShowPopupDialog<DialogEditPlaylistView, DialogEditPlaylistViewModel>(parameters, e =>
            {
                if (e.Result == ButtonResult.OK)
                {
                    var originalName = e.Parameters.GetValue<string>("originalName");

                    if (!playlist.Name.Equals(originalName))
                        PlaylistManager.RemoveByName(originalName);

                    PlaylistManager.Save(playlist);
                }
            });
        }
    }
}
