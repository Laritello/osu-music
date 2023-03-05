using DryIoc;
using Osu.Music.UI.ViewModels;
using System.Windows.Controls;

namespace Osu.Music.UI.Views
{
    /// <summary>
    /// Логика взаимодействия для PlaylistDetailsView.xaml
    /// </summary>
    public partial class PlaylistDetailsView : UserControl
    {
        public PlaylistDetailsView(IContainer container)
        {
            InitializeComponent();
            DataContext = container.Resolve<PlaylistDetailsViewModel>();
        }
    }
}
