namespace Osu.Music.Services.Dialog
{
    public interface IFileDialogService
    {
        public bool? ShowOpenFileDialog();

        public bool ShowOpenFolderDialog(out string path);
    }
}
