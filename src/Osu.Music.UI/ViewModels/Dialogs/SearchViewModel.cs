using Osu.Music.Common;
using Osu.Music.Common.Interfaces;
using Osu.Music.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace Osu.Music.UI.ViewModels.Dialogs
{
    public class SearchViewModel : BindableBase, IDialogAware
    {
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _request;
        public string Request
        {
            get => _request;
            set => SetProperty(ref _request, value);
        }

        private ObservableCollection<Beatmap> _beatmaps;
        public ObservableCollection<Beatmap> Beatmaps
        {
            get => _beatmaps;
            set => SetProperty(ref _beatmaps, value);
        }

        private ObservableCollection<Playlist> _playlists;
        public ObservableCollection<Playlist> Playlists
        {
            get => _playlists;
            set => SetProperty(ref _playlists, value);
        }

        private ObservableCollection<Collection> _collections;
        public ObservableCollection<Collection> Collections
        {
            get => _collections;
            set => SetProperty(ref _collections, value);
        }

        private ObservableCollection<ISearchable> _beatmapResult;
        public ObservableCollection<ISearchable> BeatmapResult
        {
            get => _beatmapResult;
            set => SetProperty(ref _beatmapResult, value);
        }

        private ObservableCollection<ISearchable> _playlistResult;
        public ObservableCollection<ISearchable> PlaylistResult
        {
            get => _playlistResult;
            set => SetProperty(ref _playlistResult, value);
        }

        private ObservableCollection<ISearchable> _collectionResult;
        public ObservableCollection<ISearchable> CollectionResult
        {
            get => _collectionResult;
            set => SetProperty(ref _collectionResult, value);
        }

        private ObservableCollection<ISearchable> _combinedResult;
        public ObservableCollection<ISearchable> CombinedResult
        {
            get => _combinedResult;
            set => SetProperty(ref _combinedResult, value);
        }

        public DelegateCommand<string> SearchCommand { get; private set; }
        public DelegateCommand<ISearchable> JumpCommand { get; private set; }

        public event Action<IDialogResult> RequestClose;

        public SearchViewModel()
        {
            CombinedResult = new ObservableCollection<ISearchable>();
            BeatmapResult = new ObservableCollection<ISearchable>();
            PlaylistResult = new ObservableCollection<ISearchable>();
            CollectionResult = new ObservableCollection<ISearchable>();

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            SearchCommand = new DelegateCommand<string>(Search);
            JumpCommand = new DelegateCommand<ISearchable>(Jump);
        }

        private void Search(string text)
        {
            BeatmapResult.Clear();
            PlaylistResult.Clear();
            CollectionResult.Clear();
            CombinedResult.Clear();

            if (string.IsNullOrEmpty(text))
                return;

            var query = BuildQuery(text);

            BeatmapResult.AddRange(Beatmaps.Where(x => x.Match(query)).OrderBy(x => x.Matches));
            PlaylistResult.AddRange(Playlists.Where(x => x.Match(query)).OrderBy(x => x.Matches));
            CollectionResult.AddRange(Collections.Where(x => x.Match(query)).OrderBy(x => x.Matches));
            CombinedResult.AddRange(BeatmapResult.Concat(PlaylistResult).Concat(CollectionResult).OrderBy(x => x.Matches));
        }

        private void Jump(ISearchable target)
        {
            var result = new DialogResult(ButtonResult.OK, new DialogParameters()
            {
                { "target", target }
            });
            RequestClose?.Invoke(result);
        }

        private Regex BuildQuery(string text) => new Regex($"(.*({Regex.Escape(text)}).*)", RegexOptions.IgnoreCase);

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Beatmaps = parameters.GetValue<ObservableCollection<Beatmap>>("beatmaps");
            Playlists = parameters.GetValue<ObservableCollection<Playlist>>("playlists");
            Collections = parameters.GetValue<ObservableCollection<Collection>>("collections");
        }
    }
}
