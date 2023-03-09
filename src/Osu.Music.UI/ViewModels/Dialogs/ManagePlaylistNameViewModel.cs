using Osu.Music.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;

namespace Osu.Music.UI.ViewModels.Dialogs
{
    public class ManagePlaylistNameViewModel : BindableBase, IDialogAware
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

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _caption;
        public string Caption
        {
            get => _caption;
            set => SetProperty(ref _caption, value);
        }

        public DelegateCommand CancelCommand { get; private set; }
        public DelegateCommand AcceptCommand { get; private set; }

        public event Action<IDialogResult> RequestClose;

        public ManagePlaylistNameViewModel()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            CancelCommand = new DelegateCommand(Cancel);
            AcceptCommand = new DelegateCommand(Accept);
        }

        private void Cancel() => RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));

        private void Accept()
        {
            if (NameHasError)
                return;

            var result = new DialogResult(ButtonResult.OK, new DialogParameters()
            {
                { "name", Name }
            });
            RequestClose?.Invoke(result);
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("title");
            Caption = parameters.GetValue<string>("caption");
            Name = parameters.ContainsKey("name") ? parameters.GetValue<string>("name") : string.Empty;
            Names = parameters.GetValue<IEnumerable<string>>("names");
        }
    }
}
