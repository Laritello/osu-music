using Osu.Music.Services.Abstractions;
using Osu.Music.Services.Audio;
using Osu.Music.Services.Dialog;
using Osu.Music.Services.Hotkeys;
using Osu.Music.Services.Interfaces;
using Osu.Music.Services.IO;
using Osu.Music.Services.Social;
using Osu.Music.Services.Updates;
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
            containerRegistry.Register<IFileDialogService, FileDialogService>();

			containerRegistry.RegisterSingleton<IUpdateService, UpdateService>();
			containerRegistry.RegisterSingleton<ICollectionProvider, CollectionProvider>();
            containerRegistry.RegisterSingleton<ILibraryProvider, LibraryProvider>();
            containerRegistry.RegisterSingleton<IPlaylistProvider, PlaylistProvider>();
            containerRegistry.RegisterSingleton<AudioPlayback>();
            containerRegistry.RegisterSingleton<SettingsProvider>();
            containerRegistry.RegisterSingleton<HotkeyManager>();
            containerRegistry.RegisterSingleton<DiscordManager>();
        }
    }
}
