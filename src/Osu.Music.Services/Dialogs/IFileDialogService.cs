namespace Osu.Music.Services.Dialogs
{
	public interface IFileDialogService
	{
		public bool? ShowOpenFileDialog();

		public bool ShowOpenFolderDialog(out string path);
	}
}
