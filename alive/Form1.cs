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
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

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
            //System.Timers.Timer timer = new System.Timers.Timer(min1 * 25);
            System.Timers.Timer timer = new System.Timers.Timer(sec*3);
            timer.Elapsed += OnTimedEvent;
            timer.Start();
        }
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            (source as System.Timers.Timer).Stop();
            Doloop();
            (source as System.Timers.Timer).Start();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Doloop()
        {
            const UInt32 WM_KEYDOWN = 0x0100;
            const int VK_F5 = 0x74;

            List<Process> processes = Process.GetProcessesByName("notepad").ToList();
            processes.AddRange(Process.GetProcessesByName("Shadow").ToList());
            foreach (Process proc in processes)
                PostMessage(proc.MainWindowHandle, WM_KEYDOWN, VK_F5, 0);
        }
    
    }
}
