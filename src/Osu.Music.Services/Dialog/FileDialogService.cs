using System.Windows.Forms;

namespace Osu.Music.Services.Dialog
{
    public class FileDialogService : IFileDialogService
    {
        private readonly FolderBrowserDialog _folderBrowserDialog = new FolderBrowserDialog();
        private readonly OpenFileDialog _openFileDialog = new OpenFileDialog();

        public bool? ShowOpenFileDialog()
        {
            return _openFileDialog.ShowDialog() == DialogResult.OK;
        }

        public bool ShowOpenFolderDialog(out string path)
        {
            var result = _folderBrowserDialog.ShowDialog();
            path = _folderBrowserDialog.SelectedPath;

            return result == DialogResult.OK;
        }
    }
}
