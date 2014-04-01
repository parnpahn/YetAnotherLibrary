using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace Yalib.Drawing
{
    public static class ImageHelper
    {
        /// <summary>
        /// Creates a scaled thumbnail.
        /// </summary>
        /// <param name="width">The maximum width of the thumbnail to create.</param>
        /// <param name="height">The maximum height of the thumbnail to create.</param>
        /// <param name="interpolationMode">The Interpolation of the thumbnailing (HighQualityBicubic provides best quality)</param>
        /// <returns>A bitmap thumbnail of the source image.</returns>
        public static Bitmap CreateThumbnail(Bitmap aBitmap, int width, int height, InterpolationMode interpolationMode)
        {
            //Calculate scales
            float x = ((float)aBitmap.Width / (float)width);
            float y = ((float)aBitmap.Height / (float)height);

            float factor = Math.Max(x, y);
            if (factor < 1)
                factor = 1;

            int thWidth = (int)Math.Round((aBitmap.Width / factor), 0);
            int thHeight = (int)Math.Round((aBitmap.Height / factor), 0);

            // Set the size of the target image
            Bitmap bmpTarget = new Bitmap(thWidth, thHeight);

            Graphics grfxThumb = Graphics.FromImage(bmpTarget);
            grfxThumb.InterpolationMode = interpolationMode;

            // Draw the original image to the target image
            grfxThumb.DrawImage(aBitmap, new Rectangle(0, 0, thWidth, Convert.ToInt32(thWidth * aBitmap.Height / aBitmap.Width)));

            grfxThumb.Dispose();
            return bmpTarget;
        }

        public static Bitmap CreateThumbnail(Bitmap aBitmap, int width, int height)
        {
            return CreateThumbnail(aBitmap, width, height, InterpolationMode.Default);
        }

        public static void CreateThumbnail(string srcFileName, string dstFileName, int width, int height)
        {
            Bitmap srcBitmap = new Bitmap(srcFileName);
            Bitmap dstBitmap = CreateThumbnail(srcBitmap, width, height);
            dstBitmap.Save(dstFileName);
        }

        public static Bitmap RotateImageBasedOnExif(Image img)
        {
            Bitmap bmpTarget = new Bitmap(img);
            ExifOrientations exifo = ExifHelper.ImageOrientation(img);
            switch (exifo)
            {
                case ExifOrientations.RightTop:
                    bmpTarget.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case ExifOrientations.BottomRight:
                    bmpTarget.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case ExifOrientations.LeftBottom:
                    bmpTarget.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                default:
                    break;
            }
            return bmpTarget;
        }

        public static Bitmap RotateImageBasedOnExif(string filename)
        {
            Image img = Image.FromFile(filename);
            return RotateImageBasedOnExif(img);
        }
    }
}