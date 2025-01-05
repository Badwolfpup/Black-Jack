namespace Black_Jack
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            menuStrip1 = new MenuStrip();
            startToolStripMenuItem = new ToolStripMenuItem();
            bytSpelareToolStripMenuItem = new ToolStripMenuItem();
            fyllPåMarkerToolStripMenuItem = new ToolStripMenuItem();
            hjälpToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { startToolStripMenuItem, hjälpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(4, 1, 0, 1);
            menuStrip1.Size = new Size(1041, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // startToolStripMenuItem
            // 
            startToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { bytSpelareToolStripMenuItem, fyllPåMarkerToolStripMenuItem });
            startToolStripMenuItem.Name = "startToolStripMenuItem";
            startToolStripMenuItem.Size = new Size(43, 22);
            startToolStripMenuItem.Text = "Start";
            // 
            // bytSpelareToolStripMenuItem
            // 
            bytSpelareToolStripMenuItem.Name = "bytSpelareToolStripMenuItem";
            bytSpelareToolStripMenuItem.Size = new Size(148, 22);
            bytSpelareToolStripMenuItem.Text = "Byt spelare";
            bytSpelareToolStripMenuItem.Click += bytSpelareToolStripMenuItem_Click;
            // 
            // fyllPåMarkerToolStripMenuItem
            // 
            fyllPåMarkerToolStripMenuItem.Name = "fyllPåMarkerToolStripMenuItem";
            fyllPåMarkerToolStripMenuItem.Size = new Size(148, 22);
            fyllPåMarkerToolStripMenuItem.Text = "Fyll på marker";
            fyllPåMarkerToolStripMenuItem.Click += fyllPåMarkerToolStripMenuItem_Click;
            // 
            // hjälpToolStripMenuItem
            // 
            hjälpToolStripMenuItem.Name = "hjälpToolStripMenuItem";
            hjälpToolStripMenuItem.Size = new Size(47, 22);
            hjälpToolStripMenuItem.Text = "Hjälp";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1041, 524);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip1;
            Margin = new Padding(2, 2, 2, 2);
            Name = "Form1";
            Text = "Black Jack by Badwolfpup";
            FormClosed += Form_Close_1;
            Load += Form1_Load_1;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem startToolStripMenuItem;
        private ToolStripMenuItem bytSpelareToolStripMenuItem;
        private ToolStripMenuItem fyllPåMarkerToolStripMenuItem;
        private ToolStripMenuItem hjälpToolStripMenuItem;
    }
}