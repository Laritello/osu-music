using DryIoc;
using Osu.Music.Common.Models;
using Osu.Music.Services.Audio;
using Osu.Music.UI.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Osu.Music.UI.ViewModels
{
    public class LibraryViewModel : BindableBase, INavigationAware
    {
        private LibraryModel _model;
        public LibraryModel Model
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

        public DelegateCommand<Beatmap> PlayBeatmapCommand { get; private set; }
        public DelegateCommand<Beatmap> OpenBeatmapInBrowserCommand { get; private set; }

        public LibraryViewModel(IContainer container)
        {
            _playback = container.Resolve<AudioPlayback>();

            Model = new LibraryModel();

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            PlayBeatmapCommand = new DelegateCommand<Beatmap>(PlayBeatmap);
            OpenBeatmapInBrowserCommand = new DelegateCommand<Beatmap>(OpenBeatmapInBrowser);
        }

        private void PlayBeatmap(Beatmap beatmap)
        {
            if (Playback.Queue != Model.Beatmaps)
                Playback.Queue = Model.Beatmaps;

            Playback.Beatmap = beatmap;
            Playback.Play();
        }

        private void OpenBeatmapInBrowser(Beatmap beatmap) => Process.Start(new ProcessStartInfo("cmd", $"/c start https://osu.ppy.sh/beatmapsets/{beatmap.BeatmapSetId}") { CreateNoWindow = true });

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            var collection = navigationContext.Parameters.GetValue<ObservableCollection<Beatmap>>("beatmaps");
            return collection.Equals(Model.Beatmaps);
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Model.Beatmaps = navigationContext.Parameters.GetValue<ObservableCollection<Beatmap>>("beatmaps");
        }
    }
}
