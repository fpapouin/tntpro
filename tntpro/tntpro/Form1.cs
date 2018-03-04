using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace tntpro
{
    public partial class Form1 : Form
    {

        List<Programme> ProgrammeList = new List<Programme>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BackColor = Color.White;
            flowLayoutPanel1.AllowDrop = true;
            flowLayoutPanel1.DragEnter += new DragEventHandler(DragEnters);
            flowLayoutPanel1.DragDrop += new DragEventHandler(DragDrops);

            if (File.Exists(@"tnt.m3u"))
                ProgrammeList = Helper.GetProgrammeList("tnt.m3u");

            if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "png\\")))
                AddButtons();

            string ip = new System.Net.WebClient().DownloadString("https://ipinfo.io/ip").Replace("\n", "");
            Helper.CopyOldFile();

            TextBox t = new TextBox();
            flowLayoutPanel1.Controls.Add(t);
            t.Text = "http://" + ip + ":8081";
            t.Width = 150;
            ListBox lb = new ListBox();
            flowLayoutPanel1.Controls.Add(lb);
            lb.Items.Add("UltraLow 320*200");
            lb.Items.Add("Low 640*360");
            lb.Items.Add("Normal 1280*720");
            lb.SelectedIndex = 2;

        }


        private void AddButtons()
        {
            foreach (string fileName in Directory.GetFiles("png", "*png"))
            {
                Button b = new Button();
                flowLayoutPanel1.Controls.Add(b);
                b.Height = 250;
                b.Width = 250;
                if (Screen.PrimaryScreen.Bounds.Width < 1900)
                {
                    b.Height = 150;
                    b.Width = 150;
                }
                b.BackgroundImage = Image.FromFile(fileName);
                b.BackgroundImageLayout = ImageLayout.Zoom;
                b.TabStop = false;
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.Name = fileName.Substring(7);
                b.Name = b.Name.Substring(0, b.Name.Length - 4);
                b.Name = b.Name.ToLower();
                b.Click += new EventHandler(Button_Click);
                Programme prg = ProgrammeList.FirstOrDefault(p => p.Name.Contains(b.Name));
                if (prg != null)
                {
                    ContextMenu cm = new ContextMenu();
                    MenuItem mi;
                    mi = cm.MenuItems.Add(prg.Frequency.ToString());
                    mi.Click += new EventHandler(MenuFrequency_Click);
                    mi = cm.MenuItems.Add(prg.Name);
                    mi.Click += new EventHandler(MenuName_Click);
                    mi = cm.MenuItems.Add(prg.Canal.ToString());
                    mi.Enabled = false;
                    mi = cm.MenuItems.Add(prg.Filename);
                    mi.Enabled = false;
                    b.ContextMenu = cm;
                }
            }
        }

        protected void Button_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;

            Programme prg = ProgrammeList.FirstOrDefault(p => p.Name.Contains(b.Name));
            if (prg != null)
            {
                Process.Start(Helper.GetVlcPath(), @" dvb-t:// :dvb-frequency=" + prg.Frequency + " :dvb-bandwidth=8 :program=" + prg.Canal + " --one-instance");
            }
        }

        protected void MenuFrequency_Click(object sender, EventArgs e)
        {
            MenuItem mi = sender as MenuItem;

            string cmdargs = Helper.BuildCmd(ProgrammeList.FindAll(p => p.Frequency == Int32.Parse(mi.Text)));
            Process.Start(Helper.GetVlcPath(), cmdargs);
        }

        protected void MenuName_Click(object sender, EventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            string cmdargs = Helper.BuildCmd(ProgrammeList.Where(p => p.Name.Contains(mi.Text)).ToList());
            Process.Start(Helper.GetVlcPath(), cmdargs);

        }

        protected void DragEnters(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Link;
        }

        protected void DragDrops(object sender, DragEventArgs e)
        {

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            string filename = files[0];
            string width = "1280";
            string height = "720";
            string dst = "8081";

            if ((flowLayoutPanel1.Controls[flowLayoutPanel1.Controls.Count - 1] as ListBox).SelectedIndex == 0)
            {
                width = "320";
                height = "200";
            }
            if ((flowLayoutPanel1.Controls[flowLayoutPanel1.Controls.Count - 1] as ListBox).SelectedIndex == 1)
            {
                width = "640";
                height = "360";
            }


            string cmdargs = "\"{0}\" ";

            cmdargs = cmdargs + "--sout=\"#transcode{vcodec=h264,vb=2000,venc=x264{profile=baseline},scale=Auto,";
            cmdargs = cmdargs + "width={1},height={2},acodec=mp3,ab=192,channels=2,samplerate=44100,scodec=dvbs,soverlay}";
            cmdargs = cmdargs + ":http{mux=ts,dst=:{3}/}\" --sout-keep";

            //cmdargs = String.Format(cmdargs, filename, width, height, dst);
            cmdargs = cmdargs.Replace("{0}", filename);
            cmdargs = cmdargs.Replace("{1}", width);
            cmdargs = cmdargs.Replace("{2}", height);
            cmdargs = cmdargs.Replace("{3}", dst);

            //vlc dvb-t://frequency=522000:bandwidth=8 --programs=1,2,3 :sout=#transcode{vcodec="h264",vb="600",scale="0.3",acodec="mp4a",ab="64",channels="2"}:std{access="http",mux="ts",dst="10.168.4.3:1231"}:sout-all

            //cmdargs = "dvb-t://frequency=474166000:bandwidth=8 --programs=260,261 :sout=#transcode{vcodec=h264,vb=2000,scale=Auto,acodec=mp3,ab=192,channels=2}:std{access=http,mux=ts,dst=8081}:sout-all";
            Process.Start(Helper.GetVlcPath(), cmdargs);
        }
    }

}
