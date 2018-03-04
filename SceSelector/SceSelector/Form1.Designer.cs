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
            this.ReplaceConfig = new System.Windows.Forms.CheckBox();
            this.UseConsole = new System.Windows.Forms.CheckBox();
            this.InstallDummyPlugin = new System.Windows.Forms.CheckBox();
            this.Launch = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
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
            // Launch
            // 
            this.Launch.Location = new System.Drawing.Point(317, 3);
            this.Launch.Name = "Launch";
            this.Launch.Size = new System.Drawing.Size(75, 23);
            this.Launch.TabIndex = 3;
            this.Launch.Text = "Launch";
            this.Launch.UseVisualStyleBackColor = true;
            this.Launch.Click += new System.EventHandler(this.Launch_Click);
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(410, 273);
            this.listBox1.TabIndex = 4;
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
            this.flowLayoutPanel1.Controls.Add(this.Launch);
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
            this.panel2.Size = new System.Drawing.Size(410, 273);
            this.panel2.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 309);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "SceSelector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
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
        private System.Windows.Forms.Button Launch;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
    }
}

