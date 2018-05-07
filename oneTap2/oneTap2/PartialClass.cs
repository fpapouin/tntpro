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
        public static Painter painter = new Painter();
        public static Rectangle zone = new Rectangle(new Point(1920 / 2, 1080 / 2), new Size(512, 16));

        public static Bitmap previousImg;
        public static Bitmap currentImg;

        public static Stopwatch chrono = new Stopwatch();
        static long elapsedMilliseconds = 0;

        static bool hasEnemy = false;
        static float left = 0.0f;
        static float center = 0.0f;
        static float right = 0.0f;

        //List<Square> Squares = new List<Square> { center, two, four, eight, ten };

        private void DoLoop()
        {
            chrono.Restart();

            if (Mouse.IsMouse3Down() && Mouse.IsMouseCentered(new Point(1920 / 2, 1080 / 2)))
            {
                CalcMovement(zone);

                //si les mouvements vers left ou vers right sont proche de zero alors on ne bouge pas et on peu analyser le centre.
                hasEnemy = false;
                if (left < 1 || right < 1)
                    if (center > 2.0f) hasEnemy = true;


                if (hasEnemy)
                {
                    if (IsCross(currentImg))
                    Mouse.ClickMouse1();
                }
            }

            elapsedMilliseconds = chrono.ElapsedMilliseconds;
        }

        public void CalcMovement(Rectangle rect)
        {
            previousImg = currentImg;
            currentImg = BitmapTools.CaptureBitmap(rect);

            Rectangle leftRect = new Rectangle(0, 0, 16, 16);
            Rectangle rightRect = new Rectangle(rect.Width - 16, 0, 16, 16);
            Rectangle centerRect = new Rectangle(rect.Width / 2 - 16 / 2, 0, 16, 16);

            left = BitmapTools.GetBitmapDiff(previousImg, currentImg, leftRect);
            right = BitmapTools.GetBitmapDiff(previousImg, currentImg, rightRect);
            center = BitmapTools.GetBitmapDiff(previousImg, currentImg, centerRect);

            //is Diff over trig?
            //return (diffPurcentage > trig && diffPurcentage < 100);
        }

        private void DoDraw()
        {
            Rectangle outlineZone = zone;
            outlineZone.Inflate(8, 8);
            painter.DrawRect(outlineZone);
            string message = string.Format("left={1} \t center={2} \t right={3} \t time={0}", elapsedMilliseconds, left, center, right);
            Rectangle rect = new Rectangle(zone.Location, zone.Size);
            rect.Offset(0, zone.Height * 2);
            //rect.Inflate(128, 0);
            painter.DrawString(message, rect);
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


        /*private void DoLoopNew()
        {
            if (IsMouse3Down() && IsMouseCentered(StateMachine.screenCenter) && !IsMouseMoving())
            {
                //is ref OK?
                if (IsCross(StateMachine.refBitMap, StateMachine.squarePixelSize))
                {
                    //get last
                    StateMachine.lastBitMap = CaptureBitmap(StateMachine.screenCenter, StateMachine.squarePixelSize);
                    //is last OK?
                    if (IsCross(StateMachine.lastBitMap, StateMachine.squarePixelSize))
                    {
                        //getdiff
                        float diff = GetBitmapDiff(StateMachine.refBitMap, StateMachine.lastBitMap, StateMachine.squarePixelSize);

                        //is Diff between 3% and 50% ?
                        if (diff > 3 && diff < 50)
                        {
                            //click
                            if (chrono.ElapsedMilliseconds > 250)
                            {
                                ClickMouse1();
                                chrono.Restart();
                            }
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
                    StateMachine.refBitMap = CaptureBitmap(StateMachine.screenCenter, StateMachine.squarePixelSize);
                }
            }
            else
            {
                //Delete img
                StateMachine.lastBitMap = null;
                StateMachine.refBitMap = null;
            }
        }*/



        /*   //Check if there is a single color cross
           private bool IsCross(Bitmap img, int squarePixelSize)
           {
               if (img == null) return false;

               Color center = img.GetPixel(squarePixelSize / 2, squarePixelSize / 2);
               Color left = img.GetPixel(squarePixelSize / 2 - 2, squarePixelSize / 2);
               Color right = img.GetPixel(squarePixelSize / 2 + 2, squarePixelSize / 2);

               if (!center.Equals(left)) return false;
               if (!center.Equals(right)) return false;

               return true;
           }
           */



    }
}
