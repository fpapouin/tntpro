using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IhmMinMax
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                List<double> l = new List<double>();
                foreach (string s in this.textBox1.Text.Split('\n'))
                {
                    try
                    {
                        l.Add(double.Parse(s.Replace("\r", "").Replace('.', ',')));
                    }
                    catch (Exception)
                    {
                    }
                }

                textBox2.Text = l.Min().ToString().Replace(',', '.');
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                List<double> l = new List<double>();
                foreach (string s in this.textBox1.Text.Split('\n'))
                {
                    try
                    {
                        l.Add(double.Parse(s.Replace("\r", "").Replace('.', ',')));
                    }
                    catch (Exception)
                    {
                    }
                }

                textBox2.Text = l.Max().ToString().Replace(',', '.');
            }
            catch { }

        }
    }
}
