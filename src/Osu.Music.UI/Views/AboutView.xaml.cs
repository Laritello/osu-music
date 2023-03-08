using DryIoc;
using Osu.Music.UI.ViewModels;
using System.Windows.Controls;

namespace Osu.Music.UI.Views
{
    /// <summary>
    /// Логика взаимодействия для AboutView.xaml
    /// </summary>
    public partial class AboutView : UserControl
    {
        public AboutView(IContainer container)
        {
            InitializeComponent();
            DataContext = container.Resolve<AboutViewModel>();
        }
    }
}
