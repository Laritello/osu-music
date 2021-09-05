using Osu.Music.Services;
using Osu.Music.UI;
using Osu.Music.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace Osu.Music
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
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
    }
}
