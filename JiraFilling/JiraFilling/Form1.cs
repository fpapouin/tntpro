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

namespace JiraFilling
{
    public partial class Form1 : Form
    {
        IList<Action> ActionList;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ActionList = new List<Action>();
            listBox1.SelectedIndex = 0;
            listBox2.SelectedIndex = 0;
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Action A = new Action();
            A.Type = (ActionType)listBox2.SelectedIndex;
            A.Temps = double.Parse(listBox1.Text.Substring(0, 5), NumberStyles.Any, CultureInfo.InvariantCulture);
            A.Texte = textBox2.Text;
            A.Titre = textBox3.Text;
            A.CustomerCharge = checkBox1.Checked;
            ActionList.Add(A);
            Fill();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ActionList.Count != 0)
                ActionList.Remove(ActionList.Last());
            Fill();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkBox1.Visible = listBox2.SelectedIndex == 2;
            textBox3.Visible = listBox2.SelectedIndex == 2;
        }

        private void Fill()
        {
            textBox1.Text = "";

            if (ActionList.Where(p => p.Type == ActionType.Done).Count() > 0)
            {
                textBox1.AppendText("ST:(SpentTime)");
                textBox1.AppendText(Environment.NewLine);

                foreach (Action A in ActionList.Where(p => p.Type == ActionType.Done))
                {
                    textBox1.AppendText(A.Temps + "d " + A.Texte);
                    textBox1.AppendText(Environment.NewLine);
                }
                textBox1.AppendText(Environment.NewLine);
            }

            if (ActionList.Where(p => p.Type == ActionType.Todo).Count() > 0)
            {
                textBox1.AppendText("ETC:(estimated time to complete)");
                textBox1.AppendText(Environment.NewLine);
                foreach (Action A in ActionList.Where(p => p.Type == ActionType.Todo))
                {
                    textBox1.AppendText(A.Temps + "d " + A.Texte);
                    textBox1.AppendText(Environment.NewLine);
                }
                textBox1.AppendText(Environment.NewLine);
            }

            if (ActionList.Where(p => p.Type == ActionType.Defect).Count() > 0)
            {
                textBox1.AppendText("Issues:");
                textBox1.AppendText(Environment.NewLine);
                int i = 0;
                foreach (Action A in ActionList.Where(p => p.Type == ActionType.Defect))
                {
                    i++;
                    textBox1.AppendText("Defect" + i + ": " + A.Titre);
                    textBox1.AppendText(Environment.NewLine);

                    textBox1.AppendText("Raisons: " + A.Texte);
                    textBox1.AppendText(Environment.NewLine);

                    textBox1.AppendText("Estimation du retard engendré: " + A.Temps + "d");
                    textBox1.AppendText(Environment.NewLine);

                    if (A.CustomerCharge)
                        textBox1.AppendText("À la charge de Sagem");
                    else
                        textBox1.AppendText("À la charge de Ausy");
                    textBox1.AppendText(Environment.NewLine);
                    textBox1.AppendText(Environment.NewLine);
                }

            }
            textBox1.Text=textBox1.Text.Trim();

        }

    }
}
