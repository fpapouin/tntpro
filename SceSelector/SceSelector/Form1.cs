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
        string binFolderRoot = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Siradel\";

        class ScePro
        {
            public string BinPath { get; set; }
            public string UninstallerPath { get; set; }
            public string PluginName { get; set; }
            public string UninstallerPluginPath { get; set; }

            public override string ToString()
            {
                return BinPath;
            }
        }

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

            // Load Settings
            this.Size = Properties.Settings.Default.FormSize;
            this.Location = Properties.Settings.Default.FormLocation;
            this.UseConsole.Checked = Properties.Settings.Default.UseConsole;
            this.ReplaceConfig.Checked = Properties.Settings.Default.ReplaceConfig;
            this.InstallDummyPlugin.Checked = Properties.Settings.Default.InstallDummyPlugin;
            this.ActiveControl = this.Launch;

            FillList();
        }

        private void FillList()
        {
            this.listBox1.SelectedItem = null;
            this.listBox1.Items.Clear();

            //Search exe in ProgramFiles
            foreach (string binPath in Directory.GetFiles(binFolderRoot, binName, SearchOption.AllDirectories))
            {
                this.listBox1.Items.Add(new ScePro() { BinPath = binPath });
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
            Properties.Settings.Default.Save();

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
            if (File.Exists(config.FilePath))
            {
                File.Copy(config.FilePath, "user.config", true);
            }
        }

        private void Launch_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItem != null)
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.WorkingDirectory = Path.GetDirectoryName(this.listBox1.SelectedItem.ToString());

                if (ReplaceConfig.Checked)
                    SetConfig();

                if (InstallDummyPlugin.Checked)
                    SetPlugin();

                if (this.UseConsole.Checked)
                {
                    psi.FileName = "cmd.exe";
                    psi.Arguments = "/C " + binName;
                }
                else
                {
                    psi.FileName = this.listBox1.SelectedItem.ToString();
                }

                if (this.listBox1.SelectedItem != null)
                    Process.Start(psi);
            }
        }

        private void SetPlugin()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Copy sce_online_services.xml in every subfolder
        /// </summary>
        private void SetConfig()
        {
            string configFolderRoot = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Siradel\ScePro\Config";

            foreach (string configFolder in Directory.EnumerateDirectories(configFolderRoot))
            {
                if (File.Exists(Path.Combine(configFolderRoot, "sce_online_services.xml")))
                    File.Copy(Path.Combine(configFolderRoot, "sce_online_services.xml"),
                        Path.Combine(configFolder, "sce_online_services.xml"), true);
            }
        }

        /// <summary>
        /// Look for R.r.p.h in exe path
        /// Replace sce_online_services.xml in Config\R.r.p.h if exist
        /// </summary>
        private void SetConfigOld()
        {
            string configFolderRoot = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Siradel\ScePro\Config";
            string Rrph = Regex.Match((string)this.listBox1.SelectedItem, @"\d+\.\d+\.\d+\.\d+").ToString();

            // if (!string.IsNullOrEmpty(Rrph) && Directory.Exists(configFolderRoot) && Directory.Exists(configFolderRoot + Rrph))
            if (Directory.Exists(Path.Combine(configFolderRoot, Rrph)))
            {
                if (File.Exists(Path.Combine(configFolderRoot, "sce_online_services.xml")))
                    File.Copy(Path.Combine(configFolderRoot, "sce_online_services.xml"),
                        Path.Combine(configFolderRoot, Rrph, "sce_online_services.xml"), true);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var proItem = this.contextMenuStrip1.Items[0] as ToolStripDropDownItem;
            var proUnins = proItem.DropDown.Items[0];
            var plugItem = this.contextMenuStrip1.Items[1] as ToolStripDropDownItem;
            var plugUnins = plugItem.DropDown.Items[0];

            proItem.Visible = false;
            proUnins.Visible = false;
            plugItem.Visible = false;
            plugUnins.Visible = false;

            if (this.listBox1.SelectedItem != null)
            {
                ScePro scePro = this.listBox1.SelectedItem as ScePro;
                proItem.Visible = true;
                proItem.Text = "ScePro " + FileVersionInfo.GetVersionInfo(scePro.BinPath).FileVersion;

                // Update sceProItem and contextMenu
                string binDir = Path.GetDirectoryName(scePro.BinPath);

                if (File.Exists(Path.Combine(binDir, @"..\", "unins000.exe")))
                {
                    scePro.UninstallerPath = Path.GetFullPath(Path.Combine(binDir, @"..\", "unins000.exe"));
                    proUnins.Visible = true;
                    proUnins.Text = "Uninstall " + scePro.UninstallerPath;
                }

                //Plugins
                foreach (string plugDir in Directory.GetDirectories(Path.Combine(binDir, "plugins")))
                {
                    scePro.PluginName = (new DirectoryInfo(plugDir)).Name;
                    plugItem.Visible = true;
                    plugItem.Text = scePro.PluginName;

                    // Version
                    if (File.Exists(Path.Combine(binDir, "plugins", scePro.PluginName, "Siradel.S_IoT.Plugin.dll")))
                    {
                        plugItem.Text = scePro.PluginName + " " + FileVersionInfo.GetVersionInfo(Path.Combine(binDir, "plugins", scePro.PluginName, "Siradel.S_IoT.Plugin.dll")).FileVersion;
                    }
                    else if (File.Exists(Path.Combine(binDir, "plugins", scePro.PluginName, "Siradel.S_5GChannel.Plugin.dll")))
                    {
                        plugItem.Text = scePro.PluginName + " " + FileVersionInfo.GetVersionInfo(Path.Combine(binDir, "plugins", scePro.PluginName, "Siradel.S_5GChannel.Plugin.dll")).FileVersion;
                    }
                    else if (File.Exists(Path.Combine(binDir, "plugins", scePro.PluginName, "S_Backhaul.dll")))
                    {
                        plugItem.Text = scePro.PluginName + " " + FileVersionInfo.GetVersionInfo(Path.Combine(binDir, "plugins", scePro.PluginName, "S_Backhaul.dll")).FileVersion;
                    }

                    if (File.Exists(Path.Combine(plugDir, "unins000.exe")))
                    {
                        scePro.UninstallerPluginPath = Path.Combine(plugDir, "unins000.exe");
                        plugUnins.Visible = true;
                        plugUnins.Text = "Uninstall " + scePro.UninstallerPluginPath;
                    }
                }
            }
        }

        private void uninstallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScePro current = listBox1.SelectedItem as ScePro;
            Process.Start(current.UninstallerPath, @" /VERYSILENT /NORESTART");
            this.listBox1.SelectedItem = null;
        }

        private void uninstallToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ScePro current = listBox1.SelectedItem as ScePro;
            Process.Start(current.UninstallerPluginPath, @" /VERYSILENT /NORESTART");
            this.listBox1.SelectedItem = null;
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            this.listBox1.SelectedIndex = this.listBox1.IndexFromPoint(e.X, e.Y);
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FillList();
        }
    }
}
