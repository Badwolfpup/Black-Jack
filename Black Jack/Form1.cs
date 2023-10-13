namespace Black_Jack
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Kortlek kortlek = new Kortlek();
        RitaKort ritaSpelkort = new RitaKort();
        List<string> nyKortlek = new List<string>();
        List<PictureBox> spelkortBank = new List<PictureBox>();
        List<PictureBox> spelkortSpelare = new List<PictureBox>();
        List<int> markerv�rde = new List<int>();
        int kortv�rdeBank, kortv�rdeSpelare;
        int posX = 175;
        int posY = 345;
        int bankPosX;
        int bankPosY;
        int spelarPosX;
        int spelarPosY;
        bool uppdateraForm = false;
        bool uppdateraMarker = false;
        bool uppdateraDragnakort = false;
        bool kryss5 = false;
        bool kryss10 = false;
        bool kryss25 = false;
        bool kryss50 = false;
        bool kryss100 = false;
        bool kryss500 = false;
        bool p�g�endeRunda = false;
        Image ritaBild;
        List<Tuple<Image, Point>> imagesToDraw = new List<Tuple<Image, Point>>();
        List<Tuple<Image, Point>> markerBild = new List<Tuple<Image, Point>>();
        List<Tuple<Image, Point>> dragnaKort = new List<Tuple<Image, Point>>();
        private Panel markerPanel;
        Marker marker = new Marker();



        private void uppdaterFormen()
        {
            uppdateraDragnakort = true;
            uppdateraForm = true;
            uppdateraMarker = true;
        }

 

        private void resetKort()
        {
            dragnaKort.Clear();
            markerv�rde.Clear();
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

        private void ritaMarker(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            base.OnPaint(e);

            Bitmap canvas = new Bitmap(ClientSize.Width, ClientSize.Height);
            using (Graphics canvasGraphics = Graphics.FromImage(canvas))
            {
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
        }
    



            protected override void OnPaint(PaintEventArgs e)
            {
                Graphics g = e.Graphics;

                base.OnPaint(e);

                Bitmap canvas = new Bitmap(ClientSize.Width, ClientSize.Height);
                using (Graphics canvasGraphics = Graphics.FromImage(canvas))
                {
                    canvasGraphics.Clear(Color.Transparent);

                    if (uppdateraDragnakort)
                    {
                        // Draw all images stored in the list
                        foreach (var imageTuple in dragnaKort)
                        {

                            canvasGraphics.DrawImage(imageTuple.Item1, new Rectangle(imageTuple.Item2, new Size(70, 101)));
                        }
                        uppdateraDragnakort = false;

                    }


                    // Draw the canvas on the form
                    //g.DrawImageUnscaled(canvas, Point.Empty);
            


                    if (uppdateraMarker)
                    {
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
                        uppdateraMarker = false;

                    }

                    if (uppdateraForm)
                    {
                        int y = 0;
                        posX = 646;
                        posY = 500;
                        imagesToDraw.Clear();
                        foreach (int x in markerv�rde)
                        {
                            if (y == 5)
                            {
                                posX = 646;
                                posY = 550;                           
                            }
                            switch (x)
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

                        // Draw all images stored in the list
                        foreach (var imageTuple in imagesToDraw)
                        {
                            canvasGraphics.DrawImage(imageTuple.Item1, new Rectangle(imageTuple.Item2, new Size(80, 80)));
                        }
                        uppdateraForm = false;




               
                    }
                    // Draw the canvas on the form
                    g.DrawImageUnscaled(canvas, Point.Empty);
                }
            }

        private void maxBet()
        {
            int testint = 0;
            for (int i = 0; i < markerv�rde.Count; i++)
            {
                testint += markerv�rde[i];
            }
            richTextBox1.Text = testint.ToString();
            //richTextBox1.Text += Environment.NewLine;

            if (markerv�rde.Sum() > 500)
            {
                kryss500 = true;
            }
            if (markerv�rde.Sum() > 900)
            {
                kryss100 = true;

            }
            if (markerv�rde.Sum() > 950)
            {
                kryss50 = true;
            }
            if (markerv�rde.Sum() > 975)
            {
                kryss25 = true;
            }
            if (markerv�rde.Sum() > 990)
            {
                kryss10 = true;
            }
            if (markerv�rde.Sum() > 995)
            {
                kryss5 = true;

            }
        }

 

        private void nyRunda_Click(object sender, EventArgs e)
        {
            resetKort();
            nyKortlek = kortlek.Nykortlek();
            string genv�g;
            p�g�endeRunda = true;
            bankPosX = this.Width/2 -50;
            bankPosY = this.Height/2 -200;
            spelarPosX = this.Width / 2 - 50; ;
            spelarPosY = this.Height / 2 ;
            Random rnd = new Random();
            int draKort;
            for (int i = 0; i < 3; i++)
            {
                draKort = rnd.Next(nyKortlek.Count);

                if (i < 1)
                {
                    kortv�rdeBank += kortlek.h�maKortv�rde(nyKortlek[draKort]);
                    genv�g = @"C:\\Black Jack\bilder\Spelkort\" + nyKortlek[draKort] + ".png";
                    ritaBild = Image.FromFile(genv�g);
                    dragnaKort.Add(new Tuple<Image, Point>(ritaBild, new Point(bankPosX, bankPosY)));
                    bankPosX += 15;
                    nyKortlek.RemoveAt(draKort);
                }
                else
                {
                    kortv�rdeSpelare += kortlek.h�maKortv�rde(nyKortlek[draKort]);
                    genv�g = @"C:\\Black Jack\\bilder\\Spelkort\\" + nyKortlek[draKort] + ".png";
                    ritaBild = Image.FromFile(genv�g);
                    dragnaKort.Add(new Tuple<Image, Point>(ritaBild, new Point(spelarPosX, spelarPosY)));
                    spelarPosX += 15;
                    nyKortlek.RemoveAt(draKort);
                }
            }
 
            nyRunda.Enabled = false;
            passa.Enabled = true;

            uppdaterFormen();
            this.Invalidate();
        }

        private void passa_Click_1(object sender, EventArgs e)
        {
            while (kortv�rdeBank < 17)
            {
                Random rnd = new Random();
                int draKort;
                string genv�g;
                draKort = rnd.Next(nyKortlek.Count);
                kortv�rdeBank += kortlek.h�maKortv�rde(nyKortlek[draKort]);
                genv�g = @"C:\\Black Jack\\bilder\\Spelkort\\" + nyKortlek[draKort] + ".png";
                ritaBild = Image.FromFile(genv�g);
                dragnaKort.Add(new Tuple<Image, Point>(ritaBild, new Point(bankPosX, bankPosY)));
                bankPosX += 15;
                nyKortlek.RemoveAt(draKort);
            }

  
            uppdaterFormen();
            this.Invalidate();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            bankPosX = this.Width / 2 - 50;
            bankPosY = this.Height / 2 - 200;
            spelarPosX = this.Width / 2 - 50; ;
            spelarPosY = this.Height / 2;
            markerPanel = new Panel();
            markerPanel.Size = new Size(600, 200);
            markerPanel.Location = new Point(476, 625);
            markerPanel.BackColor = Color.Transparent;
            markerPanel.Paint += new PaintEventHandler(ritaMarker);

            this.Controls.Add(markerPanel);
            this.Size = new System.Drawing.Size(1440, 800);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            Image bakgrundbild = Image.FromFile(@"C:\\Black Jack\bilder\blackjackbord.png");
            Bitmap nystorlek = new Bitmap(bakgrundbild, 1440, 647);
            this.BackgroundImage = nystorlek;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            for (int i = 0; i < 6; i++)
            {
                switch (i)
                {
                    case 0:
                        ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\5$.png");
                        markerBild.Add(new Tuple<Image, Point>(ritaBild, new Point(476, 625)));
                        break;
                    case 1:
                        ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\10$.png");
                        markerBild.Add(new Tuple<Image, Point>(ritaBild, new Point(546, 635)));
                        break;
                    case 2:
                        ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\25$.png");
                        markerBild.Add(new Tuple<Image, Point>(ritaBild, new Point(616, 645)));
                        break;
                    case 3:
                        ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\50$.png");
                        markerBild.Add(new Tuple<Image, Point>(ritaBild, new Point(706, 645)));
                        break;
                    case 4:
                        ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\100$.png");
                        markerBild.Add(new Tuple<Image, Point>(ritaBild, new Point(776, 635)));
                        break;
                    case 5:
                        ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\500$.png");
                        markerBild.Add(new Tuple<Image, Point>(ritaBild, new Point(846, 625)));
                        break;
                }
            }
            uppdateraMarker = true;
            markerPanel.Invalidate();
        }

        private void draNyttKort_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int draKort;
            string genv�g;
            draKort = rnd.Next(nyKortlek.Count);
            kortv�rdeSpelare += kortlek.h�maKortv�rde(nyKortlek[draKort]);
            genv�g = @"C:\\Black Jack\\bilder\\Spelkort\\" + nyKortlek[draKort] + ".png";
            genv�g = @"C:\\Black Jack\\bilder\\Spelkort\\" + nyKortlek[draKort] + ".png";
            ritaBild = Image.FromFile(genv�g);
            dragnaKort.Add(new Tuple<Image, Point>(ritaBild, new Point(spelarPosX, spelarPosY)));
            nyKortlek.RemoveAt(draKort);
            spelarPosX += 15;

            uppdaterFormen();
            this.Invalidate();
        }



        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = 0;
            if (!p�g�endeRunda)
            {


                foreach (var imageTuple in markerBild)
                {
                    Rectangle imageBounds = new Rectangle(imageTuple.Item2, new Size(80, 80));
                    if (imageBounds.Contains(e.Location))
                    {
                        switch (x)
                        {
                            case 0:
                                if (e.Button == MouseButtons.Right)
                                {
                                    markerv�rde = marker.raderaMarker(markerv�rde, 5);
                                    kollaOmKanBetta();
                                    uppdaterFormen();
                                    this.Invalidate();
                                    break;
                                }
                                markerv�rde.Add(5);
                                markerv�rde = marker.sorteraMarker(markerv�rde);
                                ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\5$.png");
                                imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
                                uppdaterFormen();
                                maxBet();
                                this.Invalidate();
                                break;
                            case 1:
                                if (e.Button == MouseButtons.Right)
                                {
                                    markerv�rde = marker.raderaMarker(markerv�rde, 10);
                                    kollaOmKanBetta();
                                    uppdaterFormen();
                                    this.Invalidate();
                                    break;
                                }
                                markerv�rde.Add(10);
                                markerv�rde = marker.sorteraMarker(markerv�rde);
                                maxBet();
                                ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\10$.png");
                                imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
                                uppdaterFormen();
                                this.Invalidate();
                                break;
                            case 2:
                                if (e.Button == MouseButtons.Right)
                                {
                                    markerv�rde = marker.raderaMarker(markerv�rde, 25);
                                    kollaOmKanBetta();
                                    uppdaterFormen();
                                    this.Invalidate();
                                    break;
                                }
                                markerv�rde.Add(25);
                                markerv�rde = marker.sorteraMarker(markerv�rde);
                                maxBet();
                                ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\25$.png");
                                imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
                                uppdaterFormen();
                                this.Invalidate();
                                break;
                            case 3:
                                if (e.Button == MouseButtons.Right)
                                {
                                    markerv�rde = marker.raderaMarker(markerv�rde, 50);
                                    kollaOmKanBetta();
                                    uppdaterFormen();
                                    this.Invalidate();
                                    break;
                                }
                                markerv�rde.Add(50);
                                markerv�rde = marker.sorteraMarker(markerv�rde);
                                maxBet();
                                ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\50$.png");
                                imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
                                uppdaterFormen();
                                this.Invalidate();
                                break;
                            case 4:
                                if (e.Button == MouseButtons.Right)
                                {
                                    markerv�rde = marker.raderaMarker(markerv�rde, 100);
                                    kollaOmKanBetta();
                                    uppdaterFormen();
                                    this.Invalidate();
                                    break;
                                }
                                markerv�rde.Add(100);
                                markerv�rde = marker.sorteraMarker(markerv�rde);
                                maxBet();
                                ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\100$.png");
                                imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
                                uppdaterFormen();
                                this.Invalidate();
                                break;
                            case 5:
                                if (e.Button == MouseButtons.Right)
                                {
                                    markerv�rde = marker.raderaMarker(markerv�rde, 500);
                                    kollaOmKanBetta();
                                    uppdaterFormen();
                                    this.Invalidate();
                                    break;
                                }
                                markerv�rde.Add(500);
                                markerv�rde = marker.sorteraMarker(markerv�rde);
                                ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\500$.png");
                                imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
                                uppdaterFormen();
                                maxBet();
                                this.Invalidate();
                                break;
                        }
                    }
                    x++;
                }
            }
        }

        private void kollaVinnare()
        {
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
        private void kollaOmKanBetta()
        {
            if (markerv�rde.Sum() < 1000) kryss5 = false;
            if (markerv�rde.Sum() < 990) kryss10 = false;
            if (markerv�rde.Sum() < 975) kryss25 = false;
            if (markerv�rde.Sum() < 950) kryss50 = false;
            if (markerv�rde.Sum() < 900) kryss100 = false;
            if (markerv�rde.Sum() < 500) kryss500 = false;
        }
    }
}