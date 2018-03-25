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
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002; /* left button down */
        private const int MOUSEEVENTF_LEFTUP = 0x0004; /* left button up */

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, EntryPoint = "mouse_event")]
        public static extern void Mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern short GetKeyState(int virtualKey);

        static class StateMachine
        {
            public static int squarePixelSize = 10;

            public static bool isMouse3Down = false;
            public static Bitmap refBitMap;
            public static Bitmap lastBitMap;
        }

        private void DoLoop()
        {
            StateMachine.isMouse3Down = (GetKeyState(0x04) & 0x80) == 128;

            if (StateMachine.isMouse3Down)
            {
                //is ref OK?
                if (IsCross(StateMachine.refBitMap, StateMachine.squarePixelSize))
                {
                    //get last
                    StateMachine.lastBitMap = GetBitmap(StateMachine.squarePixelSize);
                    //is last OK?
                    if (IsCross(StateMachine.lastBitMap, StateMachine.squarePixelSize))
                    {
                        //getdiff
                        float diff = GetBitmapDiff(StateMachine.refBitMap, StateMachine.lastBitMap, StateMachine.squarePixelSize);

                        //is Diff between 2% and 50% ?
                        if (diff > 3 && diff < 50)
                        {
                            //click
                            Mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                            Mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                        }
                    }
                    else
                    {
                        //Delete img
                        StateMachine.lastBitMap = null;
                        StateMachine.refBitMap = null;
                    }
                }
                else
                {
                    //take photo
                    StateMachine.refBitMap = GetBitmap(StateMachine.squarePixelSize);
                }
            }
            else
            {
                //Delete img
                StateMachine.lastBitMap = null;
                StateMachine.refBitMap = null;
            }
        }

        //Get a square img from the center of FULLHD screen.
        private Bitmap GetBitmap(int squarePixelSize)
        {
            Bitmap bmpScreenCapture = new Bitmap(squarePixelSize, squarePixelSize);
            Graphics graphic = Graphics.FromImage(bmpScreenCapture);

            graphic.CopyFromScreen(1920 / 2 - squarePixelSize / 2, 1080 / 2 - squarePixelSize / 2, 0, 0, bmpScreenCapture.Size, CopyPixelOperation.SourceCopy);

            return bmpScreenCapture;
        }

        //Get purcentage diff between to squared img
        private float GetBitmapDiff(Bitmap img1, Bitmap img2, int squarePixelSize)
        {
            float diff = 0;
            if (img1 != null && img2 != null)
            {
                for (int x = 0; x < squarePixelSize; x++)
                {
                    for (int y = 0; y < squarePixelSize; y++)
                    {
                        diff += (float)Math.Abs(img1.GetPixel(x, y).R - img2.GetPixel(x, y).R) / 255;
                        diff += (float)Math.Abs(img1.GetPixel(x, y).G - img2.GetPixel(x, y).G) / 255;
                        diff += (float)Math.Abs(img1.GetPixel(x, y).B - img2.GetPixel(x, y).B) / 255;
                    }
                }
            }
            return 100 * diff / (squarePixelSize * squarePixelSize * 3);
        }

        //Check if there is a single color cross
        private bool IsCross(Bitmap img, int squarePixelSize)
        {
            if (img == null) return false;

            Color center = img.GetPixel(squarePixelSize / 2, squarePixelSize / 2);

            for (int x = 1; x < squarePixelSize / 2; x = x + 2)
            {
                //Check vertical
                if (img.GetPixel(x, squarePixelSize / 2).R != center.R) return false;
                if (img.GetPixel(x, squarePixelSize / 2).G != center.G) return false;
                if (img.GetPixel(x, squarePixelSize / 2).B != center.B) return false;

                //Check horizontal
                if (img.GetPixel(squarePixelSize / 2, x).R != center.R) return false;
                if (img.GetPixel(squarePixelSize / 2, x).G != center.G) return false;
                if (img.GetPixel(squarePixelSize / 2, x).B != center.B) return false;
            }
            return true;
        }
    }
}
