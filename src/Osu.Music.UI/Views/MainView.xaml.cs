using DryIoc;
using Osu.Music.UI.ViewModels;
using System.Windows.Controls;

namespace Osu.Music.UI.Views
{
	public partial class MainView : UserControl
	{
		public MainView(IContainer container)
		{
			InitializeComponent();
			DataContext = container.Resolve<MainViewModel>();
		}
	}
}
