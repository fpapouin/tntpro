using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace oneTap2
{
    public class Painter : IDisposable
    {
        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("User32.dll")]
        public static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);

        IntPtr desktopPtr = GetDC(IntPtr.Zero);
        public Graphics graphic;

        SolidBrush solidBrush = new SolidBrush(Color.Yellow);
        Pen pen;
        Font font = new Font("Arial", 8.0f, FontStyle.Bold);

        //ctor
        public Painter()
        {
            this.graphic = Graphics.FromHdc(this.desktopPtr);
            this.pen = new Pen(this.solidBrush);
        }

        public void DrawRect(Rectangle rect)
        {
            this.graphic.DrawRectangle(this.pen, rect);
        }

        public void DrawString(string message, Rectangle rect)
        {
            SolidBrush solidBrushRed = new SolidBrush(Color.Red);

            //graphic.FillRectangle(solidBrushRed, rect);
            graphic.DrawString(message, font, solidBrush, rect);
        }

        void IDisposable.Dispose()
        {
            graphic.Dispose();
            ReleaseDC(IntPtr.Zero, desktopPtr);
        }
    }
}
