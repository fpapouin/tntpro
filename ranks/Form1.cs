using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ranks
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BackColor = Color.Black;
            if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "png\\")))
                AddButtons();
        }

        private void AddButtons()
        {
            foreach (string fileName in Directory.GetFiles("png", "*png"))
            {
                Button b = new Button();
                flowLayoutPanel1.Controls.Add(b);
                b.Height = 100;
                b.Width = 200;
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
                b.Name = fileName.Replace("\\", "").Replace("png", "").Replace(".", "");
            }
        }
    }
}
