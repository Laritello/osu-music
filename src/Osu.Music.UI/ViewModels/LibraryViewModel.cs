using DryIoc;
using Osu.Music.Common.Models;
using Osu.Music.Services.Audio;
using Osu.Music.UI.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.Windows;

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

        public DelegateCommand<object[]> PlayBeatmapCommand { get; private set; }

        public LibraryViewModel(IContainer container)
        {
            _playback = container.Resolve<AudioPlayback>();

            Model = new LibraryModel();

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            PlayBeatmapCommand = new DelegateCommand<object[]>(PlayBeatmap);
        }

        private void PlayBeatmap(object[] parameters)
        {
            var beatmap = parameters[0] as Beatmap;
            var collection = parameters[1] as ObservableCollection<Beatmap>;

            if (Playback.Queue != collection)
                Playback.Queue = collection;

            Playback.Beatmap = beatmap;
            Playback.Play();
        }

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
