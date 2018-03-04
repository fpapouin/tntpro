using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace csvUsFrConvertor
{
    public partial class csvUsFrConvertor : Form
    {
        bool toUs = true;
        public csvUsFrConvertor()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (toUs)
                label1.Text = "US->FR\n" + ", -> ;\n" + ". -> ,\n" + "comma -> semicolon\n";
            else
                label1.Text = "FR->US\n" + "; -> ,\n" + ", -> .\n" + "semicolon -> comma\n";

            toUs = !toUs;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Convert();
            }
            catch (Exception)
            {


            }
        }

        private void Convert()
        {
            if (toUs)
                textBox2.Text = textBox1.Text.Replace(",", ".").Replace(";", ",");
            else
                textBox2.Text = textBox1.Text.Replace(".", ",").Replace(",", ";");
        }
    }
}
