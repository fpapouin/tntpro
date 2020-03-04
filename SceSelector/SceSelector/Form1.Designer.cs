namespace SceSelector
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ReplaceConfig = new System.Windows.Forms.CheckBox();
            this.UseConsole = new System.Windows.Forms.CheckBox();
            this.InstallDummyPlugin = new System.Windows.Forms.CheckBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sdsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uninstallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deuxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uninstallToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.installToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ReplaceConfig
            // 
            this.ReplaceConfig.AutoSize = true;
            this.ReplaceConfig.Location = new System.Drawing.Point(92, 3);
            this.ReplaceConfig.Name = "ReplaceConfig";
            this.ReplaceConfig.Size = new System.Drawing.Size(96, 17);
            this.ReplaceConfig.TabIndex = 0;
            this.ReplaceConfig.Text = "ReplaceConfig";
            this.ReplaceConfig.UseVisualStyleBackColor = true;
            // 
            // UseConsole
            // 
            this.UseConsole.AutoSize = true;
            this.UseConsole.Location = new System.Drawing.Point(3, 3);
            this.UseConsole.Name = "UseConsole";
            this.UseConsole.Size = new System.Drawing.Size(83, 17);
            this.UseConsole.TabIndex = 1;
            this.UseConsole.Text = "UseConsole";
            this.UseConsole.UseVisualStyleBackColor = true;
            // 
            // InstallDummyPlugin
            // 
            this.InstallDummyPlugin.AutoSize = true;
            this.InstallDummyPlugin.Enabled = false;
            this.InstallDummyPlugin.Location = new System.Drawing.Point(194, 3);
            this.InstallDummyPlugin.Name = "InstallDummyPlugin";
            this.InstallDummyPlugin.Size = new System.Drawing.Size(117, 17);
            this.InstallDummyPlugin.TabIndex = 2;
            this.InstallDummyPlugin.Text = "InstallDummyPlugin";
            this.InstallDummyPlugin.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(410, 76);
            this.listBox1.TabIndex = 4;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            this.listBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sdsToolStripMenuItem,
            this.deuxToolStripMenuItem,
            this.refreshToolStripMenuItem,
            this.installToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 114);
            // 
            // sdsToolStripMenuItem
            // 
            this.sdsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uninstallToolStripMenuItem});
            this.sdsToolStripMenuItem.Name = "sdsToolStripMenuItem";
            this.sdsToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.sdsToolStripMenuItem.Text = "ScePro";
            this.sdsToolStripMenuItem.Visible = false;
            // 
            // uninstallToolStripMenuItem
            // 
            this.uninstallToolStripMenuItem.Name = "uninstallToolStripMenuItem";
            this.uninstallToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.uninstallToolStripMenuItem.Text = "Uninstall";
            this.uninstallToolStripMenuItem.Click += new System.EventHandler(this.uninstallToolStripMenuItem_Click);
            // 
            // deuxToolStripMenuItem
            // 
            this.deuxToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uninstallToolStripMenuItem1});
            this.deuxToolStripMenuItem.Name = "deuxToolStripMenuItem";
            this.deuxToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.deuxToolStripMenuItem.Text = "Plugin";
            this.deuxToolStripMenuItem.Visible = false;
            // 
            // uninstallToolStripMenuItem1
            // 
            this.uninstallToolStripMenuItem1.Name = "uninstallToolStripMenuItem1";
            this.uninstallToolStripMenuItem1.Size = new System.Drawing.Size(120, 22);
            this.uninstallToolStripMenuItem1.Text = "Uninstall";
            this.uninstallToolStripMenuItem1.Click += new System.EventHandler(this.uninstallToolStripMenuItem1_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(410, 36);
            this.panel1.TabIndex = 5;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.UseConsole);
            this.flowLayoutPanel1.Controls.Add(this.ReplaceConfig);
            this.flowLayoutPanel1.Controls.Add(this.InstallDummyPlugin);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(410, 36);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.listBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 36);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(410, 76);
            this.panel2.TabIndex = 6;
            // 
            // installToolStripMenuItem
            // 
            this.installToolStripMenuItem.Name = "installToolStripMenuItem";
            this.installToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.installToolStripMenuItem.Text = "Install";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 112);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "SceSelector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox ReplaceConfig;
        private System.Windows.Forms.CheckBox UseConsole;
        private System.Windows.Forms.CheckBox InstallDummyPlugin;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem sdsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deuxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uninstallToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uninstallToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem installToolStripMenuItem;
    }
}

