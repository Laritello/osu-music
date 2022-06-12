using Osu.Music.Common.Models;
using osu_database_reader.Components.Events;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Osu.Music.Common.Utility
{
    public class BackgroundRepository
    {
        public static Task<Bitmap> GetImageAsync(Beatmap beatmap)
        {
            return Task.Run(() =>
            {
                var osuFileCheck = GetFilePath(beatmap, out string osu);

                if (!osuFileCheck)
                    return new Bitmap(50, 50);

                using var fs = File.OpenRead(osu);
                var bm = BeatmapFileReader.Read(fs);

                foreach (var ev in bm.Events)
                {
                    if (!(ev is BackgroundEvent))
                        continue;

                    var path = Path.Combine(beatmap.Directory, ((BackgroundEvent)ev).Path);

                    return File.Exists(path)
                        ? ResizeImage(new Bitmap(path), 50, 50)
                        : new Bitmap(50, 50);
                }

                return new Bitmap(50, 50);
            });
        }

        private static bool GetFilePath(Beatmap beatmap, out string path)
        {
            path = string.Empty;

            if (beatmap.Directory == null || beatmap.FileName == null)
                return false;

            path = Path.Combine(beatmap.Directory, beatmap.FileName);
            return File.Exists(path);
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);

            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution,
              image.VerticalResolution);

            using (var g = Graphics.FromImage(destImage))
            {
                g.CompositingMode = CompositingMode.SourceCopy;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using var wrapMode = new ImageAttributes();
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                g.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }
            return destImage;
        }
    }
}
