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
            nyRunda = new Button();
            label1 = new Label();
            label2 = new Label();
            draNyttKort = new Button();
            passa = new Button();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            richTextBox1 = new RichTextBox();
            SuspendLayout();
            // 
            // nyRunda
            // 
            nyRunda.Location = new Point(613, 90);
            nyRunda.Margin = new Padding(2);
            nyRunda.Name = "nyRunda";
            nyRunda.Size = new Size(78, 20);
            nyRunda.TabIndex = 1;
            nyRunda.Text = "Börja spela";
            nyRunda.UseVisualStyleBackColor = true;
            nyRunda.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(190, 80);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(33, 15);
            label1.TabIndex = 2;
            label1.Text = "Bank";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(190, 255);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(45, 15);
            label2.TabIndex = 4;
            label2.Text = "Spelare";
            // 
            // draNyttKort
            // 
            draNyttKort.Location = new Point(613, 119);
            draNyttKort.Margin = new Padding(2);
            draNyttKort.Name = "draNyttKort";
            draNyttKort.Size = new Size(78, 20);
            draNyttKort.TabIndex = 6;
            draNyttKort.Text = "Dra kort";
            draNyttKort.UseVisualStyleBackColor = true;
            draNyttKort.Click += button2_Click;
            // 
            // passa
            // 
            passa.Location = new Point(616, 150);
            passa.Margin = new Padding(2);
            passa.Name = "passa";
            passa.Size = new Size(78, 20);
            passa.TabIndex = 7;
            passa.Text = "Passa";
            passa.UseVisualStyleBackColor = true;
            passa.Click += passa_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(467, 43);
            richTextBox1.Margin = new Padding(2);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(106, 88);
            richTextBox1.TabIndex = 9;
            richTextBox1.Text = "";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(828, 535);
            Controls.Add(richTextBox1);
            Controls.Add(passa);
            Controls.Add(draNyttKort);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(nyRunda);
            Margin = new Padding(2);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            MouseClick += Form1_MouseClick;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button nyRunda;
        private Label label1;
        private Label label2;
        private Button draNyttKort;
        private Button passa;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private RichTextBox richTextBox1;
    }
}