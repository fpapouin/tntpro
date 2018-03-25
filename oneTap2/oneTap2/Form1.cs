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
            System.Timers.Timer timer = new System.Timers.Timer(10);
            timer.Elapsed += OnTimedEvent;
            timer.Start();
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            (source as System.Timers.Timer).Stop();
            DoLoop();
            (source as System.Timers.Timer).Start();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
