using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

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
            System.Timers.Timer timer = new System.Timers.Timer(1000*60);
            timer.Elapsed += OnTimedEvent;
            timer.Start();
        }
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            (source as System.Timers.Timer).Stop();
            keybd_event((byte)Keys.PrintScreen, 0, 0, 0);
            keybd_event((byte)Keys.PrintScreen, 0, 2, 0);
            (source as System.Timers.Timer).Start();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
