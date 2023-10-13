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
            this.nyRunda = new System.Windows.Forms.Button();
            this.draNyttKort = new System.Windows.Forms.Button();
            this.passa = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // nyRunda
            // 
            this.nyRunda.Location = new System.Drawing.Point(1156, 215);
            this.nyRunda.Name = "nyRunda";
            this.nyRunda.Size = new System.Drawing.Size(117, 30);
            this.nyRunda.TabIndex = 1;
            this.nyRunda.Text = "Börja spela";
            this.nyRunda.UseVisualStyleBackColor = true;
            this.nyRunda.Click += new System.EventHandler(this.nyRunda_Click);
            // 
            // draNyttKort
            // 
            this.draNyttKort.Location = new System.Drawing.Point(1156, 258);
            this.draNyttKort.Name = "draNyttKort";
            this.draNyttKort.Size = new System.Drawing.Size(117, 30);
            this.draNyttKort.TabIndex = 6;
            this.draNyttKort.Text = "Dra kort";
            this.draNyttKort.UseVisualStyleBackColor = true;
            this.draNyttKort.Click += new System.EventHandler(this.draNyttKort_Click);
            // 
            // passa
            // 
            this.passa.Location = new System.Drawing.Point(1161, 305);
            this.passa.Name = "passa";
            this.passa.Size = new System.Drawing.Size(117, 30);
            this.passa.TabIndex = 7;
            this.passa.Text = "Passa";
            this.passa.UseVisualStyleBackColor = true;
            this.passa.Click += new System.EventHandler(this.passa_Click_1);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(700, 64);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(157, 130);
            this.richTextBox1.TabIndex = 9;
            this.richTextBox1.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1324, 454);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.passa);
            this.Controls.Add(this.draNyttKort);
            this.Controls.Add(this.nyRunda);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;

            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.ResumeLayout(false);

        }

        #endregion
        private Button nyRunda;
        private Button draNyttKort;
        private Button passa;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private RichTextBox richTextBox1;
    }
}