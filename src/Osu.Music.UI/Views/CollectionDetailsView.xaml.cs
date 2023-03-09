using DryIoc;
using Osu.Music.UI.ViewModels;
using System.Windows.Controls;

namespace Osu.Music.UI.Views
{
    /// <summary>
    /// Логика взаимодействия для CollectionDetailsView.xaml
    /// </summary>
    public partial class CollectionDetailsView : UserControl
    {
        public CollectionDetailsView(IContainer container)
        {
            InitializeComponent();
            DataContext = container.Resolve<CollectionDetailsViewModel>();
        }
    }
}
