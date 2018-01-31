using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace PrayashStore.Extensions
{
    public static class ImageExtensions
    {
        public static Image Resize(this Image img, int maxWidth, int maxHeight)
        {
            int width, height;

            if (img.Width > img.Height)
            {
                width = maxWidth;
                height = Convert.ToInt32(img.Height * maxHeight / (double)img.Width);
            }
            else
            {
                width = Convert.ToInt32(img.Width * maxWidth / (double)img.Height);
                height = maxHeight;
            }

            var canvas = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(canvas))
            {
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.DrawImage(img, 0, 0, width, height);
            }

            return canvas;
        }
        public static byte[] ToByteArray(this Image image, ImageFormat imageFormat)
        {
            using (var stream = new MemoryStream())
            {
                image.Save(stream, imageFormat);
                return stream.ToArray();
            }
        }

        public static byte[] ToByteArry(this HttpPostedFileBase file, ImageFormat imageFormat)
        {
            using (Image img = Image.FromStream(file.InputStream))
            {
                using (var stream = new MemoryStream())
                {
                    img.Save(stream, imageFormat);
                    return stream.ToArray();
                }
            }
        }
    }
}