using System.Drawing.Imaging;
using System.Media;
using NAudio.Wave;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing.Drawing2D;
using System.Timers;

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
        List<int> markerv�rde = new List<int>(); //Hur m�nga marker spelar har satsat
        List<int> bankListaKort = new List<int>();
        List<int> spelarListaKort = new List<int>();
        List<PictureBox> bankkort = new List<PictureBox>();
        List<PictureBox> spelarkort = new List<PictureBox>();
        bool[] kollaKryss = new bool[] { false, false, false, false, false, false };
        int kortv�rdeBank, kortv�rdeSpelare;
        int antalMarker = 5000; //Hur m�nga marker spelaren har
        int posX = 0;
        int posY = 0;
        int spelare1bet;
        bool p�g�endeRunda = false;
        bool spelaresTur = true;
        Image ritaBild;
        List<Tuple<Image, Point>> imagesToDraw = new List<Tuple<Image, Point>>();
        //List<Tuple<Image, Point>> markerBild = new List<Tuple<Image, Point>>();
        //List<Tuple<Image, Point>> dragnaKort = new List<Tuple<Image, Point>>();
        //List<Tuple<Image, Point>> knappar = new List<Tuple<Image, Point>>();
        private Panel bankkortPanel;
        private Panel spelarkortPanel;
        private Panel satsatPanel;
        PictureBox dollar5, dollar10, dollar50, dollar100, dollar500, dollar1000;
        PictureBox knappHit, knappPass, knappDouble, knappSplit, knappInsurance;
        PictureBox ram;
        Marker marker = new Marker();
        Label totalBet = new Label();
        Label betInfo = new Label();
        Label bankrulle = new Label();
        Label visaspelare1bet = new Label();
        string spelarNamn = "";
        login loggain;
        System.Windows.Forms.ToolTip hitTooltip;
        System.Windows.Forms.ToolTip passTooltip;
        System.Windows.Forms.ToolTip doubleTooltip;
        System.Windows.Forms.ToolTip splitTooltip;
        System.Windows.Forms.ToolTip insuranceTooltip;


 
        private void spelaLjud(string genv�g)
        {
            using (SoundPlayer ljudspelare = new SoundPlayer(genv�g))
            {
                ljudspelare.PlaySync();
            }
        }

        private void spelaLjudUtanSync(string genv�g)
        {
            using (SoundPlayer ljudspelare = new SoundPlayer(genv�g))
            {
                ljudspelare.Play();

            }
        }

        private void uppdateraFil()
        {
            string x = "";
            string filgenv�g = @"C:\\Black Jack\spelarinfo.txt";
            List<string> spelarlista = new List<string>();
            using (StreamReader l�sfil = new StreamReader(filgenv�g))
            {
                string spelare;
                while ((spelare = l�sfil.ReadLine()) != null)
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

            using (StreamWriter skrivFil = new StreamWriter(filgenv�g))
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

        private void tooltips()
        {
            hitTooltip = new System.Windows.Forms.ToolTip();
            hitTooltip.AutoPopDelay = 5000;
            hitTooltip.InitialDelay = 400;
            hitTooltip.ReshowDelay = 200;
            hitTooltip.SetToolTip(this.knappHit, "Hit!");

            passTooltip = new System.Windows.Forms.ToolTip();
            passTooltip.AutoPopDelay = 5000;
            passTooltip.InitialDelay = 400;
            passTooltip.ReshowDelay = 200;
            passTooltip.SetToolTip(this.knappPass, "Pass!");

            doubleTooltip = new System.Windows.Forms.ToolTip();
            doubleTooltip.AutoPopDelay = 5000;
            doubleTooltip.InitialDelay = 400;
            doubleTooltip.ReshowDelay = 200;
            doubleTooltip.SetToolTip(this.knappDouble, "Double!");

            //splitTooltip = new System.Windows.Forms.ToolTip();
            //splitTooltip.AutoPopDelay = 5000;
            //splitTooltip.InitialDelay = 1000;
            //splitTooltip.ReshowDelay = 200;
            //splitTooltip.SetToolTip(this.knappSplit, "Split!");

            //insuranceTooltip = new System.Windows.Forms.ToolTip();
            //insuranceTooltip.AutoPopDelay = 5000;
            //insuranceTooltip.InitialDelay = 1000;
            //insuranceTooltip.ReshowDelay = 200;
            //insuranceTooltip.SetToolTip(this.knappInsurance, "Insurance!");

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            this.Hide();
            loggain = new login();
            loggain.ShowDialog();
            this.Size = new System.Drawing.Size(1440, 800);
            this.Location = new Point(0,0);
            this.StartPosition = FormStartPosition.Manual;
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

            bankrulle.Location = new Point(this.Width / 2 - 420, this.Height / 2 + 100);
            bankrulle.Font = new Font("MS Gothic", 14, FontStyle.Bold);
            bankrulle.BackColor = Color.Transparent;
            bankrulle.AutoSize = true;
            this.Controls.Add(bankrulle);
            totalBet.BringToFront();
            bankrulle.Text = spelarNamn + ", du har $" + antalMarker;

            visaspelare1bet.Location = new Point(this.Width / 2 - 35, this.Height / 2 + 200);
            visaspelare1bet.Font = new Font("MS Gothic", 14, FontStyle.Bold);
            visaspelare1bet.BackColor = Color.LightGray;
            visaspelare1bet.AutoSize = true;
            this.Controls.Add(visaspelare1bet);
            visaspelare1bet.BringToFront();
            visaspelare1bet.Text = "$" + markerv�rde.Sum();

            this.DoubleBuffered = true;

            bankkortPanel = new Panel();
            bankkortPanel.Size = new Size(50, 72);
            bankkortPanel.Location = new Point(this.Width / 2 - 50, this.Height / 2  - 185);
            bankkortPanel.BackColor = Color.Transparent;
            this.Controls.Add(bankkortPanel);

            spelarkortPanel = new Panel();
            spelarkortPanel.Size = new Size(50, 72);
            spelarkortPanel.Location = new Point(this.Width / 2 - 50, this.Height / 2 + 125);
            spelarkortPanel.BackColor = Color.Transparent;
            this.Controls.Add(spelarkortPanel);
            spelarkortPanel.Hide();

            ram = new PictureBox();
            ram.Size = new Size(50, 72);
            ram.Location = new Point(200,200);
            ram.BackColor = Color.Transparent;
            Image rambild = Image.FromFile(@"C:\\Black Jack\bilder\vitram.png");
            //ram.Image = DrawTranslucentRoundedRectangle();
            ram.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(ram);
            DrawTranslucentRoundedRectangle();
            ram.Click += new EventHandler(ram_click);

            //satsatPanel = new Panel();
            //satsatPanel.Size = new Size(70, 60);
            //satsatPanel.Location = new Point(this.Width / 2 - 50, this.Height / 2 + 125);
            //satsatPanel.BackColor = Color.Transparent;
            //satsatPanel.Paint += new PaintEventHandler(ritaSatsadeMarker);
            //this.Controls.Add(satsatPanel);

            skapaMarkerBilder();
            skapaKnappBilder();
            tooltips();

        }
        
        private void ram_click(object sender, EventArgs e)
        {
            DrawTranslucentRoundedRectangle1();
        }

        #region ritaramar
        private void DrawTranslucentRoundedRectangle()
        {
            // Define the rectangle dimensions
            int width = 50;
            int height = 72;

            // Create a bitmap with a transparent background
            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Pen pen = new Pen(Color.Black, 2); // Adjust the color and width of the border

                // Draw the border of the rounded rectangle
                RectangleF rect = new RectangleF(0, 0, width - 1, height - 1);
                float radius = 10; // Adjust the corner radius as needed
                GraphicsPath path = GetRoundedRectangle(rect, radius);
                g.DrawPath(pen, path);
            }

            // Display the bitmap in the PictureBox
            ram.Image = bitmap;
        }

        private void DrawTranslucentRoundedRectangle1()
        {
            // Define the rectangle dimensions
            int width = 50;
            int height = 72;

            // Create a bitmap with a transparent background
            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                SolidBrush brush = new SolidBrush(Color.LightBlue); // Change the color as needed

                // Draw the rounded rectangle
                RectangleF rect = new RectangleF(0, 0, width - 1, height - 1);
                float radius = 10; // Adjust the corner radius as needed
                GraphicsPath path = GetRoundedRectangle(rect, radius);
                g.FillPath(brush, path);

                // Draw the border of the rounded rectangle
                Pen pen = new Pen(Color.Black, 2); // Adjust the color and width of the border
                g.DrawPath(pen, path);
            }

            // Display the bitmap in the PictureBox
            ram.Image = bitmap;
        }

        private GraphicsPath GetRoundedRectangle(RectangleF baseRect, float radius)
        {
            float diameter = radius * 2;
            SizeF sizeF = new SizeF(diameter, diameter);
            RectangleF arc = new RectangleF(baseRect.Location, sizeF);
            GraphicsPath path = new GraphicsPath();

            path.AddArc(arc, 180, 90);
            arc.X = baseRect.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = baseRect.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = baseRect.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();

            return path;
        }

        #endregion


        private void rensaKort()
        {
            foreach (Control control in bankkortPanel.Controls.OfType<Control>().ToList())
            {
                bankkortPanel.Controls.Remove(control);
            }
            foreach (Control control in spelarkortPanel.Controls.OfType<Control>().ToList())
            {
                spelarkortPanel.Controls.Remove(control);
            }
        }

        private void visaBankKort()
        {
            bankkortPanel.Width = 50 + (bankkort.Count - 1) * 15;
            for (int i = bankkort.Count - 1; i >= 0; i--)
            {
                bankkort[i].Location = new Point(i * 15, 0);
                bankkortPanel.Controls.Add(bankkort[i]);
            }
        }

        private void visaSpelarKort()
        {
            spelarkortPanel.Width = 50 + (spelarkort.Count - 1) * 15;
            for (int i = spelarkort.Count - 1; i > -1; i--)
            {
                spelarkort[i].Location = new Point(i * 15, 0);
                spelarkortPanel.Controls.Add(spelarkort[i]);
            }
        }


        #region spellogik
        private async Task spelaKort()
        {
            if (markerv�rde.Sum() > 0)
            {

                string genv�g;
                Random rnd = new Random();
                int draKort;
                if (!p�g�endeRunda)
                {
                    spelaLjud(@"C:\\Black Jack\Audio\lekblandas.wav");
                    resetKort();
                    nyKortlek = kortlek.Nykortlek();
                    for (int i = 0; i < 3; i++)
                    {
                        spelaLjud(@"C:\\Black Jack\Audio\kortspelas.wav");

                        draKort = rnd.Next(nyKortlek.Count);
                        if (i == 0)
                        {
                            bankListaKort.Add(kortlek.h�mtaKortv�rde(nyKortlek[draKort]));
                            kortv�rdeBank = kortlek.ber�knaKortv�rde(bankListaKort);
                            skapaBankkortsBilder(draKort);
                            visaBankKort();
                            nyKortlek.RemoveAt(draKort);
                            await Task.Delay(500);
                        }
                        else
                        {
                            spelarListaKort.Add(kortlek.h�mtaKortv�rde(nyKortlek[draKort]));
                            kortv�rdeSpelare = kortlek.ber�knaKortv�rde(spelarListaKort);
                            skapaSpelarkortsBilder(draKort);
                            visaSpelarKort();
                            nyKortlek.RemoveAt(draKort);
                            await Task.Delay(500);
                        }

                    }
                    knappDouble.Show();
                    knappPass.Show();
                    p�g�endeRunda = true;
                }
                else if (spelaresTur)
                {
                    spelaLjud(@"C:\\Black Jack\Audio\kortspelas.wav");
                    draKort = rnd.Next(nyKortlek.Count);
                    spelarListaKort.Add(kortlek.h�mtaKortv�rde(nyKortlek[draKort]));
                    kortv�rdeSpelare = kortlek.ber�knaKortv�rde(spelarListaKort);
                    skapaSpelarkortsBilder(draKort);
                    visaSpelarKort();
                    nyKortlek.RemoveAt(draKort);
                    await Task.Delay(500);
                    knappDouble.Location = new Point(this.Width / 2 - 50 + spelarkortPanel.Width + 25, this.Height / 2 + 125);
                    //kollaVinnare();
                }
                else
                {
                    spelaLjud(@"C:\\Black Jack\Audio\kortspelas.wav");
                    draKort = rnd.Next(nyKortlek.Count);
                    bankListaKort.Add(kortlek.h�mtaKortv�rde(nyKortlek[draKort]));
                    kortv�rdeBank = kortlek.ber�knaKortv�rde(bankListaKort);
                    skapaBankkortsBilder(draKort);
                    visaBankKort(); ;
                    nyKortlek.RemoveAt(draKort);
                    await Task.Delay(500);
                    kollaVinnare();

                }
            }
            else
            {
                MessageBox.Show("Du m�ste satsa n�got", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void passa()
        {
            spelaresTur = false;
            kollaVinnare();
        }

        private void kollaVinnare()
        {
            if (kortv�rdeSpelare > 21)
            {
                antalMarker -= spelare1bet;
                p�g�endeRunda = false;
                totalBet.Text = "Du f�rlorade $" + markerv�rde.Sum();
                bankrulle.Text = spelarNamn + " du har $" + antalMarker;
                spelaresTur = true;
                spelaLjudUtanSync(@"C:\\Black Jack\Audio\f�rlust.wav");
                uppdateraFil();
                markerv�rde.Clear();
                maxBet();
            }
            else if (kortv�rdeBank > 21)
            {
                antalMarker += spelare1bet;
                p�g�endeRunda = false;
                totalBet.Text = "Du vann $" + markerv�rde.Sum();
                bankrulle.Text = spelarNamn + " du har $" + antalMarker;
                spelaresTur = true;
                spelaLjudUtanSync(@"C:\\Black Jack\Audio\vinst.wav");
                uppdateraFil();
                markerv�rde.Clear();
                maxBet();
            }
            else if (kortv�rdeBank >= kortv�rdeSpelare && kortv�rdeBank > 16 && kortv�rdeBank < 22)
            {
                antalMarker -= spelare1bet;
                p�g�endeRunda = false;
                totalBet.Text = "Du f�rlorade $" + markerv�rde.Sum();
                bankrulle.Text = spelarNamn + " du har $" + antalMarker;
                spelaresTur = true;
                spelaLjudUtanSync(@"C:\\Black Jack\Audio\f�rlust.wav");
                uppdateraFil();
                markerv�rde.Clear();
                maxBet();
            }
            else if (!spelaresTur) spelaKort();
        }

        private void maxBet()
        {

            for (int i = 0; i < kollaKryss.Length; i++)
            {
                kollaKryss[i] = false;
            }
            if (markerv�rde.Sum() > 0)
            {
                 kollaKryss[5] = true;
                dollar1000.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\1000$kryss.png");
            }
            else dollar1000.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\1000$.png");
            
            if (markerv�rde.Sum() > 500)
            {
                kollaKryss[4] = true;
                dollar500.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\500$kryss.png");
            }
            else dollar500.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\500$.png");
           
            if (markerv�rde.Sum() > 900)
            {
                kollaKryss[3] = true;
                dollar100.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\100$kryss.png");
            }
            else dollar100.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\100$.png");
            
            if (markerv�rde.Sum() > 950)
            {
                kollaKryss[2] = true;
                dollar50.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\50$kryss.png");
            }
            else dollar50.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\50$.png");
            
            if (markerv�rde.Sum() > 990)
            {
                kollaKryss[1] = true;
                dollar10.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\10$kryss.png");
            }
            else dollar10.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\10$.png");
            
            if (markerv�rde.Sum() > 995)
            {
                kollaKryss[0] = true;
                dollar5.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\5$kryss.png");
            }
            else dollar5.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\5$.png");
        }
        private void resetKort()
        {
            kortv�rdeBank = 0;
            kortv�rdeSpelare = 0;
            spelarListaKort.Clear();
            bankkort.Clear();
            spelarkort.Clear();
            bankListaKort.Clear();
            nyKortlek.Clear();
        }

        #endregion



        #region l�ggtillbilder

        private void skapaRamarBilder()
        {

        }
        private void skapaBankkortsBilder(int x)
        {
            PictureBox kort = new PictureBox();
            kort.Size = new Size(50, 72);
            kort.BringToFront();
            string genv�g = @"C:\\Black Jack\bilder\Spelkort\" + nyKortlek[x] + ".png";
            ritaBild = Image.FromFile(genv�g);
            kort.Image = ritaBild;
            bankkort.Add(kort);
        }

        private void skapaSpelarkortsBilder(int x)
        {

            PictureBox kort = new PictureBox();
            kort.Size = new Size(50, 72);
            kort.BringToFront();
            string genv�g = @"C:\\Black Jack\bilder\Spelkort\" + nyKortlek[x] + ".png";
            ritaBild = Image.FromFile(genv�g);
            kort.Image = ritaBild;
            spelarkort.Add(kort);
        }
        private void skapaKnappBilder()
        {
            knappHit = new PictureBox();
            knappHit.Size = new Size(30, 30);
            knappHit.Location = new Point(this.Width / 2 - 90, this.Height / 2 +125);
            knappHit.BackColor = Color.Transparent;
            knappHit.Image = Image.FromFile(@"C:\\Black Jack\bilder\Knappar\hit.png");
            knappHit.SizeMode = PictureBoxSizeMode.StretchImage;
            knappHit.BringToFront();
            this.Controls.Add(knappHit);
            knappHit.MouseClick += new MouseEventHandler(knappHit_click);

            knappPass = new PictureBox();
            knappPass.Size = new Size(30, 30);
            knappPass.Location = new Point(this.Width / 2 - 90, this.Height / 2 + 160 );
            knappPass.BackColor = Color.Transparent;
            knappPass.Image = Image.FromFile(@"C:\\Black Jack\bilder\Knappar\pass.png");
            knappPass.SizeMode = PictureBoxSizeMode.StretchImage;
            knappPass.BringToFront();
            this.Controls.Add(knappPass);
            knappPass.MouseClick += new MouseEventHandler(knappPass_click);
            knappPass.Hide();

            knappDouble = new PictureBox();
            knappDouble.Size = new Size(30, 30);
            knappDouble.Location = new Point(this.Width / 2 -50 + spelarkortPanel.Width + 25, this.Height / 2 + 125);
            knappDouble.BackColor = Color.Transparent;
            knappDouble.Image = Image.FromFile(@"C:\\Black Jack\bilder\Knappar\double.png");
            knappDouble.SizeMode = PictureBoxSizeMode.StretchImage;
            knappDouble.BringToFront();
            this.Controls.Add(knappDouble);
            knappDouble.MouseClick += new MouseEventHandler(knappDouble_click);
            knappDouble.Hide();
        }

        private void skapaMarkerBilder()
        {
            dollar5 = new PictureBox();
            dollar5.Location = new Point(this.Width / 2 - 244, this.Height / 2 + 225);
            dollar5.Size = new Size(80, 80);
            dollar5.BackColor = Color.Transparent;
            dollar5.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\5$.png");
            dollar5.BringToFront();
            this.Controls.Add(dollar5);
            dollar5.MouseClick += new MouseEventHandler(dollar5_click);

            dollar10 = new PictureBox();
            dollar10.Location = new Point(this.Width / 2 - 174, this.Height / 2 + 235);
            dollar10.Size = new Size(80, 80);
            dollar10.BackColor = Color.Transparent;
            dollar10.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\10$.png");
            dollar10.BringToFront();
            this.Controls.Add(dollar10);
            dollar10.MouseClick += new MouseEventHandler(dollar10_click);

            dollar50 = new PictureBox();
            dollar50.Location = new Point(this.Width / 2 - 104, this.Height / 2 + 245);
            dollar50.Size = new Size(80, 80);
            dollar50.BackColor = Color.Transparent;
            dollar50.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\50$.png");
            dollar50.BringToFront();
            this.Controls.Add(dollar50);
            dollar50.MouseClick += new MouseEventHandler(dollar50_click);

            dollar100 = new PictureBox();
            dollar100.Location = new Point(this.Width / 2 - 34, this.Height / 2 + 245);
            dollar100.Size = new Size(80, 80);
            dollar100.BackColor = Color.Transparent;
            dollar100.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\100$.png");
            dollar100.BringToFront();
            this.Controls.Add(dollar100);
            dollar100.MouseClick += new MouseEventHandler(dollar100_click);

            dollar500 = new PictureBox();
            dollar500.Location = new Point(this.Width / 2 + 44, this.Height / 2 + 235);
            dollar500.Size = new Size(80, 80);
            dollar500.BackColor = Color.Transparent;
            dollar500.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\500$.png");
            dollar500.BringToFront();
            this.Controls.Add(dollar500);
            dollar500.MouseClick += new MouseEventHandler(dollar500_click);

            dollar1000 = new PictureBox();
            dollar1000.Location = new Point(this.Width / 2 + 114, this.Height / 2 + 225);
            dollar1000.Size = new Size(80, 80);
            dollar1000.BackColor = Color.Transparent;
            dollar1000.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\1000$.png");
            dollar1000.BringToFront();  
            this.Controls.Add(dollar1000);
            dollar1000.MouseClick += new MouseEventHandler(dollar1000_click);
        }





        #endregion

        #region Mouseclick
        private void dollar5_click(object sender, MouseEventArgs e)
        {
            if (!p�g�endeRunda)
            {
                rensaKort();
            }
            if (e.Button == MouseButtons.Right)
            {
                markerv�rde = marker.raderaMarker(markerv�rde, 5);
                visaspelare1bet.Text = "$" + markerv�rde.Sum();
            }
            else
            {
                if (!kollaKryss[0])
                {

                    if (antalMarker > 5)
                    {
                        markerv�rde.Add(5);
                        markerv�rde = marker.sortera(markerv�rde);
                        visaspelare1bet.Text = "$" + markerv�rde.Sum();
                    }
                    else
                    {
                        MessageBox.Show("Du har inte tillr�ckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            maxBet();
            spelaLjudUtanSync(@"C:\\Black Jack\Audio\satsamarker.wav");
        }
        private void dollar10_click(object sender, MouseEventArgs e)
        {
            if (!p�g�endeRunda)
            {
                rensaKort();
            }
            if (e.Button == MouseButtons.Right)
            {
                if (10 > markerv�rde.Sum())
                {
                    markerv�rde.Clear();
                    visaspelare1bet.Text = "$" + markerv�rde.Sum();

                }
                else
                {
                    markerv�rde = marker.raderaMarker(markerv�rde, 10);
                    visaspelare1bet.Text = "$" + markerv�rde.Sum();
                }
            }
            else
            {
                if (!kollaKryss[1])
                {
                    if (antalMarker >= 10)
                    {
                        markerv�rde.Add(10);
                        markerv�rde = marker.sortera(markerv�rde);
                        visaspelare1bet.Text = "$" + markerv�rde.Sum();
                    }
                    else
                    {
                        MessageBox.Show("Du har inte tillr�ckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            maxBet();
            spelaLjudUtanSync(@"C:\\Black Jack\Audio\satsamarker.wav");
        }
        private void dollar50_click(object sender, MouseEventArgs e)
        {
            if (!p�g�endeRunda)
            {
                rensaKort();
            }
            if (e.Button == MouseButtons.Right)
            {
                if (50 > markerv�rde.Sum())
                {
                    markerv�rde.Clear();
                    visaspelare1bet.Text = "$" + markerv�rde.Sum();

                }
                else
                {
                    markerv�rde = marker.raderaMarker(markerv�rde, 50);
                    visaspelare1bet.Text = "$" + markerv�rde.Sum();
                }
            }
            else
            {
                if (!kollaKryss[2])
                {
                    if (antalMarker >= 50)
                    {
                        markerv�rde.Add(50);
                        markerv�rde = marker.sortera(markerv�rde);
                        visaspelare1bet.Text = "$" + markerv�rde.Sum();
                    }
                    else
                    {
                        MessageBox.Show("Du har inte tillr�ckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            maxBet();
            spelaLjudUtanSync(@"C:\\Black Jack\Audio\satsamarker.wav");
        }
        private void dollar100_click(object sender, MouseEventArgs e)
        {
            if (!p�g�endeRunda)
            {
                rensaKort();
            }
            if (e.Button == MouseButtons.Right)
            {
                if (100 > markerv�rde.Sum())
                {
                    markerv�rde.Clear();
                    visaspelare1bet.Text = "$" + markerv�rde.Sum();

                }
                else
                {
                    markerv�rde = marker.raderaMarker(markerv�rde, 100);
                    visaspelare1bet.Text = "$" + markerv�rde.Sum();
                }
            }
            else
            {
                if (!kollaKryss[3])
                {
                    if (antalMarker >= 100)
                    {
                        markerv�rde.Add(100);
                        markerv�rde = marker.sortera(markerv�rde);
                        visaspelare1bet.Text = "$" + markerv�rde.Sum();
                    }
                    else
                    {
                        MessageBox.Show("Du har inte tillr�ckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            maxBet();
            spelaLjudUtanSync(@"C:\\Black Jack\Audio\satsamarker.wav");
        }
        private void dollar500_click(object sender, MouseEventArgs e)
        {
            if (!p�g�endeRunda)
            {
                rensaKort();
            }
            if (e.Button == MouseButtons.Right)
            {
                if (500 > markerv�rde.Sum())
                {
                    markerv�rde.Clear();
                    visaspelare1bet.Text = "$" + markerv�rde.Sum();

                }
                else
                {
                    markerv�rde = marker.raderaMarker(markerv�rde, 500);
                    visaspelare1bet.Text = "$" + markerv�rde.Sum();
                }
            }
            else
            {
                if (!kollaKryss[4])
                {
                    if (antalMarker >= 500)
                    {
                        markerv�rde.Add(500);
                        markerv�rde = marker.sortera(markerv�rde);
                        visaspelare1bet.Text = "$" + markerv�rde.Sum();
                    }
                    else
                    {
                        MessageBox.Show("Du har inte tillr�ckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            maxBet();
            spelaLjudUtanSync(@"C:\\Black Jack\Audio\satsamarker.wav");
        }
        private void dollar1000_click(object sender, MouseEventArgs e)
        {
            if (!p�g�endeRunda)
            {
                rensaKort();
            }
            if (e.Button == MouseButtons.Right)
            {
                if (1000 > markerv�rde.Sum())
                {
                    markerv�rde.Clear();
                    visaspelare1bet.Text = "$" + markerv�rde.Sum();

                }
                else
                {
                    markerv�rde = marker.raderaMarker(markerv�rde, 1000);
                    visaspelare1bet.Text = "$" + markerv�rde.Sum();
                }
            }
            else
            {
                if (!kollaKryss[5])
                {
                    if (antalMarker >= 1000)
                    {
                        markerv�rde.Add(1000);
                        markerv�rde = marker.sortera(markerv�rde);
                        visaspelare1bet.Text = "$" + markerv�rde.Sum();
                    }
                    else
                    {
                        MessageBox.Show("Du har inte tillr�ckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            maxBet();
            spelaLjudUtanSync(@"C:\\Black Jack\Audio\satsamarker.wav");
        }

        private void knappHit_click(object sender, MouseEventArgs e)
        {
            spelarkortPanel.Show();
            satsatPanel.Hide();
            spelare1bet = markerv�rde.Sum();
            spelaKort();
        }

        private void knappPass_click(object sender, MouseEventArgs e)
        {
            if (p�g�endeRunda)
            {
                passa();
            }
        }

        private void knappDouble_click(object sender, MouseEventArgs e)
        {
            spelare1bet *= 2;
            visaspelare1bet.Text = "$" + spelare1bet;
            knappDouble.Hide();
            knappPass.Hide();
            spelaKort();
            passa();
        }
        #endregion

        //#region ritamarker
 
        ////private void ritaSatsadeMarker(object sender, PaintEventArgs e)
        ////{
        ////    satsatPanel.SuspendLayout();
        ////    int y = 0;
        ////    posX = 0;
        ////    posY = 0;
        ////    imagesToDraw.Clear();
        ////    foreach (int x in markerv�rde)
        ////    {
        ////        if (y == 5)
        ////        {
        ////            posX = 0;
        ////            posY = 50;
        ////        }
        ////        switch (x)
        ////        {
        ////            case 5:
        ////                ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\5$.png");
        ////                imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
        ////                posX += 10;
        ////                break;
        ////            case 10:
        ////                ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\10$.png");
        ////                imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
        ////                posX += 10;
        ////                break;
        ////            case 50:
        ////                ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\50$.png");
        ////                imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
        ////                posX += 10;
        ////                break;
        ////            case 100:
        ////                ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\100$.png");
        ////                imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
        ////                posX += 10;
        ////                break;
        ////            case 500:
        ////                ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\500$.png");
        ////                imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
        ////                posX += 10;
        ////                break;
        ////            case 1000:
        ////                ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\1000$.png");
        ////                imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
        ////                posX += 10;
        ////                break;
        ////        }
        ////        y++;
        ////    }
        ////    Graphics g = e.Graphics;

        ////    base.OnPaint(e);


        ////    Bitmap canvas = new Bitmap(satsatPanel.Width, satsatPanel.Height);
        ////    using (Graphics canvasGraphics = Graphics.FromImage(canvas))
        ////    {
        ////        canvasGraphics.Clear(Color.Transparent);

        ////        // Draw all images stored in the list
        ////        foreach (var imageTuple in imagesToDraw)
        ////        {
        ////            canvasGraphics.DrawImage(imageTuple.Item1, new Rectangle(imageTuple.Item2, new Size(80, 80)));
        ////        }

        ////        g.DrawImageUnscaled(canvas, Point.Empty);
        ////    }
        ////    satsatPanel.ResumeLayout();
        ////}

        //private void ritaSatsadeMarker(object sender, PaintEventArgs e)
        //{
        //    satsatPanel.SuspendLayout();
        //    int y = 0;
        //    posX = 0;
        //    posY = 0;
        //    imagesToDraw.Clear();
        //    for(int i = 0 ; i < markerv�rde.Count; i++) 
        //    {
        //        if (y == 5)
        //        {
        //            posX = 32;
        //            posY = 0;
        //        }
        //        switch (markerv�rde[i])
        //        {
        //            case 5:
        //                ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\5$.png");
        //                imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
        //                posY += 5;
        //                break;
        //            case 10:
        //                ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\10$.png");
        //                imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
        //                posY += 5;
        //                break;
        //            case 50:
        //                ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\50$.png");
        //                imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
        //                posY += 5;
        //                break;
        //            case 100:
        //                ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\100$.png");
        //                imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
        //                posY += 5;
        //                break;
        //            case 500:
        //                ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\500$.png");
        //                imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
        //                posY += 5;
        //                break;
        //            case 1000:
        //                ritaBild = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\1000$.png");
        //                imagesToDraw.Add(new Tuple<Image, Point>(ritaBild, new Point(posX, posY)));
        //                posY += 5;
        //                break;
        //        }
        //        y++;
        //    }
        //    Graphics g = e.Graphics;

        //    base.OnPaint(e);


        //    Bitmap canvas = new Bitmap(satsatPanel.Width, satsatPanel.Height);
        //    using (Graphics canvasGraphics = Graphics.FromImage(canvas))
        //    {
        //        canvasGraphics.Clear(Color.Transparent);

        //        // Draw all images stored in the list
        //        foreach (var imageTuple in imagesToDraw)
        //        {
        //            canvasGraphics.DrawImage(imageTuple.Item1, new Rectangle(imageTuple.Item2, new Size(50, 50)));
        //        }

        //        g.DrawImageUnscaled(canvas, Point.Empty);
        //    }
        //    satsatPanel.ResumeLayout();
        //}
        //#endregion

    }

}