using Osu.Music.Services.Audio;
using Osu.Music.Services.Dialog;
using Osu.Music.Services.Interfaces;
using Osu.Music.Services.IO;
using Prism.Ioc;
using Prism.Modularity;

namespace Osu.Music.Services
{
    public class ServicesModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Do nothing
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IPopupDialogService, PopupDialogService>();
            containerRegistry.Register<IFileDialogService, FileDialogService>();
            containerRegistry.Register<DiscordManager>();

            containerRegistry.RegisterSingleton<ICollectionManager, CollectionManager>();
            containerRegistry.RegisterSingleton<ILibraryManager, LibraryManager>();
            containerRegistry.RegisterSingleton<IPlaylistManager, PlaylistManager>();
            containerRegistry.RegisterSingleton<AudioPlayback>();
            containerRegistry.RegisterSingleton<SettingsManager>();
        }
    }
}
