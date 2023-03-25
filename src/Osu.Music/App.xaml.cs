using Osu.Music.Services;
using Osu.Music.Services.Interfaces;
using Osu.Music.UI;
using Osu.Music.UI.ViewModels;
using Osu.Music.UI.Views;
using Osu.Music.Views;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Windows;

namespace Osu.Music
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Do nothing
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ServicesModule>();
            moduleCatalog.AddModule<UIModule>();
        }

        protected override void OnInitialized()
        {
            SplashScreen screen = new SplashScreen("Logo.png");
            screen.Show(false);
            LoadData();
            screen.Close(TimeSpan.Zero);

            base.OnInitialized();
        }

        private void LoadData()
        {
            Container.Resolve<ILibraryProvider>().Load();
            Container.Resolve<IPlaylistProvider>().Load();
            Container.Resolve<ICollectionProvider>().Load();
        }
    }
}
