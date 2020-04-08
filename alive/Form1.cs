using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace alive
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Visible = false;
            this.notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            this.notifyIcon1.Icon = this.Icon;
            this.notifyIcon1.Text = "alive";
            int sec = 1000;
            int min1 = sec * 60;
            System.Timers.Timer timer = new System.Timers.Timer(min1 * 25);
            timer.Elapsed += OnTimedEvent;
            timer.Start();
        }
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            (source as System.Timers.Timer).Stop();
            //Doloop();
            Console.Beep(37, 2500);
            (source as System.Timers.Timer).Start();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Doloop()
        {
            var pl = Process.GetProcessesByName("Shadow");
            foreach (var p in pl)
            {
                ushort WM_SYSKEYDOWN = 260;
                ushort WM_SYSKEYUP = 261;
                ushort WM_CHAR = 258;
                ushort WM_KEYDOWN = 256;
                ushort WM_KEYUP = 257;
                PostMessage(p.MainWindowHandle, WM_SYSKEYDOWN, (ushort)System.Windows.Forms.Keys.F1, 0);
            }
        }
    }
}
