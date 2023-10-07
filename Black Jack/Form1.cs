namespace Black_Jack
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Kortlek kortlek = new Kortlek();
        List<string> nyKortlek = new List<string>();
        List<PictureBox> spelkortBank = new List<PictureBox>();
        List<PictureBox> spelkortSpelare = new List<PictureBox>();
        List<int> markerv�rde = new List<int>();
        int kortv�rdeBank, kortv�rdeSpelare;
        int antalMarker = 0;
        int antalKortBank = 0;
        int antalKortSpelare = 0;
        string bankV�rde = "Bank";
        string spelarv�rde = "Spelare";
        bool uppdateraForm = false;
        bool uppDatera500 = false;
        int posX = 175;
        Image ritaBild;
        List<Tuple<Image, Point>> imagesToDraw = new List<Tuple<Image, Point>>();
        Marker marker = new Marker();





        private void Form1_Load(object sender, EventArgs e)
        {
           
           chip5dollar.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\5$.png");
           chip10dollar.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\10$.png");
           chip25dollar.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\25$.png");
           chip50dollar.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\50$.png");
           chip100dollar.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\100$.png");
           chip500dollar.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\500$.png");
        }

        private void l�ggTillKortBank(string genv�g, int antalKort)
        {
            PictureBox nyttKort = new PictureBox();
            nyttKort.Size = new Size(50, 72);
            nyttKort.Image = Image.FromFile(genv�g);
            spelkortBank.Add(nyttKort);
            antalKortBank++;
        }
        private void l�ggTillKortSpelare(string genv�g, int antalKort)
        {
            PictureBox nyttKort = new PictureBox();
            nyttKort.Size = new Size(50, 72);
            nyttKort.Image = Image.FromFile(genv�g);
            spelkortSpelare.Add(nyttKort);
            antalKortSpelare++;
        }

        private void visaKort(int startposX, int startposY, int antalKort, bool bankspelare)
        {
            //Logik f�r att r�kna ut positionering och storlek
            if (bankspelare)
            {
                for (int i = spelkortBank.Count - 1; i > -1; i--) 
                {
                    //Flyttar bilden med 15 punkter
                    spelkortBank[i].Location = new Point(startposX + antalKort * 15, startposY);
                    this.Controls.Add(spelkortBank[i]); //L�gger till bilden till Form
                    antalKort--;

                }
            }
            else
            {
                for (int i = spelkortSpelare.Count - 1; i > -1; i--)
                {
                    spelkortSpelare[i].Location = new Point(startposX + antalKort * 15, startposY);
                    this.Controls.Add(spelkortSpelare[i]);
                    antalKort--;
                }
            }
        }

        private void resetKort()
        {

            antalKortBank = 0;
            antalKortSpelare = 0;
            kortv�rdeBank = 0;
            kortv�rdeSpelare = 0;
            nyKortlek.Clear();
            for (int i = 0; i < spelkortBank.Count; i++)
            {
                this.Controls.Remove(spelkortBank[i]);
            }
            for (int i = 0; i < spelkortSpelare.Count; i++)
            {
                this.Controls.Remove(spelkortSpelare[i]);
            }
            spelkortBank.Clear();
            spelkortSpelare.Clear();
            draNyttKort.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            resetKort();
            nyKortlek = kortlek.Nykortlek();
            string genv�g;
            //int antalkort = 1;
            Random rnd = new Random();
            int draKort;
            for (int i = 0; i < 4; i++)
            {
                draKort = rnd.Next(nyKortlek.Count);

                if (i < 2)
                {
                    kortv�rdeBank += kortlek.h�maKortv�rde(nyKortlek[draKort]);
                    genv�g = @"C:\\Black Jack\bilder\Spelkort\" + nyKortlek[draKort] + ".png";
                    l�ggTillKortBank(genv�g, antalKortBank);
                    nyKortlek.RemoveAt(draKort);
                }
                else
                {
                    kortv�rdeSpelare += kortlek.h�maKortv�rde(nyKortlek[draKort]);
                    genv�g = @"C:\\Black Jack\\bilder\\Spelkort\\" + nyKortlek[draKort] + ".png";
                    l�ggTillKortSpelare(genv�g, antalKortSpelare);
                    nyKortlek.RemoveAt(draKort);
                }
            }
            visaKort(175, 100, antalKortBank, true);
            visaKort(175, 275, antalKortSpelare, false);
            nyRunda.Enabled = false;
            passa.Enabled = true;
            label1.Text = bankV�rde + " " + kortv�rdeBank.ToString();
            label2.Text = spelarv�rde + " " + kortv�rdeSpelare.ToString();
        }

        private void passa_Click(object sender, EventArgs e)
        {
            while (kortv�rdeBank < 17)
            {
                Random rnd = new Random();
                int draKort;
                string genv�g;
                draKort = rnd.Next(nyKortlek.Count);
                kortv�rdeBank += kortlek.h�maKortv�rde(nyKortlek[draKort]);
                genv�g = @"C:\\Black Jack\\bilder\\Spelkort\\" + nyKortlek[draKort] + ".png";
                l�ggTillKortBank(genv�g, antalKortBank);
                nyKortlek.RemoveAt(draKort);
                visaKort(175, 100, antalKortBank, true);
            }
            label1.Text = bankV�rde + " " + kortv�rdeBank.ToString();
            if (kortv�rdeBank > 21)
            {
                nyRunda.Enabled = true;
                draNyttKort.Enabled = false;
                passa.Enabled = false;
            }
            else if (kortv�rdeBank < kortv�rdeSpelare)
            {
                nyRunda.Enabled = true;
                draNyttKort.Enabled = false;
                passa.Enabled = false;
            }
            else
            {
                nyRunda.Enabled = true;
                draNyttKort.Enabled = false;
                passa.Enabled = false;
            }
         }

        private void l�ggTillMarker()
        {


        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (uppDatera500)
            {
   
            }
            if (uppdateraForm)
            {
                ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\5$.png");
                imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, 375)));
                base.OnPaint(e);
                Graphics g = e.Graphics;
                Bitmap canvas = new Bitmap(ClientSize.Width, ClientSize.Height);

                using (Graphics canvasGraphics = Graphics.FromImage(canvas))
                {
                    // Clear the canvas with a transparent background
                    canvasGraphics.Clear(Color.Transparent);

                    // Draw all images stored in the list
                    foreach (var imageTuple in imagesToDraw)
                    {
                        canvasGraphics.DrawImage(imageTuple.Item1, new Rectangle(imageTuple.Item2, new Size(80, 80)));
                    }
                }


                // Draw the canvas on the form
                g.DrawImageUnscaled(canvas, Point.Empty);
                posX += 10;
                uppdateraForm = false;
            }
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
 
        }
        private void maxBet()
        {
            string test = "";
            int testint = 0;
            for (int i = 0; i<markerv�rde.Count; i++)
            {
                testint += markerv�rde[i];
            }
            richTextBox1.Text = testint.ToString();
            richTextBox1.Text += Environment.NewLine;

            if (markerv�rde.Sum() >= 500)
            {
                uppDatera500 = true;
                chip500dollar.Refresh();
                //this.Invalidate();
            } if (markerv�rde.Sum() >= 900)
            {
            } if (markerv�rde.Sum() >= 950)
            {
            } if (markerv�rde.Sum() >= 975)
            {
            } if (markerv�rde.Sum() >= 990)
            {
            } if (markerv�rde.Sum() >= 995)
            {
            }
        }

        private void chip5dollar_Click(object sender, EventArgs e)
        {
            markerv�rde.Add(5);
            markerv�rde = marker.sorteraMarker(markerv�rde);
            maxBet();
            uppdateraForm = true;
            this.Invalidate();

        }

        private void chip10dollar_Click(object sender, EventArgs e)
        {
            markerv�rde.Add(10);
            markerv�rde = marker.sorteraMarker(markerv�rde);
            maxBet();
            uppdateraForm = true;
            this.Invalidate();
        }

        private void chip25dollar_Click(object sender, EventArgs e)
        {
            markerv�rde.Add(25);
            markerv�rde = marker.sorteraMarker(markerv�rde);
            maxBet();
            uppdateraForm = true;
            this.Invalidate();
        }

        private void chip50dollar_Click(object sender, EventArgs e)
        {
            markerv�rde.Add(50);
            markerv�rde = marker.sorteraMarker(markerv�rde);
            maxBet();
            uppdateraForm = true;
            this.Invalidate();
        }

        private void chip100dollar_Click(object sender, EventArgs e)
        {
            markerv�rde.Add(100);
            markerv�rde = marker.sorteraMarker(markerv�rde);
            maxBet();
            uppdateraForm = true;
            this.Invalidate();
        }

        private void chip500dollar_Click(object sender, EventArgs e)
        {
            markerv�rde.Add(500);
            markerv�rde = marker.sorteraMarker(markerv�rde);
            maxBet();
            uppdateraForm = true;
            this.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int draKort;
            string genv�g;
            draKort = rnd.Next(nyKortlek.Count);
            kortv�rdeSpelare += kortlek.h�maKortv�rde(nyKortlek[draKort]);
            genv�g = @"C:\\Black Jack\\bilder\\Spelkort\\" + nyKortlek[draKort] + ".png";
            l�ggTillKortSpelare(genv�g, antalKortSpelare);
            nyKortlek.RemoveAt(draKort);
            visaKort(175, 275, antalKortSpelare, false);
            label2.Text = spelarv�rde + " " + kortv�rdeSpelare.ToString();
            if (kortv�rdeSpelare <= kortv�rdeBank && (kortv�rdeBank > 16 && kortv�rdeBank < 22)) 
            {
                nyRunda.Enabled = false;
                passa.Enabled = false;
            }
            if (kortv�rdeSpelare > 21)
            {
                nyRunda.Enabled = true;
                draNyttKort.Enabled = false;
                passa.Enabled = false;
            }
        }
    }
}