using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Diagnostics;

namespace oneTap2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Visible = false;

            //Shift zone to center
            zone.Offset(-zone.Size.Width / 2, -zone.Size.Height / 2);

            System.Timers.Timer timer = new System.Timers.Timer(10);
            timer.Elapsed += OnTimedEvent;
            timer.Start();

            System.Timers.Timer timer2 = new System.Timers.Timer(200);
            timer2.Elapsed += OnTimedEvent2;
            //timer2.Start();
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            (source as System.Timers.Timer).Stop();
            DoLoop();
            (source as System.Timers.Timer).Start();
        }

        private void OnTimedEvent2(Object source, ElapsedEventArgs e)
        {
            (source as System.Timers.Timer).Stop();
            DoDraw();
            (source as System.Timers.Timer).Start();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
