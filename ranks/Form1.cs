﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ranks
{
    struct Profile
    {
        public int wingman;
        public int compet;
        public string url;
    }

    public partial class Form1 : Form
    {
        readonly Dictionary<string, Profile> Accounts = new Dictionary<string, Profile>();
        readonly Dictionary<int, string> WingmanRanks = new Dictionary<int, string>();
        readonly Dictionary<int, string> Ranks = new Dictionary<int, string>();
        readonly List<string> Fulls = new List<string>();

        public Form1()
        {
            InitializeComponent();
            BackColor = Color.Black;
            WingmanRanks.Add(0, "unrank-wingman.png");
            WingmanRanks.Add(1, "silver-1-wingman.png");
            WingmanRanks.Add(2, "silver-2-wingman.png");
            WingmanRanks.Add(3, "silver-3-wingman.png");
            WingmanRanks.Add(4, "silver-4-wingman.png");
            WingmanRanks.Add(5, "silver-5-wingman.png");
            WingmanRanks.Add(6, "silver-elite-master-wingman.png");
            WingmanRanks.Add(7, "gold-nova-wingman.png");
            WingmanRanks.Add(8, "gold-nova-2-wingman.png");
            WingmanRanks.Add(9, "gold-nova-3-wingman.png");
            WingmanRanks.Add(10, "Gold-nova-master-wingman.png");
            WingmanRanks.Add(11, "master-guardian-1-wingman.png");
            WingmanRanks.Add(12, "master-guardian-2-wingman.png");
            WingmanRanks.Add(13, "master-guardian-elite-wingman.png");
            WingmanRanks.Add(14, "dinstinguished-master-guardian-wingman.png");
            WingmanRanks.Add(15, "legendary-eagle-wingman.png");
            WingmanRanks.Add(16, "legendary-eagle-master-wingman.png");
            WingmanRanks.Add(17, "supreme-elite-master-class-wingman.png");
            WingmanRanks.Add(18, "Global-elite-wingman.png");
            Ranks.Add(0, "unranked.png");
            Ranks.Add(1, "silver-1.png");
            Ranks.Add(2, "silver2.png");
            Ranks.Add(3, "silver-3.png");
            Ranks.Add(4, "silver4-csgo.png");
            Ranks.Add(5, "silver-5.png");
            Ranks.Add(6, "silver-6.png");
            Ranks.Add(7, "nova-1.png");
            Ranks.Add(8, "nova-2.png");
            Ranks.Add(9, "nova-3.png");
            Ranks.Add(10, "nova4.png");
            Ranks.Add(11, "master-gardian.png");
            Ranks.Add(12, "master-gardian-2.png");
            Ranks.Add(13, "master-gardian-elite.png");
            Ranks.Add(14, "sherif.png");
            Ranks.Add(15, "legendary-eagle.png");
            Ranks.Add(16, "legendary-eagle-master.png");
            Ranks.Add(17, "supreme-elite-master-class.png");
            Ranks.Add(18, "global-elite.png");

            Fulls.Add("wood-6.png");
            Fulls.Add("wood-6.png");
            Fulls.Add("wood-6.png");
            Fulls.Add("wood-6.png");
            Fulls.Add("wood-6.png");
            Fulls.Add("wood-6.png");
            Fulls.Add("wood-6.png");
            Fulls.Add("wood-6.png");
            Fulls.Add("wood-6.png");
            Fulls.Add("wood-6.png");
            Fulls.Add("wood-6.png");
            Fulls.Add("plastic-6.png");
            Fulls.Add("bronze-1.png");
            Fulls.Add("bronze-2.png");
            Fulls.Add("bronze-3.png");
            Fulls.Add("bronze-4.png");
            Fulls.Add("bronze-5.png");
            Fulls.Add("bronze-6.png");
            Fulls.Add("silver-1.png");
            Fulls.Add("silver2.png");
            Fulls.Add("silver-3.png");
            Fulls.Add("silver4-csgo.png");
            Fulls.Add("silver-5.png");
            Fulls.Add("silver-6.png");
            Fulls.Add("nova-1.png");
            Fulls.Add("nova-2.png");
            Fulls.Add("nova-3.png");
            Fulls.Add("nova4.png");
            Fulls.Add("master-gardian.png");
            Fulls.Add("master-gardian-2.png");
            Fulls.Add("master-gardian-elite.png");
            Fulls.Add("master-guardian-king.png");
            Fulls.Add("black-1.png");
            Fulls.Add("black-2.png");
            Fulls.Add("black-3.png");
            Fulls.Add("black-4.png");
            Fulls.Add("black-5.png");
            Fulls.Add("black-6.png");
            Fulls.Add("black-7.png");
            Fulls.Add("sherif.png");
            Fulls.Add("legendary-eagle.png");
            Fulls.Add("legendary-eagle-master.png");
            Fulls.Add("supreme-elite-master-class.png");
            Fulls.Add("global-elite.png");
            Fulls.Add("mega-elite.png");
            Fulls.Add("monster-elite.png");
            Fulls.Reverse();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tableLayoutPanel1.ColumnCount = 12;
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            LoadAccounts();

            if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "png\\")))
            {
                FindAvatar();
                AddAvatarButtons();
            }
            this.Scale(new SizeF(0.7F, 0.7F));
            WindowState = FormWindowState.Maximized;
            /*int x = 319;
            int y = 35;
            int w = 107;
            int h = 42;
            using (Bitmap bmp = new Bitmap(w, h))
            {
                Graphics g = Graphics.FromImage(bmp);
                g.DrawImage(Image.FromFile(@"C:\Users\Shadow\Documents\tntpro\ranks\eRtu8qYH4HROcwXiMaC-bxYUWNKfBOH7UcCwZnOtsj0.jpg"),
                    new Rectangle(0, 0, w, h),
                    new Rectangle(x, y, w, h),
                    GraphicsUnit.Pixel);
                bmp.Save(@"black-6.png", ImageFormat.Png);
            }*/
            //*
            //SaveRanks();
            SaveRanks("ranks.png", 300, 65);
            Upload();
            Application.Exit();//*/
        }

        private void SaveRanks()
        {
            SaveRanks("ranks1.png");

            tableLayoutPanel1.Controls.OfType<Button>().Take(8 * 9).ToList().ForEach(b => b.Visible = false);
            SaveRanks("ranks2.png");

            tableLayoutPanel1.Controls.OfType<Button>().Take(8 * 18).ToList().ForEach(b => b.Visible = false);
            SaveRanks("ranks3.png", removedHeight: 70 * 11);

            Image img1 = Image.FromFile("ranks1.png");
            Image img2 = Image.FromFile("ranks2.png");
            Image img3 = Image.FromFile("ranks3.png");
            int separator = 1;
            using (Bitmap bmp = new Bitmap(img1.Width + img2.Width + separator, img1.Height + img3.Height + separator))
            {
                Graphics g = Graphics.FromImage(bmp);
                g.DrawImage(img1, 0, 0);
                g.DrawImage(img2, img1.Width + separator, 0);
                g.DrawImage(img3, img1.Width / 2, img1.Height + separator);
                bmp.Save(@"ranks.png", ImageFormat.Png);
            }
            img1.Dispose();
            img2.Dispose();
            img3.Dispose();
            File.Delete("ranks1.png");
            File.Delete("ranks2.png");
            File.Delete("ranks3.png");
        }

        private void SaveRanks(string filename, int removedWidth = 200, int removedHeight = 70)
        {
            using (Bitmap bmp = new Bitmap(tableLayoutPanel1.Width - removedWidth, tableLayoutPanel1.Height - removedHeight))
            {
                tableLayoutPanel1.DrawToBitmap(bmp, new Rectangle(Point.Empty, bmp.Size));
                bmp.Save(filename, ImageFormat.Png);
            }
        }

        private void LoadAccounts()
        {
            foreach (string data in File.ReadAllText("ranks.txt").Replace("\r", "").Split("`".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                if (data.Replace("\n", "") != string.Empty)
                {
                    string[] s = data.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (Accounts.ContainsKey(s[0]))
                    {
                        Accounts[s[0]] = new Profile() { wingman = int.Parse(s[1]), compet = int.Parse(s[2]), url = s[3] };
                    }
                    else
                    {
                        Accounts.Add(s[0], new Profile() { wingman = int.Parse(s[1]), compet = int.Parse(s[2]), url = s[3] });
                    }
                }
            }
        }

        private void AddAvatarButtons()
        {
            int i = 0;
            int position = 0;
            int lastPoint = 0;
            //foreach (var account in Accounts.OrderByDescending(a => a.Value.compet + a.Value.wingman).ThenByDescending(a => a.Value.wingman))
            foreach (var account in Accounts.OrderByDescending(a => a.Value.compet).ThenByDescending(a => a.Value.wingman))
            {
                i++;
                if ((account.Value.compet + account.Value.wingman) != lastPoint)
                {
                    position = i;
                }
                lastPoint = account.Value.compet + account.Value.wingman;
                if (lastPoint == 0) // for not ranked
                {
                    i = 0;
                    continue;
                }

                //Add point
                Button r = new Button();
                tableLayoutPanel1.Controls.Add(r);
                r.Height = 100;
                r.Width = 200;
                //r.BackgroundImage = Image.FromFile("png/" + Fulls[i]);
                r.BackgroundImageLayout = ImageLayout.Zoom;
                r.TabStop = false;
                r.FlatStyle = FlatStyle.Flat;
                r.FlatAppearance.BorderSize = 0;
                r.Name = account.Key;
                r.Text = "" + position;
                r.TextAlign = ContentAlignment.BottomRight;
                r.ForeColor = Color.BlueViolet;
                r.Font = new Font(r.Font.FontFamily, 35, FontStyle.Bold);
                if (position <= 30)
                    r.ForeColor = Color.OrangeRed;
                if (position <= 20)
                    r.ForeColor = Color.Green;
                if (position <= 10)
                    r.ForeColor = Color.RoyalBlue;
                if (position <= 3)
                    r.ForeColor = Color.Yellow;

                //r.Font = new Font(r.Font.FontFamily, 70, FontStyle.Bold);

                //Add avatar
                Button b = new Button();
                tableLayoutPanel1.Controls.Add(b);
                b.Height = 100;
                b.Width = 100;
                b.BackgroundImage = Image.FromFile("png/" + account.Key + ".png");
                b.BackgroundImageLayout = ImageLayout.Zoom;
                b.TabStop = false;
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.Name = account.Key;
                new ToolTip().SetToolTip(b, b.Name);

                //Add compet
                Button c = new Button();
                tableLayoutPanel1.Controls.Add(c);
                c.Height = 100;
                c.Width = 200;
                c.BackgroundImage = Image.FromFile("png/" + Ranks[account.Value.compet]);
                c.BackgroundImageLayout = ImageLayout.Zoom;
                c.TabStop = false;
                c.FlatStyle = FlatStyle.Flat;
                c.FlatAppearance.BorderSize = 0;
                c.Name = account.Key;

                //Add wingman
                Button w = new Button();
                tableLayoutPanel1.Controls.Add(w);
                w.Height = 100;
                w.Width = 200;
                w.BackgroundImage = Image.FromFile("png/" + WingmanRanks[account.Value.wingman]);
                w.BackgroundImageLayout = ImageLayout.Zoom;
                w.TabStop = false;
                w.FlatStyle = FlatStyle.Flat;
                w.FlatAppearance.BorderSize = 0;
                w.Name = account.Key;
            }
        }

        private void FindAvatar()
        {
            foreach (var account in Accounts)
            {
                string avatarFileName = "png/" + account.Key + ".png";
                if (!File.Exists(avatarFileName))
                {
                    using (System.Net.WebClient wc = new System.Net.WebClient())
                    {
                        string webData = wc.DownloadString(account.Value.url);
                        foreach (string line in webData.Split('\n'))
                        {
                            if (line.Contains("playerAvatarAutoSizeInner"))
                            {
                                int ss = line.IndexOf("https");
                                int se = line.IndexOf(".jpg");
                                wc.DownloadFile(line.Substring(ss, se - ss + 4), avatarFileName);
                            }
                        }
                    }
                }
            }
        }

        private void Upload()
        {
            if (Environment.GetCommandLineArgs().Length == 3)
            {
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    wc.Credentials = new System.Net.NetworkCredential(Environment.GetCommandLineArgs()[1], Environment.GetCommandLineArgs()[2]);
                    wc.UploadFile("ftp://ftpperso.free.fr/ranks.png", System.Net.WebRequestMethods.Ftp.UploadFile, "ranks.png");
                }
            }
        }
    }
}
