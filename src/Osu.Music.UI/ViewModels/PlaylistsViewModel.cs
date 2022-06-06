using Osu.Music.Common.Models;
using Osu.Music.Services.Dialog;
using Osu.Music.Services.IO;
using Osu.Music.UI.Views;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Collections;
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

        public PlaylistsViewModel(ICollection<Playlist> playlists, IPopupDialogService dialogService)
        {
            Playlists = playlists;
            DialogService = dialogService;

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            ShowCreatePlaylistDialogCommand = new DelegateCommand(ShowCreatePlaylistDialog);
        }

        private void ShowCreatePlaylistDialog()
        {
            DialogParameters parameters = new DialogParameters()
            {
                { "name", "New Playlist" },
                { "playlists", Playlists }
            };

            DialogService.ShowPopupDialog<DialogCreatePlaylistView, DialogCreatePlaylistViewModel>(parameters, e =>
            {
                if (e.Result == ButtonResult.OK)
                {
                    var playlist = new Playlist()
                    {
                        Name = e.Parameters.GetValue<string>("name")
                    };

                    PlaylistManager.Save(playlist);

                    Playlists.Add(playlist);
                }
            });
        }
    }
}
