using DryIoc;
using Osu.Music.UI.ViewModels;
using System.Windows.Controls;

namespace Osu.Music.UI.Views
{
    /// <summary>
    /// Логика взаимодействия для SongsView.xaml
    /// </summary>
    public partial class SongsView : UserControl
    {
        public SongsView(IContainer container)
        {
            InitializeComponent();
            DataContext = container.Resolve<SongsViewModel>();
        }
    }
}
