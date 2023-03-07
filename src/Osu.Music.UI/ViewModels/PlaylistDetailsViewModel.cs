using Osu.Music.Common.Models;
using Osu.Music.UI.Models;
using Prism.Mvvm;
using Prism.Regions;

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

        public PlaylistDetailsViewModel()
        {
            Model = new PaylistDetailsModel();
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
