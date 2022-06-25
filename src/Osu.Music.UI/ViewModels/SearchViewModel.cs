using Osu.Music.Common.Models;
using Osu.Music.Services.Search;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Osu.Music.UI.ViewModels
{
    public class SearchViewModel : BindableBase
    {
        private ObservableCollection<Beatmap> _beatmaps;
        /// <summary>
        /// Full list of beatmaps from osu library.
        /// </summary>
        public ObservableCollection<Beatmap> Beatmaps
        {
            get => _beatmaps;
            set => SetProperty(ref _beatmaps, value);
        }

        private ObservableCollection<Beatmap> _result;
        /// <summary>
        /// Full list of beatmaps from osu library.
        /// </summary>
        public ObservableCollection<Beatmap> Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        private string _request;
        public string Request
        {
            get => _request;
            set => SetProperty(ref _request, value);
        }

        public DelegateCommand SearchCommand { get; set; }

        public SearchViewModel(ObservableCollection<Beatmap> beatmaps, string request = null)
        {
            Beatmaps = beatmaps;
            Request = request;

            InitializeCommands();
            Search();
        }

        private void InitializeCommands()
        {
            SearchCommand = new DelegateCommand(Search);
        }

        private void Search() => Result = BeatmapSearch.Search(Beatmaps, Request);
    }
}
