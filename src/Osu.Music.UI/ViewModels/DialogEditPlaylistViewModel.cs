using MaterialDesignThemes.Wpf;
using Osu.Music.Common.Models;
using Osu.Music.UI.Utility;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;

namespace Osu.Music.UI.ViewModels
{
    public class DialogEditPlaylistViewModel : BindableBase, IDialogAware
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

        public DelegateCommand ConfirmCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public string Title => throw new NotImplementedException();

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        private string _originalName;

        public DialogEditPlaylistViewModel()
        {
            ConfirmCommand = new DelegateCommand(Confirm);
            CancelCommand = new DelegateCommand(Cancel);

            Icons = PlaylistIcons.GetIcons();
        }

        public void OnDialogClosed()
        {
            Playlist.CancelEdit();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Playlist = parameters.GetValue<Playlist>("playlist");
            Playlist.BeginEdit();
            _originalName = Playlist.Name;

            Playlists = parameters.GetValue<ICollection<Playlist>>("playlists");
        }

        private void Cancel()
        {
            Playlist.CancelEdit();
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        private void Confirm()
        {
            if (NameHasError)
                return;

            var result = new DialogResult(ButtonResult.OK, new DialogParameters()
            {
                { "originalName", _originalName }
            });

            Playlist.EndEdit();
            RequestClose?.Invoke(result);
        }
    }
}
