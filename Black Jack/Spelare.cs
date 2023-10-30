using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Black_Jack
{
    public class Spelare
    {
        public List<Spelhög> spelhög { get; set; }
        public Label spelarinfolabel { get; set; }

        public Spelare()
        {
            spelhög = new List<Spelhög>();
            spelarinfolabel = new Label();
        }

        public void läggtillSpelarinfo(int i, int datornummer, int posX, int posY)
        {
            spelarinfolabel.Location = new Point(posX, posY);
            spelarinfolabel.Font = new Font("MS Gothic", 12, FontStyle.Bold);
            spelarinfolabel.BackColor = Color.LightGray;
            spelarinfolabel.AutoSize = true;
            spelarinfolabel.BringToFront();

            if (i == spelarinfo.spelarNummer) spelarinfolabel.Text = spelarinfo.spelarnamn + ": $" + spelarinfo.kontobalans;
            else spelarinfolabel.Text = "Dator" + datornummer + ": $5000";
        }

        public void läggtillSpelhög()
        {
            Spelhög hög = new Spelhög();
            spelhög.Add(hög);
        }

        public void läggtillKortochVärde(string kort, int i, int posX, int posY)
        {
            spelhög[i].läggtillImage(kort, posX, posY);
            spelhög[i].läggtillKortvärde(kort);
        }

        public void läggtillBetinfo(int i, int posX, int posY)
        {
            spelhög[i].läggtillBetinfo(posX, posY);
        }

        public void läggtillKortSumma(int i, int posX, int posY)
        {
            spelhög[i].läggtillKortsumma(posX, posY);
        }



        public class Spelhög
        {
            public List<PictureBox> spelkort { get; set; }
            public List<Button> knappar { get; set; }
            public List<int> kortvärde { get; set; }
            public Label betinfo { get; set; }
            public Label kortsumma { get; set; }

            public Spelhög()
            {
                spelkort = new List<PictureBox>();
                knappar = new List<Button>();
                kortvärde = new List<int>();
                betinfo = new Label();
                kortsumma = new Label();
            }

            public void läggtillImage(string kort, int posX, int posY)
            {
                string genväg = @"C:\\Black Jack\bilder\Spelkort\" + kort + ".png";
                Image image = Image.FromFile(genväg);
                PictureBox kortbild = new PictureBox();
                kortbild.Size = new Size(50, 72);
                kortbild.Location = new Point(posX, posY);
                kortbild.BackColor = Color.Transparent;
                kortbild.SizeMode = PictureBoxSizeMode.StretchImage;
                spelkort.Add(kortbild);  
                
            }

            public void läggtillBetinfo(int posX, int posY)
            {
                betinfo.Location = new Point(posX, posY);
                betinfo.Font = new Font("MS Gothic", 12, FontStyle.Bold);
                betinfo.BackColor = Color.LightGray;
                betinfo.AutoSize = true;
                betinfo.BringToFront();
                betinfo.Text = "!";
            }

            public void läggtillKortsumma(int posX, int posY)
            {
                kortsumma.Location = new Point(posX, posY);
                kortsumma.Font = new Font("MS Gothic", 12, FontStyle.Bold);
                kortsumma.BackColor = Color.LightGray;
                kortsumma.AutoSize = true;
                kortsumma.BringToFront();
                kortsumma.Text = "!";
            }

            public void läggtillKortvärde(string kort)
            {
                kortvärde.Add(Kortlek.hämtaKortvärde(kort));
            }

            public void läggtillKappar()
            {

            }
        }
    }
}
