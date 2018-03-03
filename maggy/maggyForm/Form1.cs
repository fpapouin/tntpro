using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Karna.Magnification;
using Gma.System.MouseKeyHook;
using System.Diagnostics;

namespace maggyForm
{
    public partial class Form1 : Form
    {
        private IKeyboardMouseEvents m_GlobalHook;
        private KeyboardHook keyboardHook;
        private int factorK = 1;

        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.MouseDown += m_GlobalHook_MouseDown;
            m_GlobalHook.MouseUp += m_GlobalHook_MouseUp;

            keyboardHook = new KeyboardHook(true);
            keyboardHook.KeyDown += Kh_KeyDown;

            notifyIcon1.Icon = new Icon("appicon.ico");
            if (!NativeMethods.MagInitialize())
                throw new Exception();
        }

        private void Kh_KeyDown(Keys key, bool Shift, bool Ctrl, bool Alt)
        {
            if (key == Keys.End) factorK = 1;
            if (key == Keys.Next) factorK = 4;
            if (key == Keys.PageUp) factorK = 8;
            Debug.WriteLine("The Key: " + key);
        }

        private void m_GlobalHook_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right) zoom(factorK);
        }

        private void m_GlobalHook_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right) zoom(1);
        }

        private void zoom(int k)
        {
            float magnificationFactor = k;
            int xDlg = (int)((float)NativeMethods.GetSystemMetrics(NativeMethods.SM_CXSCREEN) * (1.0 - (1.0 / magnificationFactor)) / 2.0);
            int yDlg = (int)((float)NativeMethods.GetSystemMetrics(NativeMethods.SM_CYSCREEN) * (1.0 - (1.0 / magnificationFactor)) / 2.0);
            NativeMethods.MagSetFullscreenTransform(magnificationFactor, xDlg, yDlg);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            NativeMethods.MagUninitialize();
            m_GlobalHook.MouseDown -= m_GlobalHook_MouseDown;
            m_GlobalHook.MouseUp -= m_GlobalHook_MouseUp;
            keyboardHook.KeyDown -= Kh_KeyDown;
            m_GlobalHook.Dispose();
        }
    }
}
