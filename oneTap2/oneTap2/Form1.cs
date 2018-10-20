using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Diagnostics;
using System.IO;

namespace oneTap2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool checkCross = true;
        double timerInterval = 10;
        long minElapsedMs = 100;
        float minDiff = 20.0f;
        int recSize = 5;
        int mouseKey = 0x04;

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Visible = false;
            LoadConfig();
            System.Timers.Timer timer = new System.Timers.Timer(timerInterval);
            timer.Elapsed += OnTimedEvent;
            timer.Start();
        }

        private void LoadConfig()
        {
            if (File.Exists("oneTap2.cfg"))
            {
                string cfgFile = File.ReadAllText("oneTap2.cfg");
                cfgFile = cfgFile.Replace("\r", "");
                foreach (string line in cfgFile.Split('\n'))
                {
                    if (line.Contains("checkCross")) checkCross = Convert.ToBoolean(line.Split('=').Last());
                    if (line.Contains("timerInterval")) timerInterval = Convert.ToDouble(line.Split('=').Last());
                    if (line.Contains("minElapsedMs")) minElapsedMs = Convert.ToInt64(line.Split('=').Last());
                    if (line.Contains("minDiff")) minDiff = Convert.ToSingle(line.Split('=').Last());
                    if (line.Contains("recSize")) recSize = Convert.ToInt32(line.Split('=').Last());
                    if (line.Contains("mouseKey")) mouseKey = Convert.ToInt32(line.Split('=').Last(), 16);
                }
            }
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
