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

        #region Validation
        private bool _nameHasError;
        public bool NameHasError
        {
            get => _nameHasError;
            set => SetProperty(ref _nameHasError, value);
        }
        #endregion

        #region Expanders
        private bool _backgroundExpanded = true;
        public bool BackgroundExpanded
        {
            get => _backgroundExpanded;
            set
            {
                if (_backgroundExpanded == value)
                    return;

                SetProperty(ref _backgroundExpanded, value);
                IconExpanded = !value;
            }
        }

        private bool _iconExpanded = false;
        public bool IconExpanded
        {
            get => _iconExpanded;
            set
            {
                if (_iconExpanded == value)
                    return;

                SetProperty(ref _iconExpanded, value);
                BackgroundExpanded = !value;
            }
        }
        #endregion

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
            if (NameHasError)
                return;

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
            // Do nothing
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Playlists = parameters.GetValue<ICollection<Playlist>>("playlists");
        }
    }
}
