using DryIoc;
using Osu.Music.Common.Models;
using Osu.Music.Services.Audio;
using Osu.Music.UI.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Diagnostics;
using System.Linq;

namespace Osu.Music.UI.ViewModels
{
    internal class CollectionDetailsViewModel : BindableBase, INavigationAware
    {
        private CollectionDetailsModel _model;
        public CollectionDetailsModel Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        private AudioPlayback _playback;
        public AudioPlayback Playback
        {
            get => _playback;
            set => SetProperty(ref _playback, value);
        }

        public DelegateCommand LaunchCollectionCommand { get; private set; }
        public DelegateCommand<Beatmap> PlayBeatmapCommand { get; private set; }
        public DelegateCommand<Beatmap> OpenBeatmapInBrowserCommand { get; private set; }

        public CollectionDetailsViewModel(IContainer container, CollectionDetailsModel model)
        {
            _playback = container.Resolve<AudioPlayback>();
            _model = model;

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            LaunchCollectionCommand = new DelegateCommand(LaunchCollection);
            PlayBeatmapCommand = new DelegateCommand<Beatmap>(PlayBeatmap);
            OpenBeatmapInBrowserCommand = new DelegateCommand<Beatmap>(OpenBeatmapInBrowser);
        }

        private void LaunchCollection()
        {
            if (Model.Collection != null && Model.Collection.Beatmaps.Count > 0)
            {
                _playback.Queue = Model.Collection.Beatmaps;
                _playback.Beatmap = Model.Collection.Beatmaps.FirstOrDefault();
                _playback.Play();
            }
        }

        private void PlayBeatmap(Beatmap beatmap)
        {
            if (_playback.Queue != Model.Collection.Beatmaps)
                _playback.Queue = Model.Collection.Beatmaps;

            _playback.Beatmap = beatmap;
            _playback.Play();
        }

        private void OpenBeatmapInBrowser(Beatmap beatmap) => Process.Start(new ProcessStartInfo("cmd", $"/c start https://osu.ppy.sh/beatmapsets/{beatmap.BeatmapSetId}") { CreateNoWindow = true });

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var collection = navigationContext.Parameters.GetValue<Collection>("collection");
            return Model.Collection == collection;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Model.Collection = navigationContext.Parameters.GetValue<Collection>("collection");
        }
    }
}
