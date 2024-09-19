using System;
using Velopack;

namespace Osu.Music
{
	public static class Program
	{
		[STAThread]
		public static void Main()
		{
			VelopackApp.Build().Run();

			App application = new();
			application.InitializeComponent();
			application.Run();
		}
	}
}
