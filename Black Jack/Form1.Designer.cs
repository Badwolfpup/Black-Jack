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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.nyRunda = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.draNyttKort = new System.Windows.Forms.Button();
            this.passa = new System.Windows.Forms.Button();
            this.chip5dollar = new System.Windows.Forms.PictureBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.chip10dollar = new System.Windows.Forms.PictureBox();
            this.chip25dollar = new System.Windows.Forms.PictureBox();
            this.chip50dollar = new System.Windows.Forms.PictureBox();
            this.chip100dollar = new System.Windows.Forms.PictureBox();
            this.chip500dollar = new System.Windows.Forms.PictureBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.chip5dollar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chip10dollar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chip25dollar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chip50dollar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chip100dollar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chip500dollar)).BeginInit();
            this.SuspendLayout();
            // 
            // nyRunda
            // 
            this.nyRunda.Location = new System.Drawing.Point(876, 150);
            this.nyRunda.Name = "nyRunda";
            this.nyRunda.Size = new System.Drawing.Size(111, 33);
            this.nyRunda.TabIndex = 1;
            this.nyRunda.Text = "Börja spela";
            this.nyRunda.UseVisualStyleBackColor = true;
            this.nyRunda.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(271, 133);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Bank";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(271, 425);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "Spelare";
            // 
            // draNyttKort
            // 
            this.draNyttKort.Location = new System.Drawing.Point(876, 198);
            this.draNyttKort.Name = "draNyttKort";
            this.draNyttKort.Size = new System.Drawing.Size(111, 33);
            this.draNyttKort.TabIndex = 6;
            this.draNyttKort.Text = "Dra kort";
            this.draNyttKort.UseVisualStyleBackColor = true;
            this.draNyttKort.Click += new System.EventHandler(this.button2_Click);
            // 
            // passa
            // 
            this.passa.Location = new System.Drawing.Point(880, 250);
            this.passa.Name = "passa";
            this.passa.Size = new System.Drawing.Size(111, 33);
            this.passa.TabIndex = 7;
            this.passa.Text = "Passa";
            this.passa.UseVisualStyleBackColor = true;
            this.passa.Click += new System.EventHandler(this.passa_Click);
            // 
            // chip5dollar
            // 
            this.chip5dollar.InitialImage = ((System.Drawing.Image)(resources.GetObject("chip5dollar.InitialImage")));
            this.chip5dollar.Location = new System.Drawing.Point(757, 425);
            this.chip5dollar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chip5dollar.Name = "chip5dollar";
            this.chip5dollar.Size = new System.Drawing.Size(57, 67);
            this.chip5dollar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.chip5dollar.TabIndex = 8;
            this.chip5dollar.TabStop = false;
            this.chip5dollar.Click += new System.EventHandler(this.chip5dollar_Click);
            // 
            // chip10dollar
            // 
            this.chip10dollar.InitialImage = ((System.Drawing.Image)(resources.GetObject("chip10dollar.InitialImage")));
            this.chip10dollar.Location = new System.Drawing.Point(829, 425);
            this.chip10dollar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chip10dollar.Name = "chip10dollar";
            this.chip10dollar.Size = new System.Drawing.Size(57, 67);
            this.chip10dollar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.chip10dollar.TabIndex = 8;
            this.chip10dollar.TabStop = false;
            this.chip10dollar.Click += new System.EventHandler(this.chip10dollar_Click);
            // 
            // chip25dollar
            // 
            this.chip25dollar.InitialImage = ((System.Drawing.Image)(resources.GetObject("chip25dollar.InitialImage")));
            this.chip25dollar.Location = new System.Drawing.Point(900, 425);
            this.chip25dollar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chip25dollar.Name = "chip25dollar";
            this.chip25dollar.Size = new System.Drawing.Size(57, 67);
            this.chip25dollar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.chip25dollar.TabIndex = 8;
            this.chip25dollar.TabStop = false;
            this.chip25dollar.Click += new System.EventHandler(this.chip25dollar_Click);
            // 
            // chip50dollar
            // 
            this.chip50dollar.InitialImage = ((System.Drawing.Image)(resources.GetObject("chip50dollar.InitialImage")));
            this.chip50dollar.Location = new System.Drawing.Point(757, 508);
            this.chip50dollar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chip50dollar.Name = "chip50dollar";
            this.chip50dollar.Size = new System.Drawing.Size(57, 67);
            this.chip50dollar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.chip50dollar.TabIndex = 8;
            this.chip50dollar.TabStop = false;
            this.chip50dollar.Click += new System.EventHandler(this.chip50dollar_Click);
            // 
            // chip100dollar
            // 
            this.chip100dollar.InitialImage = ((System.Drawing.Image)(resources.GetObject("chip100dollar.InitialImage")));
            this.chip100dollar.Location = new System.Drawing.Point(829, 508);
            this.chip100dollar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chip100dollar.Name = "chip100dollar";
            this.chip100dollar.Size = new System.Drawing.Size(57, 67);
            this.chip100dollar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.chip100dollar.TabIndex = 8;
            this.chip100dollar.TabStop = false;
            this.chip100dollar.Click += new System.EventHandler(this.chip100dollar_Click);
            // 
            // chip500dollar
            // 
            this.chip500dollar.InitialImage = ((System.Drawing.Image)(resources.GetObject("chip500dollar.InitialImage")));
            this.chip500dollar.Location = new System.Drawing.Point(900, 508);
            this.chip500dollar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chip500dollar.Name = "chip500dollar";
            this.chip500dollar.Size = new System.Drawing.Size(57, 67);
            this.chip500dollar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.chip500dollar.TabIndex = 8;
            this.chip500dollar.TabStop = false;
            this.chip500dollar.Click += new System.EventHandler(this.chip500dollar_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(667, 71);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(150, 144);
            this.richTextBox1.TabIndex = 9;
            this.richTextBox1.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1183, 892);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.chip500dollar);
            this.Controls.Add(this.chip100dollar);
            this.Controls.Add(this.chip50dollar);
            this.Controls.Add(this.chip25dollar);
            this.Controls.Add(this.chip10dollar);
            this.Controls.Add(this.chip5dollar);
            this.Controls.Add(this.passa);
            this.Controls.Add(this.draNyttKort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nyRunda);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chip5dollar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chip10dollar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chip25dollar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chip50dollar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chip100dollar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chip500dollar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button nyRunda;
        private Label label1;
        private Label label2;
        private Button draNyttKort;
        private Button passa;
        private PictureBox chip5dollar;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private PictureBox chip10dollar;
        private PictureBox chip25dollar;
        private PictureBox chip50dollar;
        private PictureBox chip100dollar;
        private PictureBox chip500dollar;
        private RichTextBox richTextBox1;
    }
}