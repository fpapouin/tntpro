using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;

namespace oneTapForm
{
    public partial class Form1 : Form
    {
        private IKeyboardMouseEvents m_GlobalHook;
        private KeyboardHook keyboardHook;

        private const int MOUSEEVENTF_LEFTDOWN = 0x0002; /* left button down */
        private const int MOUSEEVENTF_LEFTUP = 0x0004; /* left button up */

        //  if (!File.Exists("log.txt")) File.CreateText("log.txt");
        //File.AppendAllText("log.txt", diff.ToString());
        //bmpScreenCapture.Save("snap" + trig + ".bmp");

        static class StateMachine
        {
            public static int cfgSize = 10;

            public static bool isMouse3Down = false;
            public static bool doLoop = true;
            public static Bitmap refBitMap;
            public static Bitmap lastBitMap;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.Icon = new Icon("appicon.ico");
            m_GlobalHook = Hook.GlobalEvents();
            keyboardHook = new KeyboardHook(true);

            //Disabled
            // keyboardHook.KeyDown += Kh_KeyDown;

            m_GlobalHook.MouseDown += M_GlobalHook_MouseDown;
            m_GlobalHook.MouseUp += M_GlobalHook_MouseUp;

            new System.Threading.Thread(() => DoThread()).Start();
        }

        private void DoThread()
        {
            while (StateMachine.doLoop)
            {
                System.Threading.Thread.Sleep(10);
                if (StateMachine.isMouse3Down)
                {
                    //is ref OK?
                    if (IsCross(StateMachine.refBitMap, StateMachine.cfgSize))
                    {
                        //get last
                        StateMachine.lastBitMap = GetBitmap(StateMachine.cfgSize);
                        //is last OK?
                        if (IsCross(StateMachine.lastBitMap, StateMachine.cfgSize))
                        {
                            //getdiff
                            float diff = GetBitmapDiff(StateMachine.refBitMap, StateMachine.lastBitMap, StateMachine.cfgSize);

                            //is Diff between 2% and 50% ?
                            if (diff > 3 && diff < 50)
                            {
                                //click
                                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
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
                        StateMachine.refBitMap = GetBitmap(StateMachine.cfgSize);
                    }
                }
                else
                {
                    //Delete img
                    StateMachine.lastBitMap = null;
                    StateMachine.refBitMap = null;
                }
            }
        }

        //Disabled
        private void Kh_KeyDown(Keys key, bool Shift, bool Ctrl, bool Alt)
        {
            Debug.WriteLine("The Key: " + key);

            if (key == Keys.End)
            {
            }

            if (key == Keys.PageUp)
            {
            }

            if (key == Keys.Next)
            {
            }
        }

        private void M_GlobalHook_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Debug.WriteLine("The Mousedown: " + e.Button);
            if (e.Button == System.Windows.Forms.MouseButtons.Middle) StateMachine.isMouse3Down = true;
        }

        private void M_GlobalHook_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Debug.WriteLine("The Mouseup: " + e.Button);
            if (e.Button == System.Windows.Forms.MouseButtons.Middle) StateMachine.isMouse3Down = false;
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

        //Check if there is a dark cross
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

        private void Log(string s)
        {
            if (!File.Exists("log.txt")) File.CreateText("log.txt");
            File.AppendAllText("log.txt", s + Environment.NewLine);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StateMachine.doLoop = false;
            keyboardHook.KeyDown -= Kh_KeyDown;
            m_GlobalHook.Dispose();
        }
    }
}
