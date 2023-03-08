using Osu.Music.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;

namespace Osu.Music.UI.ViewModels.Dialogs
{
    public class NewPlaylistViewModel : BindableBase, IDialogAware
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private IEnumerable<string> _names;
        public IEnumerable<string> Names
        {
            get => _names;
            set => SetProperty(ref _names, value);
        }

        private bool _nameHasError;
        public bool NameHasError
        {
            get => _nameHasError;
            set => SetProperty(ref _nameHasError, value);
        }

        public DelegateCommand CancelCommand { get; private set; }
        public DelegateCommand AcceptCommand { get; private set; }

        public string Title => "New Playlist";

        public event Action<IDialogResult> RequestClose;

        public NewPlaylistViewModel()
        {
            CancelCommand = new DelegateCommand(Cancel);
            AcceptCommand = new DelegateCommand(Accept);
        }

        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        private void Accept()
        {
            if (NameHasError)
                return;

            Playlist playlist = new Playlist() 
            {
                Name = Name,
                Updated = DateTime.Now
            };

            var result = new DialogResult(ButtonResult.OK, new DialogParameters()
            {
                { "playlist", playlist }
            });
            RequestClose?.Invoke(result);
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Names = parameters.GetValue<IEnumerable<string>>("names");
        }
    }
}
