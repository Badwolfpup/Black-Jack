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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hjälpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bytSpelareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fyllPåMarkerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.hjälpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1562, 33);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bytSpelareToolStripMenuItem,
            this.fyllPåMarkerToolStripMenuItem});
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(64, 29);
            this.startToolStripMenuItem.Text = "Start";
            // 
            // hjälpToolStripMenuItem
            // 
            this.hjälpToolStripMenuItem.Name = "hjälpToolStripMenuItem";
            this.hjälpToolStripMenuItem.Size = new System.Drawing.Size(69, 29);
            this.hjälpToolStripMenuItem.Text = "Hjälp";
            // 
            // bytSpelareToolStripMenuItem
            // 
            this.bytSpelareToolStripMenuItem.Name = "bytSpelareToolStripMenuItem";
            this.bytSpelareToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.bytSpelareToolStripMenuItem.Text = "Byt spelare";
            this.bytSpelareToolStripMenuItem.Click += new System.EventHandler(this.bytSpelareToolStripMenuItem_Click);
            // 
            // fyllPåMarkerToolStripMenuItem
            // 
            this.fyllPåMarkerToolStripMenuItem.Name = "fyllPåMarkerToolStripMenuItem";
            this.fyllPåMarkerToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.fyllPåMarkerToolStripMenuItem.Text = "Fyll på marker";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1562, 786);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Black Jack by Badwolfpup";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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