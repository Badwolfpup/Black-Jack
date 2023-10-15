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
        string filgenväg = @"C:\\Black Jack\spelarinfo.txt";
        bool valtNamn = false;

        private void login_Load(object sender, EventArgs e)
        {
            if (!File.Exists(filgenväg))
            {
                File.Create(filgenväg);
            }
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

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                spelarinfo.spelare = listBox1.Text;
                this.Close();
            }
            else
            {
                MessageBox.Show("Du måste välja en spelare", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
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


    }
}
