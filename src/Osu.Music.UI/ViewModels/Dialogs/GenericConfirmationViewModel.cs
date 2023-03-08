using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;

namespace Osu.Music.UI.ViewModels.Dialogs
{
    public class GenericConfirmationViewModel : BindableBase, IDialogAware
    {
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private string _caption;
        public string Caption
        {
            get => _caption;
            set => SetProperty(ref _caption, value);
        }

        public event Action<IDialogResult> RequestClose;

        public DelegateCommand AcceptCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public GenericConfirmationViewModel()
        {
            AcceptCommand = new DelegateCommand(Accept);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Accept() => RequestClose?.Invoke(new DialogResult(ButtonResult.OK));

        private void Cancel() => RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("title");
            Message = parameters.GetValue<string>("message");
            Caption = parameters.GetValue<string>("caption");
        }
    }
}
