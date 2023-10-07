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
        bool uppdateraMarker = false;
        bool kryss5 = false;
        bool kryss10 = false;
        bool kryss25 = false;
        bool kryss50 = false;
        bool kryss100 = false;
        bool kryss500 = false;
        int posX = 175;
        int posY = 345;
        Image ritaBild;
        List<Tuple<Image, Point>> imagesToDraw = new List<Tuple<Image, Point>>();
        List<Tuple<Image, Point>> markerBild = new List<Tuple<Image, Point>>();
        Marker marker = new Marker();





        private void Form1_Load(object sender, EventArgs e)
        {


            for (int i = 0; i < 6; i++)
            {
                switch (i)
                {
                    case 0:
                        ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\5$.png");
                        markerBild.Add(new Tuple<Image, Point>(ritaBild, new Point(530, 255)));
                        break;
                    case 1:
                        ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\10$.png");
                        markerBild.Add(new Tuple<Image, Point>(ritaBild, new Point(580, 255)));
                        break;
                    case 2:
                        ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\25$.png");
                        markerBild.Add(new Tuple<Image, Point>(ritaBild, new Point(630, 255)));
                        break;
                    case 3:
                        ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\50$.png");
                        markerBild.Add(new Tuple<Image, Point>(ritaBild, new Point(530, 305)));
                        break;
                    case 4:
                        ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\100$.png");
                        markerBild.Add(new Tuple<Image, Point>(ritaBild, new Point(580, 305)));
                        break;
                    case 5:
                        ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\500$.png");
                        markerBild.Add(new Tuple<Image, Point>(ritaBild, new Point(630, 305)));
                        break;
                }
            }
            uppdateraMarker = true;
            this.Invalidate();
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


        protected override void OnPaint(PaintEventArgs e)
        {
            if (uppdateraMarker)
            {
                base.OnPaint(e);
                Graphics g = e.Graphics;
 
                Bitmap canvas = new Bitmap(ClientSize.Width, ClientSize.Height);

                using (Graphics canvasGraphics = Graphics.FromImage(canvas))
                {
                    // Clear the canvas with a transparent background
                    canvasGraphics.Clear(Color.Transparent);

                    // Draw all images stored in the list
                    foreach (var imageTuple in markerBild)
                    {
                        canvasGraphics.DrawImage(imageTuple.Item1, new Rectangle(imageTuple.Item2, new Size(80, 80)));
                    }

                    if (kryss5)
                    {
                        Point imagePosition = markerBild[0].Item2;
                        Size imageSize = markerBild[0].Item1.Size;
                        // Calculate the center of the image
                        int centerX = imagePosition.X + (imageSize.Width / 2);
                        int centerY = imagePosition.Y + (imageSize.Height / 2);

                        // Calculate half of the diagonal length of the image
                        int halfDiagonal = ((int)(Math.Sqrt(Math.Pow(imageSize.Width, 2) + Math.Pow(imageSize.Height, 2)) / 2)) / 3;

                        Pen redPen = new Pen(Color.Red, 5);

                        // Draw the first diagonal line of the "X" from top-left to bottom-right
                        canvasGraphics.DrawLine(redPen, centerX - halfDiagonal, centerY - halfDiagonal, centerX + halfDiagonal, centerY + halfDiagonal);

                        // Draw the second diagonal line of the "X" from top-right to bottom-left
                        canvasGraphics.DrawLine(redPen, centerX + halfDiagonal, centerY - halfDiagonal, centerX - halfDiagonal, centerY + halfDiagonal);
                    }

                    if (kryss10)
                    {
                        Point imagePosition = markerBild[1].Item2;
                        Size imageSize = markerBild[1].Item1.Size;
                        // Calculate the center of the image
                        int centerX = imagePosition.X + (imageSize.Width / 2);
                        int centerY = imagePosition.Y + (imageSize.Height / 2);

                        // Calculate half of the diagonal length of the image
                        int halfDiagonal = ((int)(Math.Sqrt(Math.Pow(imageSize.Width, 2) + Math.Pow(imageSize.Height, 2)) / 2)) / 3;

                        Pen redPen = new Pen(Color.Red, 5);

                        // Draw the first diagonal line of the "X" from top-left to bottom-right
                        canvasGraphics.DrawLine(redPen, centerX - halfDiagonal, centerY - halfDiagonal, centerX + halfDiagonal, centerY + halfDiagonal);

                        // Draw the second diagonal line of the "X" from top-right to bottom-left
                        canvasGraphics.DrawLine(redPen, centerX + halfDiagonal, centerY - halfDiagonal, centerX - halfDiagonal, centerY + halfDiagonal);
                    }

                    if (kryss25)
                    {
                        Point imagePosition = markerBild[2].Item2;
                        Size imageSize = markerBild[2].Item1.Size;
                        // Calculate the center of the image
                        int centerX = imagePosition.X + (imageSize.Width / 2);
                        int centerY = imagePosition.Y + (imageSize.Height / 2);

                        // Calculate half of the diagonal length of the image
                        int halfDiagonal = ((int)(Math.Sqrt(Math.Pow(imageSize.Width, 2) + Math.Pow(imageSize.Height, 2)) / 2)) / 3;

                        Pen redPen = new Pen(Color.Red, 5);

                        // Draw the first diagonal line of the "X" from top-left to bottom-right
                        canvasGraphics.DrawLine(redPen, centerX - halfDiagonal, centerY - halfDiagonal, centerX + halfDiagonal, centerY + halfDiagonal);

                        // Draw the second diagonal line of the "X" from top-right to bottom-left
                        canvasGraphics.DrawLine(redPen, centerX + halfDiagonal, centerY - halfDiagonal, centerX - halfDiagonal, centerY + halfDiagonal);
                    }

                    if (kryss50)
                    {
                        Point imagePosition = markerBild[3].Item2;
                        Size imageSize = markerBild[3].Item1.Size;
                        // Calculate the center of the image
                        int centerX = imagePosition.X + (imageSize.Width / 2);
                        int centerY = imagePosition.Y + (imageSize.Height / 2);

                        // Calculate half of the diagonal length of the image
                        int halfDiagonal = ((int)(Math.Sqrt(Math.Pow(imageSize.Width, 2) + Math.Pow(imageSize.Height, 2)) / 2)) / 3;

                        Pen redPen = new Pen(Color.Red, 5);

                        // Draw the first diagonal line of the "X" from top-left to bottom-right
                        canvasGraphics.DrawLine(redPen, centerX - halfDiagonal, centerY - halfDiagonal, centerX + halfDiagonal, centerY + halfDiagonal);

                        // Draw the second diagonal line of the "X" from top-right to bottom-left
                        canvasGraphics.DrawLine(redPen, centerX + halfDiagonal, centerY - halfDiagonal, centerX - halfDiagonal, centerY + halfDiagonal);
                    }

                    if (kryss100)
                    {
                        Point imagePosition = markerBild[4].Item2;
                        Size imageSize = markerBild[4].Item1.Size;
                        // Calculate the center of the image
                        int centerX = imagePosition.X + (imageSize.Width / 2);
                        int centerY = imagePosition.Y + (imageSize.Height / 2);

                        // Calculate half of the diagonal length of the image
                        int halfDiagonal = ((int)(Math.Sqrt(Math.Pow(imageSize.Width, 2) + Math.Pow(imageSize.Height, 2)) / 2)) / 3;

                        Pen redPen = new Pen(Color.Red, 5);

                        // Draw the first diagonal line of the "X" from top-left to bottom-right
                        canvasGraphics.DrawLine(redPen, centerX - halfDiagonal, centerY - halfDiagonal, centerX + halfDiagonal, centerY + halfDiagonal);

                        // Draw the second diagonal line of the "X" from top-right to bottom-left
                        canvasGraphics.DrawLine(redPen, centerX + halfDiagonal, centerY - halfDiagonal, centerX - halfDiagonal, centerY + halfDiagonal);
                    }

                    if (kryss500)
                    {
                        Point imagePosition = markerBild[5].Item2;
                        Size imageSize = markerBild[5].Item1.Size;
                        // Calculate the center of the image
                        int centerX = imagePosition.X + (imageSize.Width / 2);
                        int centerY = imagePosition.Y + (imageSize.Height / 2);

                        // Calculate half of the diagonal length of the image
                        int halfDiagonal = ((int)(Math.Sqrt(Math.Pow(imageSize.Width, 2) + Math.Pow(imageSize.Height, 2)) / 2)) / 3;

                        Pen redPen = new Pen(Color.Red, 5);

                        // Draw the first diagonal line of the "X" from top-left to bottom-right
                        canvasGraphics.DrawLine(redPen, centerX - halfDiagonal, centerY - halfDiagonal, centerX + halfDiagonal, centerY + halfDiagonal);

                        // Draw the second diagonal line of the "X" from top-right to bottom-left
                        canvasGraphics.DrawLine(redPen, centerX + halfDiagonal, centerY - halfDiagonal, centerX - halfDiagonal, centerY + halfDiagonal);
                    }
                }
                // Draw the canvas on the form
                g.DrawImageUnscaled(canvas, Point.Empty);
                uppdateraMarker = false;
            }
            if (uppdateraForm)
            {
                int y = 0;
                posX = 175;
                posY = 345;
                imagesToDraw.Clear();
                foreach (int x in markerv�rde)
                {
                    if (y == 5)
                    {
                        posY = 375;
                        posX = 175;
                    }
                    switch(x)
                    {
                        case 5:
                            ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\5$.png");
                            imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
                            posX += 10;
                            break;
                        case 10:
                            ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\10$.png");
                            imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
                            posX += 10;
                            break;
                        case 25:
                            ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\25$.png");
                            imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
                            posX += 10;
                            break;
                        case 50:
                            ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\50$.png");
                            imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
                            posX += 10;
                            break;
                        case 100:
                            ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\100$.png");
                            imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
                            posX += 10;
                            break;
                        case 500:
                            ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\500$.png");
                            imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
                            posX += 10;
                            break;
                    }
                    y++;
                }
                Graphics g = e.Graphics;

                base.OnPaint(e);
                
                Bitmap canvas = new Bitmap(ClientSize.Width, ClientSize.Height);

                using (Graphics canvasGraphics = Graphics.FromImage(canvas))
                {
                    // Clear the canvas with a transparent background
                    canvasGraphics.Clear(Color.Transparent);

                    // Draw all images stored in the list
                    foreach (var imageTuple in imagesToDraw)
                    {
                        canvasGraphics.DrawImage(imageTuple.Item1, new Rectangle(imageTuple.Item2, new Size(50, 50)));
                    }
                }


                // Draw the canvas on the form
                g.DrawImageUnscaled(canvas, Point.Empty);
                posX += 10;
                uppdateraForm = false;
            }
        }

        private void maxBet()
        {
            string test = "";
            int testint = 0;
            for (int i = 0; i < markerv�rde.Count; i++)
            {
                testint += markerv�rde[i];
            }
            richTextBox1.Text = testint.ToString();
            richTextBox1.Text += Environment.NewLine;

            if (markerv�rde.Sum() >= 500)
            {
                kryss500 = true;
            }
            if (markerv�rde.Sum() >= 900)
            {
                kryss100 = true;

            }
            if (markerv�rde.Sum() >= 950)
            {
                kryss50 = true;
            }
            if (markerv�rde.Sum() >= 975)
            {
                kryss25 = true;
            }
            if (markerv�rde.Sum() >= 990)
            {
                kryss10 = true;
            }
            if (markerv�rde.Sum() >= 995)
            {
                kryss5 = true;

            }
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

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = 0;
            foreach (var imageTuple in markerBild)
            {
                Rectangle imageBounds = new Rectangle(imageTuple.Item2, new Size(80, 80));
                if (imageBounds.Contains(e.Location))
                {
                    switch (x)
                    {
                        case 0:
                            markerv�rde.Add(5);
                            markerv�rde = marker.sorteraMarker(markerv�rde);
                            ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\5$.png");
                            imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
                            uppdateraForm = true;
                            uppdateraMarker = true;
                            maxBet();
                            this.Invalidate();
                            break;
                        case 1:
                            markerv�rde.Add(10);
                            markerv�rde = marker.sorteraMarker(markerv�rde);
                            maxBet();
                            ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\10$.png");
                            imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
                            uppdateraForm = true;
                            uppdateraMarker = true;
                            this.Invalidate();
                            break;
                        case 2:
                            markerv�rde.Add(25);
                            markerv�rde = marker.sorteraMarker(markerv�rde);
                            maxBet();
                            ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\25$.png");
                            imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
                            uppdateraForm = true;
                            uppdateraMarker = true;
                            this.Invalidate();
                            break;
                        case 3:
                            markerv�rde.Add(50);
                            markerv�rde = marker.sorteraMarker(markerv�rde);
                            maxBet();
                            ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\50$.png");
                            imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
                            uppdateraForm = true;
                            uppdateraMarker = true;
                            this.Invalidate();
                            break;
                        case 4:
                            markerv�rde.Add(100);
                            markerv�rde = marker.sorteraMarker(markerv�rde);
                            maxBet();
                            ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\100$.png");
                            imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
                            uppdateraForm = true;
                            uppdateraMarker = true;
                            this.Invalidate();
                            break;
                        case 5:
                            if (e.Button == MouseButtons.Right)
                            {
                                //kryss500 = false;
                                uppdateraForm = true;
                                uppdateraMarker = true;
                                markerv�rde = marker.raderaMarker(markerv�rde, 500);
                                this.Invalidate();
                                break;
                            }
                            markerv�rde.Add(500);
                            markerv�rde = marker.sorteraMarker(markerv�rde);
                            ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\500$.png");
                            imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
                            uppdateraForm = true;
                            uppdateraMarker = true;
                            maxBet();
                            this.Invalidate();
                            break;
                    }
                }
                x++;
            }
        }
    }
}