using Osu.Music.Common;
using Osu.Music.UI.ViewModels;
using Osu.Music.UI.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Osu.Music.UI
{
    public class UIModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RequestNavigate(RegionNames.GlobalRegion, nameof(MainView));
            regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(SongsView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            RegisterViews(containerRegistry);
            RegisterViewModels(containerRegistry);
        }

        private void RegisterViews(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainView>();
            containerRegistry.RegisterForNavigation<CollectionsView>();
            containerRegistry.RegisterForNavigation<PlaylistsView>();
            containerRegistry.RegisterForNavigation<PlaylistDetailsView>();
            containerRegistry.RegisterForNavigation<SearchView>();
            containerRegistry.RegisterForNavigation<SongsView>();
        }

        private void RegisterViewModels(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<MainViewModel>();
            containerRegistry.Register<CollectionsViewModel>();
            containerRegistry.Register<PlaylistsViewModel>();
            containerRegistry.Register<PlaylistDetailsViewModel>();
            containerRegistry.Register<SearchViewModel>();
            containerRegistry.Register<SongsViewModel>();
        }
    }
}