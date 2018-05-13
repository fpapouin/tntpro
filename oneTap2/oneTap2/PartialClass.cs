using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace oneTap2
{
    public partial class Form1
    {
        public static Bitmap previousImg;
        public static Bitmap currentImg;
        public static Stopwatch chrono = new Stopwatch();

        private void DoLoop()
        {
            if (Mouse.IsMouse3Down() && Mouse.IsMouseCentered(new Point(1920 / 2, 1080 / 2)))
            {
                if (chrono.ElapsedMilliseconds > minElapsedMs)
                {
                    float diff = CenterDiff();
                    if (IsCross(currentImg)
                    && (IsCross(previousImg))
                    && (diff > minDiff))
                    {
                        //System.Threading.Thread.Sleep(Convert.ToInt32(timerInterval)/2);
                        Mouse.ClickMouse1();
                        previousImg = null;
                    }
                    else
                    {
                        if (!IsCross(previousImg))
                            previousImg = currentImg;
                    }


                }
            }
            else
            {
                chrono.Restart();
                previousImg = null;
            }
        }

        private float CenterDiff()
        {
            Rectangle zone = new Rectangle(new Point(1920 / 2, 1080 / 2), new Size(recSize, recSize));
            //Shift zone to center
            zone.Offset(-zone.Size.Width / 2, -zone.Size.Height / 2);

            currentImg = BitmapTools.CaptureBitmap(zone);
            zone.Location = new Point(0, 0);
            return BitmapTools.GetBitmapDiff(previousImg, currentImg, zone);
        }

        private bool IsCross(Bitmap img)
        {
            if (!checkCross) return true;

            if (img == null) return false;

            Color center = img.GetPixel(img.Width / 2, img.Height / 2);
            Color left = img.GetPixel(img.Width / 2 - 1, img.Height / 2);
            Color right = img.GetPixel(img.Width / 2 + 1, img.Height / 2);

            if (center.R == Color.White.R
             && center.G == Color.White.G
             && center.B == Color.White.B)
                return true;

            if (!left.Equals(right)) return false;
            return true;
        }
    }
}
