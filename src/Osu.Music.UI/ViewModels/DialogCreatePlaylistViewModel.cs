using Osu.Music.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Osu.Music.UI.ViewModels
{
    public class DialogCreatePlaylistViewModel : BindableBase, IDialogAware
    {
        public DelegateCommand CancelCommand { get; private set; }
        public DelegateCommand ConfirmCommand { get; private set; }

        private string _playlistName;
        public string PlaylistName
        {
            get => _playlistName;
            set => SetProperty(ref _playlistName, value);
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

        public string Title => "Create Playlist";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public DialogCreatePlaylistViewModel()
        {
            CancelCommand = new DelegateCommand(Cancel);
            ConfirmCommand = new DelegateCommand(Confirm);
        }

        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        private void Confirm()
        {
            // Can't have empty name or duplicates of playlists
            if (string.IsNullOrEmpty(PlaylistName) || Playlists.Any(x => x.Name == PlaylistName))
                return;

            var result = new DialogResult(ButtonResult.OK, new DialogParameters()
            {
                { "name", PlaylistName }
            });
            RequestClose?.Invoke(result);
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Playlists = parameters.GetValue<ICollection<Playlist>>("playlists");
        }
    }
}
