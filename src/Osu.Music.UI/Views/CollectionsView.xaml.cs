using DryIoc;
using Osu.Music.UI.ViewModels;
using System.Windows.Controls;

namespace Osu.Music.UI.Views
{
    /// <summary>
    /// Логика взаимодействия для CollectionsView.xaml
    /// </summary>
    public partial class CollectionsView : UserControl
    {
        public CollectionsView(IContainer container)
        {
            InitializeComponent();
            DataContext = container.Resolve<CollectionsViewModel>();
        }
    }
}
