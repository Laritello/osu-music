using DryIoc;
using Osu.Music.UI.ViewModels;
using System.Windows.Controls;

namespace Osu.Music.UI.Views
{
    /// <summary>
    /// Логика взаимодействия для PlaylistsView.xaml
    /// </summary>
    public partial class PlaylistsView : UserControl
    {
        public PlaylistsView(IContainer container)
        {
            InitializeComponent();
            DataContext = container.Resolve<PlaylistsViewModel>();
        }
    }
}
