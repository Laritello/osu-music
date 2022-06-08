using MaterialDesignThemes.Wpf;
using Osu.Music.Common.Models;
using Osu.Music.Services.UItility;
using Osu.Music.UI.Utility;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Osu.Music.UI.ViewModels
{
    public class DialogCreatePlaylistViewModel : BindableBase, IDialogAware
    {
        private IEnumerable<PackIconKind> _icons;
        public IEnumerable<PackIconKind> Icons
        {
            get => _icons;
            set => SetProperty(ref _icons, value);
        }

        private Playlist _playlist;
        public Playlist Playlist
        {
            get => _playlist;
            set => SetProperty(ref _playlist, value);
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

        public DelegateCommand CancelCommand { get; private set; }
        public DelegateCommand ConfirmCommand { get; private set; }

        public string Title => "Create Playlist";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public DialogCreatePlaylistViewModel()
        {
            ResourceDictionary resource = Application.Current.Resources;

            Playlist = new Playlist()
            {
                Cover = new PlaylistCover()
                {
                    Icon = PackIconKind.PlaylistMusic,
                    IconColor = Colors.White.ToHex(),
                    BackgroundColor = resource.MergedDictionaries.GetMainColor().ToHex(),
                }
            };

            CancelCommand = new DelegateCommand(Cancel);
            ConfirmCommand = new DelegateCommand(Confirm);
            Icons = PlaylistIcons.GetIcons();
        }

        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        private void Confirm()
        {
            // Can't have empty name or duplicates of playlists
            if (string.IsNullOrEmpty(Playlist.Name) || Playlists.Any(x => x.Name == Playlist.Name))
                return;

            var result = new DialogResult(ButtonResult.OK, new DialogParameters()
            {
                { "playlist", Playlist }
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
