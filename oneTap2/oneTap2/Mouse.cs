﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace oneTap2
{
    public static class Mouse
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, EntryPoint = "mouse_event")]
        public static extern void Mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern short GetKeyState(int virtualKey);

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        //True if mouse is centered
        public static bool IsMouseCentered(Point center)
        {
            Point currentPosition = new Point();
            GetCursorPos(ref currentPosition);
            return (currentPosition.Equals(center));
        }

        //True if mouse3 is down
        public static bool IsMouse3Down()
        {
            //MSB of byte is true 0000 0000 1000 0000
            //0x04 = virtual key of middle mouse buton
            //return (GetKeyState(0x04) & 0x80) == 128;
            //Mouse5
            return (GetKeyState(0x06) & 0x80) == 128;
        }

        //OneClick
        public static void ClickMouse1()
        {
            int MOUSEEVENTF_LEFTDOWN = 0x0002; /* left button down */
            int MOUSEEVENTF_LEFTUP = 0x0004; /* left button up */
            Mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            Mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
    }
}
