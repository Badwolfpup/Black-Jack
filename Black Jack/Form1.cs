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
        List<int> markervärde = new List<int>();
        int kortvärdeBank, kortvärdeSpelare;
        int antalMarker = 0;
        int antalKortBank = 0;
        int antalKortSpelare = 0;
        string bankVärde = "Bank";
        string spelarvärde = "Spelare";
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

        private void läggTillKortBank(string genväg, int antalKort)
        {
            PictureBox nyttKort = new PictureBox();
            nyttKort.Size = new Size(50, 72);
            nyttKort.Image = Image.FromFile(genväg);
            spelkortBank.Add(nyttKort);
            antalKortBank++;
        }
        private void läggTillKortSpelare(string genväg, int antalKort)
        {
            PictureBox nyttKort = new PictureBox();
            nyttKort.Size = new Size(50, 72);
            nyttKort.Image = Image.FromFile(genväg);
            spelkortSpelare.Add(nyttKort);
            antalKortSpelare++;
        }

        private void visaKort(int startposX, int startposY, int antalKort, bool bankspelare)
        {
            //Logik för att räkna ut positionering och storlek
            if (bankspelare)
            {
                for (int i = spelkortBank.Count - 1; i > -1; i--) 
                {
                    //Flyttar bilden med 15 punkter
                    spelkortBank[i].Location = new Point(startposX + antalKort * 15, startposY);
                    this.Controls.Add(spelkortBank[i]); //Lägger till bilden till Form
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
            kortvärdeBank = 0;
            kortvärdeSpelare = 0;
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
            string genväg;
            //int antalkort = 1;
            Random rnd = new Random();
            int draKort;
            for (int i = 0; i < 4; i++)
            {
                draKort = rnd.Next(nyKortlek.Count);

                if (i < 2)
                {
                    kortvärdeBank += kortlek.hämaKortvärde(nyKortlek[draKort]);
                    genväg = @"C:\\Black Jack\bilder\Spelkort\" + nyKortlek[draKort] + ".png";
                    läggTillKortBank(genväg, antalKortBank);
                    nyKortlek.RemoveAt(draKort);
                }
                else
                {
                    kortvärdeSpelare += kortlek.hämaKortvärde(nyKortlek[draKort]);
                    genväg = @"C:\\Black Jack\\bilder\\Spelkort\\" + nyKortlek[draKort] + ".png";
                    läggTillKortSpelare(genväg, antalKortSpelare);
                    nyKortlek.RemoveAt(draKort);
                }
            }
            visaKort(175, 100, antalKortBank, true);
            visaKort(175, 275, antalKortSpelare, false);
            nyRunda.Enabled = false;
            passa.Enabled = true;
            label1.Text = bankVärde + " " + kortvärdeBank.ToString();
            label2.Text = spelarvärde + " " + kortvärdeSpelare.ToString();
        }

        private void passa_Click(object sender, EventArgs e)
        {
            while (kortvärdeBank < 17)
            {
                Random rnd = new Random();
                int draKort;
                string genväg;
                draKort = rnd.Next(nyKortlek.Count);
                kortvärdeBank += kortlek.hämaKortvärde(nyKortlek[draKort]);
                genväg = @"C:\\Black Jack\\bilder\\Spelkort\\" + nyKortlek[draKort] + ".png";
                läggTillKortBank(genväg, antalKortBank);
                nyKortlek.RemoveAt(draKort);
                visaKort(175, 100, antalKortBank, true);
            }
            label1.Text = bankVärde + " " + kortvärdeBank.ToString();
            if (kortvärdeBank > 21)
            {
                nyRunda.Enabled = true;
                draNyttKort.Enabled = false;
                passa.Enabled = false;
            }
            else if (kortvärdeBank < kortvärdeSpelare)
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

        private void läggTillMarker()
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
            for (int i = 0; i<markervärde.Count; i++)
            {
                testint += markervärde[i];
            }
            richTextBox1.Text = testint.ToString();
            richTextBox1.Text += Environment.NewLine;

            if (markervärde.Sum() >= 500)
            {
                uppDatera500 = true;
                chip500dollar.Refresh();
                //this.Invalidate();
            } if (markervärde.Sum() >= 900)
            {
            } if (markervärde.Sum() >= 950)
            {
            } if (markervärde.Sum() >= 975)
            {
            } if (markervärde.Sum() >= 990)
            {
            } if (markervärde.Sum() >= 995)
            {
            }
        }

        private void chip5dollar_Click(object sender, EventArgs e)
        {
            markervärde.Add(5);
            markervärde = marker.sorteraMarker(markervärde);
            maxBet();
            uppdateraForm = true;
            this.Invalidate();

        }

        private void chip10dollar_Click(object sender, EventArgs e)
        {
            markervärde.Add(10);
            markervärde = marker.sorteraMarker(markervärde);
            maxBet();
            uppdateraForm = true;
            this.Invalidate();
        }

        private void chip25dollar_Click(object sender, EventArgs e)
        {
            markervärde.Add(25);
            markervärde = marker.sorteraMarker(markervärde);
            maxBet();
            uppdateraForm = true;
            this.Invalidate();
        }

        private void chip50dollar_Click(object sender, EventArgs e)
        {
            markervärde.Add(50);
            markervärde = marker.sorteraMarker(markervärde);
            maxBet();
            uppdateraForm = true;
            this.Invalidate();
        }

        private void chip100dollar_Click(object sender, EventArgs e)
        {
            markervärde.Add(100);
            markervärde = marker.sorteraMarker(markervärde);
            maxBet();
            uppdateraForm = true;
            this.Invalidate();
        }

        private void chip500dollar_Click(object sender, EventArgs e)
        {
            markervärde.Add(500);
            markervärde = marker.sorteraMarker(markervärde);
            maxBet();
            uppdateraForm = true;
            this.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int draKort;
            string genväg;
            draKort = rnd.Next(nyKortlek.Count);
            kortvärdeSpelare += kortlek.hämaKortvärde(nyKortlek[draKort]);
            genväg = @"C:\\Black Jack\\bilder\\Spelkort\\" + nyKortlek[draKort] + ".png";
            läggTillKortSpelare(genväg, antalKortSpelare);
            nyKortlek.RemoveAt(draKort);
            visaKort(175, 275, antalKortSpelare, false);
            label2.Text = spelarvärde + " " + kortvärdeSpelare.ToString();
            if (kortvärdeSpelare <= kortvärdeBank && (kortvärdeBank > 16 && kortvärdeBank < 22)) 
            {
                nyRunda.Enabled = false;
                passa.Enabled = false;
            }
            if (kortvärdeSpelare > 21)
            {
                nyRunda.Enabled = true;
                draNyttKort.Enabled = false;
                passa.Enabled = false;
            }
        }
    }
}