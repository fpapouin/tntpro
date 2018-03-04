using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace AutoFillMCM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
            if (File.Exists("user.config"))
            {
                File.Copy("user.config", config.FilePath, true);
            }

            //Load Settings
            this.Size = Properties.Settings.Default.FormSize;
            this.Location = Properties.Settings.Default.FormLocation;
            this.textBox1.Text = Properties.Settings.Default.Text;
        }




        private void button1_Click(object sender, EventArgs e)
        {
            foreach (string line in textBox1.Text.Split('\n'))
            {
                DoExternalWrite(line);
            }
        }



        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        private static extern int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, string lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);


        private const int WM_SETTEXT = 12;
        private const uint WM_KEYDOWN = 0x0100;
        private const uint WM_KEYUP = 0x0101;
        private const uint WM_CHAR = 0x0102;
        private const int VK_TAB = 0x09;
        private const int VK_ENTER = 0x0D;
        private const int VK_UP = 0x26;
        private const int VK_DOWN = 0x28;
        private const int VK_RIGHT = 0x27;

        private void SendEnter(IntPtr child)
        {
            PostMessage(child, WM_KEYDOWN, (IntPtr)VK_ENTER, IntPtr.Zero);
            PostMessage(child, WM_CHAR, (IntPtr)VK_ENTER, IntPtr.Zero);
            SendMessage(child, WM_CHAR, (IntPtr)VK_ENTER, "");
            PostMessage(child, WM_KEYUP, (IntPtr)VK_ENTER, IntPtr.Zero);
        }


        public void DoExternalWrite(string text)
        {
            IntPtr parent = IntPtr.Zero;
            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.MainWindowTitle.Contains("Saisie code MCM"))
                {
                    parent = pList.MainWindowHandle;
                }
            }

            List<IntPtr> childHandles = GetChildWindows(parent);
            IntPtr child = IntPtr.Zero;
            foreach (IntPtr childHandle in childHandles)
            {
                StringBuilder className = new StringBuilder(1024);
                GetClassName(childHandle, className, 1024);
                if (className.ToString().Contains("WindowsForms10.EDIT.app"))
                {
                    child = childHandle;
                }
            }

            SendMessage(child, WM_SETTEXT, IntPtr.Zero, text);
            //   SendMessage(child, VK_ENTER, IntPtr.Zero, text);
            SendEnter(child);
        }


        public delegate bool Win32Callback(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.Dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr parentHandle, Win32Callback callback, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static public extern IntPtr GetClassName(IntPtr hWnd, System.Text.StringBuilder lpClassName, int nMaxCount);

        private static bool EnumWindow(IntPtr handle, IntPtr pointer)
        {
            GCHandle gch = GCHandle.FromIntPtr(pointer);
            List<IntPtr> list = gch.Target as List<IntPtr>;
            if (list == null)
                throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
            list.Add(handle);
            return true;
        }

        public static List<IntPtr> GetChildWindows(IntPtr parent)
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                Win32Callback childProc = new Win32Callback(EnumWindow);
                EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return result;
        }


        private void textBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            //Save Settings
            Properties.Settings.Default.FormSize = this.Size;
            Properties.Settings.Default.FormLocation = this.Location;
            Properties.Settings.Default.Text = this.textBox1.Text;
            Properties.Settings.Default.Save();

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);

            if (File.Exists(config.FilePath))
            {
                File.Copy(config.FilePath, "user.config", true);
            }
        }
    }
}
