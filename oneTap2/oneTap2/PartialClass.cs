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
        public static Bitmap previousRadar;
        public static Bitmap currentRadar;
        public static Stopwatch chrono = new Stopwatch();

        private void DoLoop()
        {
            if (Mouse.IsMouse3Down() && Mouse.IsMouseCentered(new Point(1920 / 2, 1080 / 2)))
            {
                if (chrono.ElapsedMilliseconds > 100)
                {

                    float diff = CenterDiff();
                    float radar = RadarDiff();
                    if (IsCross(currentImg))
                        if (IsCross(previousImg))
                            if (radar < 1)
                                if (diff > 20.0)
                                {
                                    Mouse.ClickMouse1();
                                    chrono.Restart();
                                }

                    previousImg = currentImg;
                    previousRadar = currentRadar;
                }
            }
            else
            {
                chrono.Restart();
                previousImg = null;
                previousRadar = null;
            }
        }

        private float RadarDiff()
        {
            Rectangle zone = new Rectangle(new Point(346, 338), new Size(5, 5));
            //Shift zone to center
            zone.Offset(-zone.Size.Width / 2, -zone.Size.Height / 2);

            currentRadar = BitmapTools.CaptureBitmap(zone);
            zone.Location = new Point(0, 0);
            return BitmapTools.GetBitmapDiff(previousRadar, currentRadar, zone);
        }

        private float CenterDiff()
        {
            Rectangle zone = new Rectangle(new Point(1920 / 2, 1080 / 2), new Size(5, 5));
            //Shift zone to center
            zone.Offset(-zone.Size.Width / 2, -zone.Size.Height / 2);

            currentImg = BitmapTools.CaptureBitmap(zone);
            zone.Location = new Point(0, 0);
            return BitmapTools.GetBitmapDiff(previousImg, currentImg, zone);
        }

        private bool IsCross(Bitmap img)
        {
            if (img == null) return false;

            Color center = img.GetPixel(img.Width / 2, img.Height / 2);
            Color left = img.GetPixel(img.Width / 2 - 2, img.Height / 2);
            Color right = img.GetPixel(img.Width / 2 + 2, img.Height / 2);

            if (!center.Equals(left)) return false;
            if (!center.Equals(right)) return false;

            return true;
        }
    }
}
