using System.Drawing.Imaging;
using System.Media;
using NAudio.Wave;
using System.Threading;
using System.Windows.Forms;

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
        List<int> markervärde = new List<int>(); //Hur många marker spelar har satsat
        List<int> bankListaKort = new List<int>();
        List<int> spelarListaKort = new List<int>();
        bool[] kollaKryss = new bool[] { false, false, false, false, false, false };
        int kortvärdeBank, kortvärdeSpelare;
        int antalMarker = 5000; //Hur många marker spelaren har
        int posX = 0;
        int posY = 0;
        int bankPosX;
        int bankPosY;
        int spelarPosX;
        int spelarPosY;
        bool pågåendeRunda = false;
        bool spelaresTur = true;
        Image ritaBild;
        List<Tuple<Image, Point>> imagesToDraw = new List<Tuple<Image, Point>>();
        List<Tuple<Image, Point>> markerBild = new List<Tuple<Image, Point>>();
        List<Tuple<Image, Point>> dragnaKort = new List<Tuple<Image, Point>>();
        List<Tuple<Image, Point>> knappar = new List<Tuple<Image, Point>>();
        private Panel markerPanel;
        private Panel spelkortPanel;
        private Panel satsatPanel;
        private Panel knapparPanel;

        Marker marker = new Marker();
        Label totalBet = new Label();
        Label betInfo = new Label();
        Label bankrulle = new Label();
        string spelarNamn = "";
        login loggain;


        private void spelaLjud(string genväg)
        {
            using (SoundPlayer ljudspelare = new SoundPlayer(genväg))
            {
                ljudspelare.PlaySync();
            }
        }

        private void spelaLjudUtanSync(string genväg)
        {
            using (SoundPlayer ljudspelare = new SoundPlayer(genväg))
            {
                ljudspelare.Play();

            }
        }

        private void uppdateraFil()
        {
            string x = "";
            string filgenväg = @"C:\\Black Jack\spelarinfo.txt";
            List<string> spelarlista = new List<string>();
            using (StreamReader läsfil = new StreamReader(filgenväg))
            {
                string spelare;
                while ((spelare = läsfil.ReadLine()) != null)
                {
                    spelarlista.Add(spelare);
                }
            }
            for (int i = 0; i < spelarlista.Count; i++)
            {
                if (spelarlista[i].Contains(spelarNamn))
                {
                    x = spelarlista[i];
                    int y = x.IndexOf(',');
                    x = x.Remove(y);
                    x += ", " + antalMarker.ToString();
                    spelarlista.Insert(i, x);
                    spelarlista.RemoveAt(i + 1);
                }
            }

            using (StreamWriter skrivFil = new StreamWriter(filgenväg))
            {
                foreach (string spelare in spelarlista)
                {
                    skrivFil.WriteLine(spelare);
                }
            }
        }

        private void spelarNamnMarker()
        {

            string namn = "";
            string marker = "";
            bool namnellermarker = true;
            string spelare = spelarinfo.spelare;
            try
            {
                foreach (char c in spelare)
                {
                    if (namnellermarker && c != ',')
                    {
                        namn += c.ToString();
                    }
                    else if (!namnellermarker && c != ',') marker += c.ToString();
                    else namnellermarker = false;
                }
                spelarNamn = namn;
                int.TryParse(marker, out antalMarker);
            }
            catch (Exception e)
            {
                if (spelarNamn == "")
                {
                    Environment.Exit(0);
                }
            }
            
            
        }

        private void bytSpelareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loggain.ShowDialog();

        }


        private void Form1_Load_1(object sender, EventArgs e)
        {
            this.Hide();
            loggain = new login();
            loggain.ShowDialog();
            //this.WindowState = FormWindowState.Maximized;
            this.Size = new System.Drawing.Size(1440, 800);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            Image bakgrundbild = Image.FromFile(@"C:\\Black Jack\bilder\blackjackbord.png");
            Bitmap nystorlek = new Bitmap(bakgrundbild, 1440, 647);
            this.BackgroundImage = nystorlek;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            spelarNamnMarker();

            totalBet.Location = new Point(this.Width / 2 - 40, this.Height / 2 + 85);
            totalBet.Font = new Font("MS Gothic", 14, FontStyle.Bold);
            totalBet.BackColor = Color.Transparent;
            totalBet.AutoSize = true;
            this.Controls.Add(totalBet);
            totalBet.BringToFront();

            betInfo.Location = new Point(this.Width / 2 - 105, this.Height / 2 + 225);
            betInfo.Font = new Font("MS Gothic", 14, FontStyle.Bold);
            betInfo.BackColor = Color.Transparent;
            betInfo.AutoSize = true;
            this.Controls.Add(betInfo);
            betInfo.BringToFront();
            betInfo.Text = "Maxbet är $1000";

            bankrulle.Location = new Point(this.Width / 2 - 420, this.Height / 2 + 100);
            bankrulle.Font = new Font("MS Gothic", 14, FontStyle.Bold);
            bankrulle.BackColor = Color.Transparent;
            bankrulle.AutoSize = true;
            this.Controls.Add(bankrulle);
            totalBet.BringToFront();
            bankrulle.Text = spelarNamn + ", du har $" + antalMarker;

            this.DoubleBuffered = true;
            bankPosX = this.Width / 2 - 50;
            bankPosY = this.Height / 2 - 200;
            spelarPosX = this.Width / 2 - 50; ;
            spelarPosY = this.Height / 2;

            markerPanel = new Panel();
            markerPanel.Size = new Size(600, 200);
            markerPanel.Location = new Point(this.Width/2-244, this.Height/2 + 225);
            markerPanel.BackColor = Color.Transparent;
            markerPanel.Paint += new PaintEventHandler(ritaMarker);
            this.Controls.Add(markerPanel);
            markerPanel.MouseClick += new MouseEventHandler(markerPanel_MouseClick);


            spelkortPanel = new Panel();
            spelkortPanel.Size = new Size(350, 300);
            spelkortPanel.Location = new Point(this.Width / 2 - 50, this.Height / 2 - 242);
            spelkortPanel.BackColor = Color.Transparent;
            spelkortPanel.Paint += new PaintEventHandler(ritaSpelkort);
            this.Controls.Add(spelkortPanel);

            satsatPanel = new Panel();
            satsatPanel.Size = new Size(350, 200);
            satsatPanel.Location = new Point(this.Width / 2 - 90, this.Height / 2 +95);
            satsatPanel.BackColor = Color.Transparent;
            satsatPanel.Paint += new PaintEventHandler(ritaSatsadeMarker);
            this.Controls.Add(satsatPanel);

            knapparPanel = new Panel();
            knapparPanel.Size = new Size(100, 300);
            knapparPanel.Location = new Point(this.Width / 2 - 120, this.Height / 2 - 50);
            knapparPanel.BackColor = Color.Transparent;
            knapparPanel.Paint += new PaintEventHandler(ritaknapparPanel);
            this.Controls.Add(knapparPanel);
            knapparPanel.MouseClick += new MouseEventHandler(knapparPanel_MouseClick);



            läggTillMarkerBilder();
            läggTillKnappBilder();

        }

        #region onPaint
        private void ritaMarker(object sender, PaintEventArgs e)
        {
            markerPanel.SuspendLayout();
            Graphics g = e.Graphics;

            base.OnPaint(e);

            Bitmap canvas = new Bitmap(markerPanel.Width, markerPanel.Height);
            using (Graphics canvasGraphics = Graphics.FromImage(canvas))
            {
                canvasGraphics.Clear(Color.Transparent);

                // Draw all images stored in the list
                foreach (var imageTuple in markerBild)
                {
                    canvasGraphics.DrawImage(imageTuple.Item1, new Rectangle(imageTuple.Item2, new Size(80, 80)));
                }
                for (int i = 0; i < kollaKryss.Length; i++)
                {
                    if (kollaKryss[i])
                    {
                        Point imagePosition = markerBild[i].Item2;
                        Size imageSize = markerBild[i].Item1.Size;
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
                g.DrawImageUnscaled(canvas, Point.Empty);
            }
            markerPanel.ResumeLayout();
        }

        private void ritaSpelkort(object sender, PaintEventArgs e)
        {
            spelkortPanel.SuspendLayout();
            Graphics g = e.Graphics;

            base.OnPaint(e);

            Bitmap canvas = new Bitmap(spelkortPanel.Width, spelkortPanel.Height);
            using (Graphics canvasGraphics = Graphics.FromImage(canvas))
            {
                canvasGraphics.Clear(Color.Transparent);

                foreach (var imageTuple in dragnaKort)
                {

                    canvasGraphics.DrawImage(imageTuple.Item1, new Rectangle(imageTuple.Item2, new Size(70, 101)));
                }

                g.DrawImageUnscaled(canvas, Point.Empty);
            }
            spelkortPanel.ResumeLayout();
        }

        private void ritaSatsadeMarker(object sender, PaintEventArgs e)
        {
            satsatPanel.SuspendLayout();
            int y = 0;
            posX = 0;
            posY = 0;
            imagesToDraw.Clear();
            foreach (int x in markervärde)
            {
                if (y == 5)
                {
                    posX = 0;
                    posY = 50;
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
                    case 1000:
                        ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\1000$.png");
                        imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
                        posX += 10;
                        break;
                }
                y++;
            }
            Graphics g = e.Graphics;

            base.OnPaint(e);

            Bitmap canvas = new Bitmap(satsatPanel.Width, satsatPanel.Height);
            using (Graphics canvasGraphics = Graphics.FromImage(canvas))
            {
                canvasGraphics.Clear(Color.Transparent);

                // Draw all images stored in the list
                foreach (var imageTuple in imagesToDraw)
                {
                    canvasGraphics.DrawImage(imageTuple.Item1, new Rectangle(imageTuple.Item2, new Size(80, 80)));
                }

                g.DrawImageUnscaled(canvas, Point.Empty);
            }
            satsatPanel.ResumeLayout();
        }

        private void ritaknapparPanel(object sender, PaintEventArgs e)
        {
            knapparPanel.SuspendLayout();
            Graphics g = e.Graphics;

            base.OnPaint(e);

            Bitmap canvas = new Bitmap(knapparPanel.Width, knapparPanel.Height);
            using (Graphics canvasGraphics = Graphics.FromImage(canvas))
            {
                canvasGraphics.Clear(Color.Transparent);

                foreach (var imageTuple in knappar)
                {
                    canvasGraphics.DrawImage(imageTuple.Item1, new Rectangle(imageTuple.Item2, new Size(50, 50)));

                }

                g.DrawImageUnscaled(canvas, Point.Empty);
            }

            knapparPanel.ResumeLayout();
        }


        #endregion


        #region spellogik
        private async Task spelaKort()
        {
            if (markervärde.Sum() > 0)
            {

                string genväg;
                Random rnd = new Random();
                int draKort;
                if (!pågåendeRunda)
                {
                    spelaLjud(@"C:\\Black Jack\Audio\lekblandas.wav");
                    resetKort();
                    bankPosX = 0;
                    bankPosY = 0;
                    spelarPosX = 0;
                    spelarPosY = 200;
                    nyKortlek = kortlek.Nykortlek();
                    for (int i = 0; i < 3; i++)
                    {
                        spelaLjud(@"C:\\Black Jack\Audio\kortspelas.wav");

                        draKort = rnd.Next(nyKortlek.Count);
                        if (i == 0)
                        {
                            bankListaKort.Add(kortlek.hämtaKortvärde(nyKortlek[draKort]));
                            kortvärdeBank = kortlek.beräknaKortvärde(bankListaKort);
                            genväg = @"C:\\Black Jack\bilder\Spelkort\" + nyKortlek[draKort] + ".png";
                            ritaBild = Image.FromFile(genväg);
                            dragnaKort.Add(new Tuple<Image, Point>(ritaBild, new Point(bankPosX, bankPosY)));
                            bankPosX += 15;
                            nyKortlek.RemoveAt(draKort);
                            spelkortPanel.Invalidate();
                            await Task.Delay(500);
                        }
                        else
                        {
                            spelarListaKort.Add(kortlek.hämtaKortvärde(nyKortlek[draKort]));
                            kortvärdeSpelare = kortlek.beräknaKortvärde(spelarListaKort);
                            genväg = @"C:\\Black Jack\\bilder\\Spelkort\\" + nyKortlek[draKort] + ".png";
                            ritaBild = Image.FromFile(genväg);
                            dragnaKort.Add(new Tuple<Image, Point>(ritaBild, new Point(spelarPosX, spelarPosY)));
                            spelarPosX += 15;
                            nyKortlek.RemoveAt(draKort);
                            spelkortPanel.Invalidate();
                            await Task.Delay(500);
                        }

                    }
                    pågåendeRunda = true;
                }
                else if (spelaresTur)
                {
                    spelaLjud(@"C:\\Black Jack\Audio\kortspelas.wav");
                    draKort = rnd.Next(nyKortlek.Count);
                    spelarListaKort.Add(kortlek.hämtaKortvärde(nyKortlek[draKort]));
                    kortvärdeSpelare = kortlek.beräknaKortvärde(spelarListaKort);
                    genväg = @"C:\\Black Jack\\bilder\\Spelkort\\" + nyKortlek[draKort] + ".png";
                    ritaBild = Image.FromFile(genväg);
                    dragnaKort.Add(new Tuple<Image, Point>(ritaBild, new Point(spelarPosX, spelarPosY)));
                    spelarPosX += 15;
                    nyKortlek.RemoveAt(draKort);
                    kollaVinnare();
                    spelkortPanel.Invalidate();
                    await Task.Delay(500);
                }
                else
                {
                    spelaLjud(@"C:\\Black Jack\Audio\kortspelas.wav");
                    draKort = rnd.Next(nyKortlek.Count);
                    bankListaKort.Add(kortlek.hämtaKortvärde(nyKortlek[draKort]));
                    kortvärdeBank = kortlek.beräknaKortvärde(bankListaKort);
                    genväg = @"C:\\Black Jack\bilder\Spelkort\" + nyKortlek[draKort] + ".png";
                    ritaBild = Image.FromFile(genväg);
                    dragnaKort.Add(new Tuple<Image, Point>(ritaBild, new Point(bankPosX, bankPosY)));
                    bankPosX += 15;
                    nyKortlek.RemoveAt(draKort);
                    spelkortPanel.Invalidate();
                    await Task.Delay(500);
                    kollaVinnare();

                }
            }
            else
            {
                MessageBox.Show("Du måste satsa något", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void passa()
        {
            spelaresTur = false;
            //while (kortvärdeBank < 17 && kortvärdeBank < kortvärdeSpelare)
            //{
            //    spelaKort();
            //}
            kollaVinnare();


            spelkortPanel.Invalidate();
        }

        private void kollaVinnare()
        {
            if (kortvärdeSpelare > 21)
            {
                antalMarker -= markervärde.Sum();
                pågåendeRunda = false;
                totalBet.Text = "Du förlorade $" + markervärde.Sum();
                bankrulle.Text = spelarNamn + " du har $" + antalMarker;
                spelaresTur = true;
                spelaLjudUtanSync(@"C:\\Black Jack\Audio\förlust.wav");
                uppdateraFil();
                markervärde.Clear();
                maxBet();
                knapparPanel.Invalidate();
                markerPanel.Invalidate();
                satsatPanel.Invalidate();
            }
            else if (kortvärdeBank > 21)
            {
                antalMarker += markervärde.Sum();
                pågåendeRunda = false;
                totalBet.Text = "Du vann $" + markervärde.Sum();
                bankrulle.Text = spelarNamn + " du har $" + antalMarker;
                spelaresTur = true;
                spelaLjudUtanSync(@"C:\\Black Jack\Audio\vinst.wav");
                uppdateraFil();
                markervärde.Clear();
                maxBet();
                knapparPanel.Invalidate();
                markerPanel.Invalidate();
                satsatPanel.Invalidate();
            }
            else if (kortvärdeBank >= kortvärdeSpelare && kortvärdeBank > 16 && kortvärdeBank < 22)
            {
                antalMarker -= markervärde.Sum();
                pågåendeRunda = false;
                totalBet.Text = "Du förlorade $" + markervärde.Sum();
                bankrulle.Text = spelarNamn + " du har $" + antalMarker;
                spelaresTur = true;
                spelaLjudUtanSync(@"C:\\Black Jack\Audio\förlust.wav");
                uppdateraFil();
                markervärde.Clear();
                maxBet();
                knapparPanel.Invalidate();
                markerPanel.Invalidate();
                satsatPanel.Invalidate();
            }
            else if (!spelaresTur) spelaKort();
        }

        private void maxBet()
        {

            for (int i = 0; i < kollaKryss.Length; i++)
            {
                kollaKryss[i] = false;
            }
            if (markervärde.Sum() > 0)
            {
                kollaKryss[5] = true;
            }
            if (markervärde.Sum() > 500)
            {
                kollaKryss[4] = true;
            }
            if (markervärde.Sum() > 900)
            {
                kollaKryss[3] = true;
            }
            if (markervärde.Sum() > 950)
            {
                kollaKryss[2] = true;
            }
            if (markervärde.Sum() > 990)
            {
                kollaKryss[1] = true;
            }
            if (markervärde.Sum() > 995)
            {
                kollaKryss[0] = true;
            }
        }
        private void resetKort()
        {
            dragnaKort.Clear();
            kortvärdeBank = 0;
            kortvärdeSpelare = 0;
            spelarListaKort.Clear();
            bankListaKort.Clear();
            nyKortlek.Clear();
        }

        #endregion


        #region läggtillbilder
        private void läggTillKnappBilder()
        {
            ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Knappar\hit.png");
            knappar.Add(new Tuple<Image, Point>(ritaBild, new Point(0, 0)));


            ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Knappar\pass.png");
            knappar.Add(new Tuple<Image, Point>(ritaBild, new Point(0, 50)));

            knapparPanel.Invalidate();
            PictureBox test = new PictureBox();
            test.Size = new Size(50, 50);
            test.Location = new Point(100, 100);
            test.BackColor = Color.Transparent;
            test.BorderStyle = BorderStyle.None;

            test.Image = Image.FromFile(@"C:\\Black Jack\bilder\Knappar\hit.png");
            this.Controls.Add(test);

            PictureBox test1 = new PictureBox();
            test1.Size = new Size(50, 50);
            test1.Location = new Point(100, 120);
            test1.BackColor = Color.Transparent;
            test1.Image = Image.FromFile(@"C:\\Black Jack\bilder\Knappar\pass.png");
            test1.BringToFront();
            test1.BorderStyle = BorderStyle.None;
            this.Controls.Add(test1);
        }
        private void läggTillMarkerBilder()
        {
            for (int i = 0; i < 6; i++)
            {
                switch (i)
                {
                    case 0:
                        ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\5$.png");
                        markerBild.Add(new Tuple<Image, Point>(ritaBild, new Point(0, 0)));
                        break;
                    case 1:
                        ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\10$.png");
                        markerBild.Add(new Tuple<Image, Point>(ritaBild, new Point(70, 10)));
                        break;
                    case 2:
                        ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\50$.png");
                        markerBild.Add(new Tuple<Image, Point>(ritaBild, new Point(140, 20)));
                        break;
                    case 3:
                        ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\100$.png");
                        markerBild.Add(new Tuple<Image, Point>(ritaBild, new Point(230, 20)));
                        break;
                    case 4:
                        ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\500$.png");
                        markerBild.Add(new Tuple<Image, Point>(ritaBild, new Point(300, 10)));
                        break;
                    case 5:
                        ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\1000$.png");
                        markerBild.Add(new Tuple<Image, Point>(ritaBild, new Point(370, 0)));
                        break;
                }
            }
            markerPanel.Invalidate();
        }
        #endregion


        #region Mouseclick
        private void knapparPanel_MouseClick(object sender, MouseEventArgs e)
        {
            int x = 0;
            foreach (var imageTuple in knappar)
            {
                Rectangle imageBounds = new Rectangle(imageTuple.Item2, new Size(50, 50));
                if (imageBounds.Contains(e.Location))
                {
                    switch (x)
                    {
                        case 0:
                            spelaKort();
                            break;
                        case 1:
                            if (pågåendeRunda)
                            {
                                passa();
                            }
                            break;
                    }

                }
                x++;
            }
        }


        private void markerPanel_MouseClick(object sender, MouseEventArgs e)
        {
            int x = 0;
            if (!pågåendeRunda)
            {
                dragnaKort.Clear();
                spelkortPanel.Invalidate();
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
                                    markervärde = marker.raderaMarker(markervärde, 5);
                                }
                                else
                                {
                                    if (!kollaKryss[0])
                                    {

                                        if (antalMarker > 5)
                                        {
                                            markervärde.Add(5);
                                            markervärde = marker.sortera(markervärde);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Du har inte tillräckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                                maxBet();
                                spelaLjudUtanSync(@"C:\\Black Jack\Audio\satsamarker.wav");
                                markerPanel.Invalidate();
                                satsatPanel.Invalidate();
                                break;
                            case 1:
                                if (e.Button == MouseButtons.Right)
                                {
                                    markervärde = marker.raderaMarker(markervärde, 10);
                                }
                                else
                                {
                                    if (!kollaKryss[1])
                                    {
                                        if (antalMarker >= 10)
                                        {
                                            markervärde.Add(10);
                                            markervärde = marker.sortera(markervärde);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Du har inte tillräckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                                maxBet();
                                spelaLjudUtanSync(@"C:\\Black Jack\Audio\satsamarker.wav");
                                markerPanel.Invalidate();
                                satsatPanel.Invalidate();
                                break;
                            case 2:
                                if (e.Button == MouseButtons.Right)
                                {
                                    markervärde = marker.raderaMarker(markervärde, 50);
                                }
                                else
                                {
                                    if (!kollaKryss[2])
                                    {
                                        if (antalMarker >= 50)
                                        {
                                            markervärde.Add(50);
                                            markervärde = marker.sortera(markervärde);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Du har inte tillräckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                                maxBet();
                                spelaLjudUtanSync(@"C:\\Black Jack\Audio\satsamarker.wav");
                                markerPanel.Invalidate();
                                satsatPanel.Invalidate();
                                break;
                            case 3:
                                if (e.Button == MouseButtons.Right)
                                {
                                    markervärde = marker.raderaMarker(markervärde, 100);
                                }
                                else
                                {
                                    if (!kollaKryss[3])
                                    {
                                        if (antalMarker >= 100)
                                        {
                                            markervärde.Add(100);
                                            markervärde = marker.sortera(markervärde);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Du har inte tillräckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                                maxBet();
                                spelaLjudUtanSync(@"C:\\Black Jack\Audio\satsamarker.wav");
                                markerPanel.Invalidate();
                                satsatPanel.Invalidate();
                                break;
                            case 4:
                                if (e.Button == MouseButtons.Right)
                                {
                                    markervärde = marker.raderaMarker(markervärde, 500);
                                }
                                else
                                {
                                    if (!kollaKryss[4])
                                    {
                                        if (antalMarker >= 500)
                                        {
                                            markervärde.Add(500);
                                            markervärde = marker.sortera(markervärde);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Du har inte tillräckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                                maxBet();
                                spelaLjudUtanSync(@"C:\\Black Jack\Audio\satsamarker.wav");
                                markerPanel.Invalidate();
                                satsatPanel.Invalidate();
                                break;
                            case 5:
                                if (e.Button == MouseButtons.Right)
                                {
                                    markervärde = marker.raderaMarker(markervärde, 1000);
                                }
                                else
                                {
                                    if (!kollaKryss[5])
                                    {
                                        if (antalMarker >= 1000)
                                        {
                                            markervärde.Add(1000);
                                            markervärde = marker.sortera(markervärde);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Du har inte tillräckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                                maxBet();
                                spelaLjudUtanSync(@"C:\\Black Jack\Audio\satsamarker.wav");
                                markerPanel.Invalidate();
                                satsatPanel.Invalidate();
                                break;
                        }
                        knapparPanel.Invalidate();
                    }
                    x++;
                    totalBet.Text = "$" + markervärde.Sum();
                }
            }
        }

        #endregion




    }

}