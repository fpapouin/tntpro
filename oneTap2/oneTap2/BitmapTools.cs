using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oneTap2
{
    public static class BitmapTools
    {
        //Get a piece of the screen.
        public static Bitmap CaptureBitmap(Rectangle rect)
        {
            Bitmap bmp = new Bitmap(rect.Width, rect.Height);
            Graphics graphic = Graphics.FromImage(bmp);
            graphic.CopyFromScreen(rect.Location, new Point(), rect.Size);
            return bmp;
        }

        //Get purcentage diff between img
        public static float GetBitmapDiff(Bitmap img1, Bitmap img2)
        {
            float diff = 0;
            if (img1 != null && img2 != null)
            {
                if (img1.Size.Equals(img2.Size))
                {
                    for (int x = 0; x < img1.Size.Width; x++)
                    {
                        for (int y = 0; y < img1.Size.Height; y++)
                        {
                            diff += (float)Math.Abs(img1.GetPixel(x, y).R - img2.GetPixel(x, y).R) / 255;
                            diff += (float)Math.Abs(img1.GetPixel(x, y).G - img2.GetPixel(x, y).G) / 255;
                            diff += (float)Math.Abs(img1.GetPixel(x, y).B - img2.GetPixel(x, y).B) / 255;
                        }
                    }
                    return 1000 * diff / (img1.Size.Width * img1.Size.Height * 3);
                }
            }
            return 0;
        }

        //Get purcentage diff between img in a rectangle
        public static float GetBitmapDiff(Bitmap img1, Bitmap img2, Rectangle rect)
        {
            float diff = 0;
            if (img1 != null && img2 != null)
            {
                if (img1.Size.Equals(img2.Size))
                {
                    for (int x = rect.X; x < rect.Width + rect.X; x++)
                    {
                        for (int y = rect.Y; y < rect.Height + rect.Y; y++)
                        {
                            diff += (float)Math.Abs(img1.GetPixel(x, y).R - img2.GetPixel(x, y).R) / 255;
                            diff += (float)Math.Abs(img1.GetPixel(x, y).G - img2.GetPixel(x, y).G) / 255;
                            diff += (float)Math.Abs(img1.GetPixel(x, y).B - img2.GetPixel(x, y).B) / 255;
                        }
                    }
                    return 1000 * diff / (img1.Size.Width * img1.Size.Height * 3);
                }
            }
            return 0;
        }

    }
}
