using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Black_Jack
{
    public class Spelare
    {
        public List<Spelhög> spelhög { get; set; }
        public Label spelarinfolabel { get; set; }
        public int datorbalans { get; set; }

        public string spelarinfotext;
        public int skicklighet { get; set; } //Sannolikheten att datorn tar rätt beslut

        public Spelare(int i)
        {
            spelhög = new List<Spelhög>();
            spelarinfolabel = new Label();
            if (i != spelarinfo.spelarNummer && i != 0) datorbalans = 5000;
            Random r = new Random();
            skicklighet = r.Next(70, 96);
        }

        public void läggtillSpelarinfo(int i, int datornummer, int posX, int posY) //Lägger till labeln som visar namn och kontobalans
        {
            spelarinfolabel.Location = new Point(posX, posY);
            spelarinfolabel.Font = new Font("MS Gothic", 10, FontStyle.Regular);
            spelarinfolabel.BackColor = Color.LightGray;
            spelarinfolabel.AutoSize = true;
            spelarinfolabel.BringToFront();

            if (i == spelarinfo.spelarNummer) //Om spelaren
            {
                spelarinfotext = spelarinfo.spelarnamn + ": $";
                spelarinfolabel.Text = spelarinfotext + spelarinfo.kontobalans;
            }
            else //Datorspelarna
            {
                spelarinfotext = "Dator" + datornummer + ": $";
                spelarinfolabel.Text = spelarinfotext + datorbalans;
            }
        }

        public void uppdateraSpelarinfo(int i) //Uppdaterar labeln med namn och kontobalans, efter ett gjort bet.
        {
            if (i == spelarinfo.spelarNummer) spelarinfolabel.Text = spelarinfotext + spelarinfo.kontobalans.ToString(); 
            else spelarinfolabel.Text = spelarinfotext + datorbalans.ToString();
        }

        public void läggtillSpelhög() //Lägger till en ny spelhög i listan
        {
            Spelhög hög = new Spelhög();
            spelhög.Add(hög);
        }

        public void insertSpelhög() //Insertar en ny spelhög först i list. Används vid split
        {
            Spelhög hög = new Spelhög();
            spelhög.Insert(0, hög);
        }

        public void läggtillKortochVärde(string kort, int i, int posX, int posY) //Callar metoder i spelhögklassen
        {
            spelhög[i].läggtillImage(kort, posX, posY);
            spelhög[i].läggtillKortvärde(kort);
        }

        public void läggtillBetinfo(int i, int posX, int posY) //Callar metoder i spelhögklassen
        {
            spelhög[i].läggtillBetinfo(posX, posY);
        }

        public void läggtillKortSumma(int i, int posX, int posY) //Callar metoder i spelhögklassen
        {
            spelhög[i].läggtillKortsumma(posX, posY);
        }



        public class Spelhög //Har information om varje spelhög
        {
            public List<PictureBox> spelkort { get; set; } //Lista med picturebox som innehåller bilderna på korten
            public List<int> kortvärde { get; set; } //Lista med det numeriska värdet av varje kort
            public Label betinfo { get; set; } //Label som visar hur mycket som bettats
            public Label kortsumma { get; set; } //Label som visar hur mycket korten är värda tillsammans
            public int betsumma { get; set; } //Int som sparar hur mycket som bettats
            public bool splitEllerDouble  { get; set; } = false; //Håller koll på om det har gjorts split eller double (då man auromatiska drar ett kort till)

            public bool kollatVinst { get; set; } = false; //Håller koll på om spelaren har gått bust och därmed inte behöver kontrolleras på slutet


            public Spelhög()
            {
                spelkort = new List<PictureBox>();
                kortvärde = new List<int>();
                betinfo = new Label();
                kortsumma = new Label();
                splitEllerDouble = false;
                kollatVinst = false;
            }

            public void läggtillImage(string kort, int posX, int posY) //Lägger till bilden på kortet till pictureboxen, samt properties
            {
                Image image = (Image)Properties.Resources.ResourceManager.GetObject(kort);
                PictureBox kortbild = new PictureBox();
                kortbild.Size = new Size(50, 72);
                kortbild.Location = new Point(posX, posY);
                kortbild.BackColor = Color.Transparent;
                kortbild.BringToFront();
                kortbild.SizeMode = PictureBoxSizeMode.StretchImage;
                kortbild.Image = image;
                spelkort.Add(kortbild);   //test               
            }

            public void läggtillBetinfo(int posX, int posY) //Ger properties till labeln som visar hur mycket som bettats
            {
                betinfo.Location = new Point(posX, posY);
                betinfo.Font = new Font("MS Gothic", 10, FontStyle.Regular);
                betinfo.BackColor = Color.LightGray;
                betinfo.AutoSize = true;
                betinfo.BringToFront() ;
                betinfo.Text = "$0";
            }

            public void läggtillKortsumma(int posX, int posY) //Ger properties till labeln som visar hur korten är värda
            {
                kortsumma.Location = new Point(posX, posY);
                kortsumma.Font = new Font("MS Gothic", 10, FontStyle.Regular);
                kortsumma.BackColor = Color.LightGray;
                kortsumma.AutoSize = true;
                kortsumma.BringToFront();
                kortsumma.Text = "";
            }

            public void läggtillKortvärde(string kort) //Lägger till, i listan, det numeriska värdet för varje kort
            {
                kortvärde.Add(Kortlek.hämtaKortvärde(kort));
            }
        }
    }
}
