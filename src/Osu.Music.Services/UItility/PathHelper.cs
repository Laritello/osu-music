using Microsoft.Win32;
using System.IO;

namespace Osu.Music.Services.UItility
{
    public static class PathHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetOsuInstallationFolder()
        {
            // Kudos to https://osu.ppy.sh/community/forums/topics/381311?n=3
            const string keyName1 = @"HKEY_CLASSES_ROOT\osu\shell\open\command";
            const string keyName2 = @"HKEY_CLASSES_ROOT\osu!\shell\open\command";
            string path;

            try
            {
                path = Registry.GetValue(keyName1, string.Empty, string.Empty).ToString();

                if (path == string.Empty)
                    path = Registry.GetValue(keyName2, string.Empty, string.Empty).ToString();

                if (path != string.Empty)
                {
                    path = path.Remove(0, 1);
                    path = path.Split('\"')[0];
                    path = Path.GetDirectoryName(path);
                }

                return path ?? null;
            }
            catch
            {
                return "";
            }
        }
    }
}
