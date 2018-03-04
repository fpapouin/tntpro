using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace SceSelector
{
    public partial class Form1 : Form
    {
        string binName = "smartCityExplorer.exe";

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
            this.UseConsole.Checked = Properties.Settings.Default.UseConsole;
            this.ReplaceConfig.Checked = Properties.Settings.Default.ReplaceConfig;
            this.InstallDummyPlugin.Checked = Properties.Settings.Default.InstallDummyPlugin;
            FillList();
            this.listBox1.SelectedItem = Properties.Settings.Default.SelectedItem;
            this.ActiveControl = this.Launch;
        }

        private void FillList()
        {
            //Search exe in ProgramFiles
            string binFolderRoot = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Siradel\";
            foreach (string s in Directory.GetFiles(binFolderRoot, binName, SearchOption.AllDirectories))
            {
                this.listBox1.Items.Add(s);
            }

            //Search exe in all real drive in root/Git/
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType != DriveType.Network)
                {
                    string devRoot = drive.RootDirectory + "Git";
                    if (Directory.Exists(devRoot))
                        foreach (string s in Directory.GetFiles(devRoot, binName, SearchOption.AllDirectories))
                        {
                            if (!s.Contains("obj") && !s.Contains("migration"))
                                this.listBox1.Items.Add(s);
                        }
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Save Settings
            Properties.Settings.Default.FormSize = this.Size;
            Properties.Settings.Default.FormLocation = this.Location;
            Properties.Settings.Default.UseConsole = this.UseConsole.Checked;
            Properties.Settings.Default.ReplaceConfig = this.ReplaceConfig.Checked;
            Properties.Settings.Default.InstallDummyPlugin = this.InstallDummyPlugin.Checked;
            Properties.Settings.Default.SelectedItem = (string)this.listBox1.SelectedItem;
            Properties.Settings.Default.Save();

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);

            if (File.Exists(config.FilePath))
            {
                File.Copy(config.FilePath, "user.config", true);
            }
        }

        private void Launch_Click(object sender, EventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            string Rrph;

            if (ReplaceConfig.Checked)
                Rrph = SetConfig();

            if (InstallDummyPlugin.Checked)
                SetPlugin();

            if (this.UseConsole.Checked)
            {
                psi.WorkingDirectory = Path.GetDirectoryName((string)this.listBox1.SelectedItem);
                psi.FileName = "cmd.exe";
                psi.Arguments = "/C " + binName;
            }
            else
            {
                psi.FileName = (string)this.listBox1.SelectedItem;
                psi.WorkingDirectory = Path.GetPathRoot(psi.FileName);
            }

            Process.Start(psi);
            //Todo restor config Rrph if not null. not sure with defaultConfig
        }

        private void SetPlugin()
        {

            throw new NotImplementedException();
        }
        /// <summary>
        /// Look for R.r.p.h in exe path
        /// Replace Config folder by ConfigR.r.p.h if exist
        /// </summary>
        private string SetConfig()
        {
            string configFolderRoot = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Siradel\SmartCityExplorer\Config";
            string Rrph = Regex.Match((string)this.listBox1.SelectedItem, @"\d+\.\d+\.\d+\.\d+").ToString();

            if (!string.IsNullOrEmpty(Rrph) && Directory.Exists(configFolderRoot) && Directory.Exists(configFolderRoot + Rrph))
            {
                Directory.Delete(configFolderRoot, true);
                CopyDirectory(configFolderRoot + Rrph, configFolderRoot);
            }
            return Rrph;
        }

        private void CopyDirectory(string SourcePath, string DestinationPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
        }
    }
}
