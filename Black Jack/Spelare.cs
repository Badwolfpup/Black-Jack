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
        public Label spelarinfo { get; set; }

        public Spelare()
        {
            spelhög = new List<Spelhög>();
            spelarinfo = new Label();
        }

        public void läggtillSpelarinfo(int posX, int posY)
        {
            spelarinfo.Location = new Point(posX, posY);
            spelarinfo.Font = new Font("MS Gothic", 12, FontStyle.Bold);
            spelarinfo.BackColor = Color.LightGray;
            spelarinfo.AutoSize = true;
            spelarinfo.BringToFront();
            //if (i == spelarNummer) visaspelareinfo.Text = spelarNamn + ": $" + antalMarker;
            //else visaspelareinfo.Text = "Dator" + nummer1 + ": $5000";
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

        public class Spelhög
        {
            List<PictureBox> spelkort = new List<PictureBox>();
            List<Button> knappar = new List<Button>();
            List<int> kortvärde = new List<int>();
            Label betinfo = new Label();
            Label kortsumma = new Label();            

            public Spelhög()
            {

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
            }

            public void läggtillKortsumma(int posX, int posY)
            {
                kortsumma.Location = new Point(posX, posY);
                kortsumma.Font = new Font("MS Gothic", 12, FontStyle.Bold);
                kortsumma.BackColor = Color.LightGray;
                kortsumma.AutoSize = true;
                kortsumma.BringToFront();
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
