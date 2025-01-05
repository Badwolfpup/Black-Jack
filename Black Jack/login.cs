using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Black_Jack
{
    public partial class login : Form
    {

        public login()
        {
            InitializeComponent();


        }
        string filgenväg = AppDomain.CurrentDomain.BaseDirectory + "spelarinfo.txt";
        bool stängtsjälv = true;

        private void login_Load(object sender, EventArgs e)
        {
            if (!File.Exists(filgenväg))
            {
                File.Create(filgenväg).Close();
            }
            radioButton1.Checked = true;
            spelarinfo.antalspelare = 1;
            laddaSpelare();
        }

        private void laddaSpelare()
        {
            listBox1.Items.Clear();
            FileInfo filinfo = new FileInfo(filgenväg);
            if (filinfo.Length == 0)
            {

            }
            else
            {
                using (StreamReader läsfil = new StreamReader(filgenväg))
                {

                    string spelare;
                    while ((spelare = läsfil.ReadLine()) != null)
                    {
                        listBox1.Items.Add(spelare);
                    }
                }
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
        }

        private void läggtill_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if ((textBox1.Text.Length > 8))
                {
                    textBox1.Text = textBox1.Text.Remove(8);
                }
                using (StreamWriter skrivTillFil = new StreamWriter(filgenväg, true))
                {
                    skrivTillFil.WriteLine(textBox1.Text + ", 5000");
                }
                listBox1.Items.Add(textBox1.Text + ", 5000");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                textBox1.Text = "";
            }
            else
            {
                MessageBox.Show("Du måste skriv ett namn", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void starta_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                spelarinfo.spelare = listBox1.Text;
                stängtsjälv = false;
                this.Close();
                spelarinfo.spelarNamnMarker();
            }
            else
            {
                MessageBox.Show("Du måste välja en spelare", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tabort_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                List<string> spelarlista = new List<string>();
                using (StreamReader läsfil = new StreamReader(filgenväg))
                {

                    string spelare;
                    while ((spelare = läsfil.ReadLine()) != null)
                    {
                        spelarlista.Add(spelare);
                    }
                }
                spelarlista.Remove(listBox1.SelectedItem.ToString());

                using (StreamWriter skrivFil = new StreamWriter(filgenväg))
                {
                    foreach (string spelare in spelarlista)
                    {
                        skrivFil.WriteLine(spelare);
                    }
                }
                laddaSpelare();
            }
            else
            {
                MessageBox.Show("Du måste välja en spelare", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (stängtsjälv)
            {
                Environment.Exit(0);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                spelarinfo.antalspelare = 1;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                spelarinfo.antalspelare = 2;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                spelarinfo.antalspelare = 3;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                spelarinfo.antalspelare = 4;
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
            {
                spelarinfo.antalspelare = 5;
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
            {
                spelarinfo.antalspelare = 6;
            }
        }


    }
}
