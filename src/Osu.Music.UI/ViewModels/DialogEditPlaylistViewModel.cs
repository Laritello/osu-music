using MaterialDesignThemes.Wpf;
using Osu.Music.Common.Models;
using Osu.Music.Services.IO;
using Osu.Music.UI.Utility;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;

namespace Osu.Music.UI.ViewModels
{
    // TODO: Validation, renaming, check for dups
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

        public DelegateCommand ConfirmCommand { get; private set; }

        public string Title => throw new NotImplementedException();

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public DialogEditPlaylistViewModel()
        {
            ConfirmCommand = new DelegateCommand(Confirm);
            Icons = PlaylistIcons.GetIcons();
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Playlist = parameters.GetValue<Playlist>("playlist");
        }

        private void Confirm()
        {
            var result = new DialogResult(ButtonResult.OK);
            RequestClose?.Invoke(result);
        }
    }
}
