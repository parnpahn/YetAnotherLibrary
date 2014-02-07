using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Hlt.Drawing
{
    public enum ExifOrientations
    {
        Unknown = 0,
        TopLeft = 1,
        TopRight = 2,
        BottomRight = 3,
        BottomLeft = 4,
        LeftTop = 5,
        RightTop = 6,
        RightBottom = 7,
        LeftBottom = 8
    }

    /// <summary>
    /// Reference: http://www.vb-helper.com/howto_net_read_exif_orientation.html
    /// </summary>
    public static class ExifHelper
    {
        private const int OrientationID = 0x112;

        /// <summary>
        /// Return the image's orientation.
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static ExifOrientations ImageOrientation(Image img)
        {
            // Get the index of the orientation property.
            int orientation_index = Array.IndexOf(img.PropertyIdList, OrientationID);

            // If there is no such property, return Unknown.
            if ((orientation_index < 0))
                return ExifOrientations.Unknown;

            // Return the orientation value.
            return (ExifOrientations)img.GetPropertyItem(OrientationID).Value[0];
        }

        /// <summary>
        /// Make an image to demonstrate orientations.
        /// </summary>
        /// <param name="orientation"></param>
        /// <returns></returns>
        public static Image OrientationImage(ExifOrientations orientation)
        {
            const int size = 64;
            Bitmap bm = new Bitmap(64, 64);
            using (Graphics gr = Graphics.FromImage(bm))
            {
                gr.Clear(Color.White);
                gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                // Orient the result.
                switch ((orientation))
                {
                    case ExifOrientations.TopLeft:
                        break;
                    case ExifOrientations.TopRight:
                        gr.ScaleTransform(-1, 1);
                        break;
                    case ExifOrientations.BottomRight:
                        gr.RotateTransform(180);
                        break;
                    case ExifOrientations.BottomLeft:
                        gr.ScaleTransform(1, -1);
                        break;
                    case ExifOrientations.LeftTop:
                        gr.RotateTransform(90);
                        gr.ScaleTransform(-1, 1, MatrixOrder.Append);
                        break;
                    case ExifOrientations.RightTop:
                        gr.RotateTransform(-90);
                        break;
                    case ExifOrientations.RightBottom:
                        gr.RotateTransform(90);
                        gr.ScaleTransform(1, -1, MatrixOrder.Append);
                        break;
                    case ExifOrientations.LeftBottom:
                        gr.RotateTransform(90);
                        break;
                }

                // Translate the result to the center of the bitmap.
                gr.TranslateTransform(size / 2, size / 2, MatrixOrder.Append);
                using (StringFormat string_format = new StringFormat())
                {
                    string_format.LineAlignment = StringAlignment.Center;
                    string_format.Alignment = StringAlignment.Center;
                    using (Font the_font = new Font("Times New Roman", 40, GraphicsUnit.Point))
                    {
                        if ((orientation == ExifOrientations.Unknown))
                        {
                            gr.DrawString("?", the_font, Brushes.Black, 0, 0, string_format);
                        }
                        else
                        {
                            gr.DrawString("F", the_font, Brushes.Black, 0, 0, string_format);
                        }
                    }
                }
            }

            return bm;
        }
    }
}