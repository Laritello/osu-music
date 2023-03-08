using DryIoc;
using Osu.Music.Common.Models;
using Osu.Music.Services.Dialog;
using Osu.Music.UI.Models;
using Osu.Music.UI.ViewModels.Dialogs;
using Osu.Music.UI.Views.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace Osu.Music.UI.ViewModels
{
    public class PlaylistDetailsViewModel : BindableBase, INavigationAware
    {
        private PaylistDetailsModel _model;
        public PaylistDetailsModel Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        public DelegateCommand DeleteCommand { get; private set; }

        private IPopupDialogService _dialogService;

        public PlaylistDetailsViewModel(IContainer container)
        {
            _dialogService = container.Resolve<IPopupDialogService>();

            Model = new PaylistDetailsModel();

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            DeleteCommand = new DelegateCommand(Delete);
        }

        private void Delete()
        {
            DialogParameters parameters = new DialogParameters()
            {
                { "title", "Delete playlist" },
                { "message", $"Are you sure you want to delete {Model.Playlist.Name}?\r\nThis action cannot be undone." },
                { "caption", "DELETE" }
            };

            _dialogService.ShowPopupDialog<GenericConfirmationView, GenericConfirmationViewModel>(parameters, e =>
            {
                if (e.Result == ButtonResult.OK)
                {
                    // TODO: Implement logic
                }
            });
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var playlist = navigationContext.Parameters.GetValue<Playlist>("playlist");
            return Model.Playlist == playlist;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Model.Playlist = navigationContext.Parameters.GetValue<Playlist>("playlist");
        }
    }
}
