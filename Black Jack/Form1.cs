using System.Drawing.Imaging;
using System.Media;
using NAudio.Wave;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing.Drawing2D;
using System.Timers;
using System.Drawing;
using System.Linq;
using System.Diagnostics.Eventing.Reader;
using System.ComponentModel;
using static Black_Jack.Spelare;

namespace Black_Jack
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            loggain = new login();
           
        }

        login loggain; //Klass f�r att hantera val av spelare och antal
        List<Spelare> spelarlista = new List<Spelare>(); //H�ller lista �ver alla spelare
        Spelare? spelare; //Instansierar spelarklassen

        //X, Y-koordinater f�r varje m�jlig spelare
        int[] kortPosX = new int[7] { 756, 155, 385, 615, 845, 1075, 1305 };
        int[] kortPosY = new int[7] { 250, 340, 460, 550, 550, 460, 340 };

        int vilkenSpelh�g = 0;

        //F�r bilder p� de olika markerna
        PictureBox dollar5 = new PictureBox(); 
        PictureBox dollar10 = new PictureBox();
        PictureBox dollar50 = new PictureBox();
        PictureBox dollar100 = new PictureBox();
        PictureBox dollar500 = new PictureBox();
        PictureBox dollar1000 = new PictureBox();

        List<PictureBox> ramar = new List<PictureBox>(); //H�ller ramarna man ser innan kort spelas

        //H�ller kolla p� om det �verstiger maxbet p� $1000
        bool[] kollaKryss = new bool[] { false, false, false, false, false, false }; //True om spelarens inte har tillr�ckligt med marker f�r att betta en vissa val�r
        bool[] kollaKryssV = new bool[] { false, false, false, false, false, false }; //Sparar den icke-aktive beth�gen
        bool[] kollaKryssH = new bool[] { false, false, false, false, false, false }; 


        bool p�g�endeRunda = false; //H�ller koll p� om det �r en p�g�ende spelrunda (true) eller om man �r i bettingfasen (false).
        bool vilkensatsat = true; //H�lla koll p� om v�nster eller h�ger bet, true = v�nster
        bool harDelat2 = false; //H�ller koll p� om de f�rsta tv� korten har delats ut
        bool f�rstaRunda = true; //H�ller koll om det �r den absolut f�rsta rundan som spelas efter att programmet startats

        //Knappar som spelaren kan trycka p�
        System.Windows.Forms.Button knappHit;
        System.Windows.Forms.Button knappPass;
        System.Windows.Forms.Button knappDouble;
        System.Windows.Forms.Button knappSplit;

        //Tooltips f�r varje knapp
        System.Windows.Forms.ToolTip hitTooltip = new System.Windows.Forms.ToolTip(); 
        System.Windows.Forms.ToolTip passTooltip = new System.Windows.Forms.ToolTip(); 
        System.Windows.Forms.ToolTip doubleTooltip = new System.Windows.Forms.ToolTip(); 
        System.Windows.Forms.ToolTip splitTooltip = new System.Windows.Forms.ToolTip(); 

        SolidBrush r�dpensel = new SolidBrush(Color.IndianRed); //F�rgl�gger gr�n d�r spelarens kan satsa
        SolidBrush gr�npensel = new SolidBrush(Color.LightGreen); //F�rgl�gger r�d d�r spelarens inte satsar
   



        private void spelaLjud(string genv�g) //Spelar upp en ljudeffekt med sync
        {
            using (SoundPlayer ljudspelare = new SoundPlayer(genv�g))
            {
                ljudspelare.PlaySync();
            }
        } 

        private void spelaLjudUtanSync(string genv�g) //Spelar upp en ljudeffekt utan sync
        {
            using (SoundPlayer ljudspelare = new SoundPlayer(genv�g))
            {
                ljudspelare.Play();

            }
        } 

        private void uppdateraFil() //Uppdaterar textfilen med f�r�ndrad kontobalans
        {
            string x = "";
            string filgenv�g = @"C:\\Black Jack\spelarinfo.txt";
            List<string> reggadeSpelare = new List<string>();

            using (StreamReader l�sfil = new StreamReader(filgenv�g)) //L�ser in inneh�llet i filen i en lista med string
            {
                string spelare;
                while ((spelare = l�sfil.ReadLine()) != null)
                {
                    reggadeSpelare.Add(spelare);
                }
            }
            for (int i = 0; i < reggadeSpelare.Count; i++) //Uppdaterar spelarens rad med aktuell kontobalans
            {
                if (reggadeSpelare[i].Contains(spelarinfo.spelarnamn)) //Kollar om raden inneh�ller spelarens namn
                {
                    x = reggadeSpelare[i];
                    int y = x.IndexOf(','); //Hittar index fr�n var i stringen den ska radera den gamla kontobalansen
                    x = x.Remove(y);  //Raderar den gamla kontobalansen
                    x += ", " + spelarinfo.kontobalans.ToString(); //L�gger till den nya kontobalansen till stringen
                    reggadeSpelare.Insert(i, x);  //L�gger till en uppdaterad string i listan med spelare
                    reggadeSpelare.RemoveAt(i + 1); //Raderar den gamla stringen
                }
            }

            using (StreamWriter skrivFil = new StreamWriter(filgenv�g)) //Ers�tter filen med den aktuella informationen
            {
                foreach (string spelare in reggadeSpelare)
                {
                    skrivFil.WriteLine(spelare);
                }
            }
        } 



        private void bytSpelareToolStripMenuItem_Click(object sender, EventArgs e) //Visar formen d�r man kan v�lja spelare etc.
        {
            nyOmg�ng(); //Nollst�ller spelomg�ngen
            loggain.ShowDialog(); 
        }

        private void tooltips() //Egenskaper f�r tooltips f�r knapparna
        {
            hitTooltip.AutoPopDelay = 5000;
            hitTooltip.InitialDelay = 400;
            hitTooltip.ReshowDelay = 200;
            hitTooltip.SetToolTip(this.knappHit, "Hit!");

            passTooltip.AutoPopDelay = 5000;
            passTooltip.InitialDelay = 400;
            passTooltip.ReshowDelay = 200;
            passTooltip.SetToolTip(this.knappPass, "Pass!");

            doubleTooltip.AutoPopDelay = 5000;
            doubleTooltip.InitialDelay = 400;
            doubleTooltip.ReshowDelay = 200;
            doubleTooltip.SetToolTip(this.knappDouble, "Double!");

            splitTooltip.AutoPopDelay = 5000;
            splitTooltip.InitialDelay = 1000;
            splitTooltip.ReshowDelay = 200;
            splitTooltip.SetToolTip(this.knappSplit, "Split!");
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            this.Hide();

            //Skapar och visar formen d�r man kan v�lja spelare etc.
            //loggain = new login();
            loggain.ShowDialog();
            loggain.FormClosed += new FormClosedEventHandler(loggain_Closed);

            //Egenskaper f�r Form
            this.Size = new Size(1600, 900);
            this.Location = new Point(0, 0);
            this.StartPosition = FormStartPosition.Manual;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.DoubleBuffered = true;
            
            //Bakgrundbild
            Image bakgrundbild = Image.FromFile(@"C:\\Black Jack\bilder\blackjackbord.png");
            Bitmap nystorlek = new Bitmap(bakgrundbild, 1440, 647);
            this.BackgroundImage = nystorlek;
            this.BackgroundImageLayout = ImageLayout.Stretch;
           
            //Slumpar vilken plats spelaren sitter p�.
            Random random = new Random();
            spelarinfo.spelarNummer = random.Next(1, spelarinfo.antalspelare + 1);


            l�ggtillInitialaRamar(); //L�gger till ramar och knappar, samt instansierar klassen Spelare f�r varje spelare
            l�ggTillMarkerBilder(); //L�gger till bilderna f�r de olika markerval�rerna
            tooltips(); //L�gger till tooltips f�r knapparna
            datorBet(); //Hanterar hur m�nga bet datorn g�r samt hur mycket per bet

        }

        private void loggain_Closed(object? sender, FormClosedEventArgs e) //Callar metoden nyOmg�ng som nollst�ller spelomg�ngen
        {
            nyOmg�ng();
        }

        private void Form_Close_1(object sender, FormClosedEventArgs e) //Uppdaterar filen s� att spelarens kontobalans uppdateras
        {
            uppdateraFil();
        }

        #region l�ggtillcontrols

        private void l�ggTillMarkerBilder() //L�gger till bilderna f�r de olika markerval�rerna 
        {
            dollar5.Location = new Point(this.Width / 2 - 244, this.Height / 2 + 225);
            dollar5.Size = new Size(80, 80);
            dollar5.BackColor = Color.Transparent;
            dollar5.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\5$.png");
            dollar5.BringToFront();
            this.Controls.Add(dollar5);
            dollar5.MouseClick += new MouseEventHandler(dollar5_click);

            dollar10.Location = new Point(this.Width / 2 - 174, this.Height / 2 + 235);
            dollar10.Size = new Size(80, 80);
            dollar10.BackColor = Color.Transparent;
            dollar10.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\10$.png");
            dollar10.BringToFront();
            this.Controls.Add(dollar10);
            dollar10.MouseClick += new MouseEventHandler(dollar10_click);

            dollar50.Location = new Point(this.Width / 2 - 104, this.Height / 2 + 245);
            dollar50.Size = new Size(80, 80);
            dollar50.BackColor = Color.Transparent;
            dollar50.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\50$.png");
            dollar50.BringToFront();
            this.Controls.Add(dollar50);
            dollar50.MouseClick += new MouseEventHandler(dollar50_click);

            dollar100.Location = new Point(this.Width / 2 - 34, this.Height / 2 + 245);
            dollar100.Size = new Size(80, 80);
            dollar100.BackColor = Color.Transparent;
            dollar100.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\100$.png");
            dollar100.BringToFront();
            this.Controls.Add(dollar100);
            dollar100.MouseClick += new MouseEventHandler(dollar100_click);

            dollar500.Location = new Point(this.Width / 2 + 44, this.Height / 2 + 235);
            dollar500.Size = new Size(80, 80);
            dollar500.BackColor = Color.Transparent;
            dollar500.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\500$.png");
            dollar500.BringToFront();
            this.Controls.Add(dollar500);
            dollar500.MouseClick += new MouseEventHandler(dollar500_click);

            dollar1000.Location = new Point(this.Width / 2 + 114, this.Height / 2 + 225);
            dollar1000.Size = new Size(80, 80);
            dollar1000.BackColor = Color.Transparent;
            dollar1000.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\1000$.png");
            dollar1000.BringToFront();
            this.Controls.Add(dollar1000);
            dollar1000.MouseClick += new MouseEventHandler(dollar1000_click);
        } 

        private void l�ggTillKnapp(int posX, int posY) //L�gger till knappar p� spelarens position
        {
            knappHit = new System.Windows.Forms.Button();
            knappHit.Size = new Size(30, 30);
            knappHit.Location = new Point(posX, posY);
            knappHit.FlatStyle = FlatStyle.Flat;
            knappHit.FlatAppearance.BorderSize = 0;
            knappHit.BackColor = Color.Transparent;
            knappHit.BackgroundImage = Image.FromFile(@"C:\\Black Jack\bilder\Knappar\hit.png");
            knappHit.BackgroundImageLayout = ImageLayout.Stretch;
            knappHit.FlatAppearance.MouseOverBackColor = Color.Transparent;
            knappHit.FlatAppearance.MouseDownBackColor = Color.Transparent;
            knappHit.BringToFront();
            this.Controls.Add(knappHit);
            knappHit.MouseClick += new MouseEventHandler(knappHit_click);

            knappPass = new System.Windows.Forms.Button();
            knappPass.Size = new Size(30, 30);
            knappPass.Location = new Point(posX, posY+35);
            knappPass.FlatStyle = FlatStyle.Flat;
            knappPass.FlatAppearance.BorderSize = 0;
            knappPass.BackColor = Color.Transparent;
            knappPass.BackgroundImage = Image.FromFile(@"C:\\Black Jack\bilder\Knappar\pass.png");
            knappPass.BackgroundImageLayout = ImageLayout.Stretch;
            knappPass.FlatAppearance.MouseOverBackColor = Color.Transparent;
            knappPass.FlatAppearance.MouseDownBackColor = Color.Transparent;
            knappPass.BringToFront();
            this.Controls.Add(knappPass);
            knappPass.MouseClick += new MouseEventHandler(knappPass_click);
            knappPass.Hide();

            knappDouble = new System.Windows.Forms.Button();
            knappDouble.Size = new Size(30, 30);
            knappDouble.Location = new Point(posX+145, posY);
            knappDouble.FlatStyle = FlatStyle.Flat;
            knappDouble.FlatAppearance.BorderSize = 0;
            knappDouble.BackColor = Color.Transparent;
            knappDouble.BackgroundImage = Image.FromFile(@"C:\\Black Jack\bilder\Knappar\double.png");
            knappDouble.BackgroundImageLayout = ImageLayout.Stretch;
            knappDouble.FlatAppearance.MouseOverBackColor = Color.Transparent;
            knappDouble.FlatAppearance.MouseDownBackColor = Color.Transparent;
            knappDouble.BringToFront();
            this.Controls.Add(knappDouble);
            knappDouble.MouseClick += new MouseEventHandler(knappDouble_click);
            knappDouble.Hide();

            knappSplit = new System.Windows.Forms.Button();
            knappSplit.Size = new Size(30, 30);
            knappSplit.Location = new Point(posX+145, posY+35);
            knappSplit.FlatStyle = FlatStyle.Flat;
            knappSplit.FlatAppearance.BorderSize = 0;
            knappSplit.BackColor = Color.Transparent;
            knappSplit.BackgroundImage = Image.FromFile(@"C:\\Black Jack\bilder\Knappar\split.png");
            knappSplit.BackgroundImageLayout = ImageLayout.Stretch;
            knappSplit.FlatAppearance.MouseOverBackColor = Color.Transparent;
            knappSplit.FlatAppearance.MouseDownBackColor = Color.Transparent;
            knappSplit.BringToFront();
            this.Controls.Add(knappSplit);
            knappSplit.MouseClick += new MouseEventHandler(knappSplit_click);
            knappSplit.Hide();
        }

        private void l�ggtillInitialaRamar() //L�gger till ramar och knappar, samt instansierar klassen Spelare f�r varje spelare, samt banken
        {
            int datornummer = 0; //Hanterar vilket nummer datorpspelaren f�r.  
            int posXskillnad = 0; //Hanterar hur mycket till h�ger varje control ska ligga


            for (int i = 0; i < spelarinfo.antalspelare+1; i++) //Loopar antalet spelare + banken
            {
                if (i == 0) //Banken �r alltid f�rst
                {
                    spelarlista.Add(spelare = new Spelare(i)); //L�gger till nytt ojbekt av klassen Spelare, i spelarlista
                    spelarlista[i].l�ggtillSpelh�g(); //L�gger till nytt ojbekt av klassen spelh�g, i klassen Spelare. 
                    spelarlista[i].spelh�g[0].l�ggtillKortsumma(kortPosX[i] + 15, kortPosY[i] - 20); //L�gger till label, som visar hur mycket korten �r v�rda tillsammans
                    this.Controls.Add(spelarlista[i].spelh�g[0].kortsumma); //L�gger till labeln till formen
                }
                else //Spelaren samt datorspelarna
                {
                    if (i != spelarinfo.spelarNummer) datornummer++; //Hoppar �ver att inkrementera om det �r spelaren som skapas
                    spelarlista.Add(spelare = new Spelare(i)); //L�gger till nytt ojbekt av klassen Spelare, i spelarlista
                    spelarlista[i].l�ggtillSpelarinfo(i, datornummer, kortPosX[i], kortPosY[i] + 117); //L�gger till label, som visar vem spelaren �r, samt kontoalansen
                    this.Controls.Add(spelarlista[i].spelarinfolabel); //L�gger till labeln till formen

                    for (int j = 0; j < 2; j++) //Skapar tv� instanser av spelh�g
                    {
                        spelarlista[i].l�ggtillSpelh�g(); //L�gger till nytt ojbekt av klassen spelh�g, i klassen Spelare. 
                        spelarlista[i].l�ggtillBetinfo(j, kortPosX[i] + posXskillnad, kortPosY[i] + 97); //L�gger till label, som visar hur mycket som satsats
                        spelarlista[i].l�ggtillKortSumma(j, kortPosX[i] + 15 + posXskillnad, kortPosY[i] + 77); //L�gger till label, som visar hur mycket korten �r v�rda tillsammans
                        this.Controls.Add(spelarlista[i].spelh�g[j].betinfo); //L�gger till labeln till formen
                        this.Controls.Add(spelarlista[i].spelh�g[j].kortsumma); //L�gger till labeln till formen
                        
                        //Skapar Picturebox som inneh�ller ramarna man ser innan korten delas ut
                        PictureBox p = new PictureBox(); 
                        p.Size = new Size(50, 72);
                        p.Location = new Point(kortPosX[i] + posXskillnad, kortPosY[i]);
                        p.BackColor = Color.Transparent;
                        p.SizeMode = PictureBoxSizeMode.StretchImage;
                        if (i == spelarinfo.spelarNummer) p.Click += new EventHandler(ram_click); //L�gger till Clickevent f�r spelaren
                        if (i == spelarinfo.spelarNummer)
                        {
                            
                            if (j == 0)
                            {
                                l�ggTillKnapp(p.Location.X - 35, p.Location.Y); //L�gger till knappar f�r spelaren
                                p.Tag = true;
                                DrawFilledRoundedRectangle(p, gr�npensel); //Fyller rektangeln gr�n, det �r den aktive korth�gen n�r spelaren satsar
                            }
                            else
                            {
                                p.Tag = false;
                                DrawFilledRoundedRectangle(p, r�dpensel); //Fyller rektangeln r�d, det �r den inaktive korth�gen n�r spelaren satsar
                            }
                        }
                        else
                        {
                            DrawTranslucentRoundedRectangle(p); //Ritar en tom rektangel f�r datorspelarna
                        }
                        posXskillnad += 55; //Flyttar controls till h�ger f�r andra korth�gen
                        
                        Random r = new Random(); 
                        int bet1eller2 = r.Next(1, 11); //Slumpar om datorna ska satsa en eller tv� g�nger
                        if (bet1eller2 < 4 && i != spelarinfo.spelarNummer) break; //Avbryter loopen f�r datorn om bet1eller2 �r mindre �n 4
                    }
                    posXskillnad = 0; //Nollst�ller
                }

            }

        }

        #endregion

        #region bettar
        private void harGjortBet()
        {
            if (vilkensatsat)
            {
                spelarlista[spelarinfo.spelarNummer].spelh�g[0].betinfo.Text = "$" + spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma.ToString();
                spelarlista[spelarinfo.spelarNummer].uppdateraSpelarinfo(spelarinfo.spelarNummer);
            }
            else
            {
                spelarlista[spelarinfo.spelarNummer].spelh�g[1].betinfo.Text = "$" + spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma.ToString();
                spelarlista[spelarinfo.spelarNummer].uppdateraSpelarinfo(spelarinfo.spelarNummer);
            }
        }

        private void datorBet()
        {
            for (int i = 1; i < spelarlista.Count; i++)
            {
                if (i != spelarinfo.spelarNummer)
                {
                    for (int j = 0; j < spelarlista[i].spelh�g.Count; j++) 
                    {
                        spelarlista[i].spelh�g[j].betsumma = betAI(spelarlista[i].datorbalans, i);
                        spelarlista[i].spelh�g[j].betinfo.Text = "$:" + spelarlista[i].spelh�g[j].betsumma.ToString();
                        spelarlista[i].uppdateraSpelarinfo(i);
                    }
                }
            }
        }

        private int betAI(int balans, int i)
        {
            double bet = 0;
            int bet1 = 0;
            double storlekBet = 0;
            Random r = new Random();
            storlekBet = r.Next(3,10);
            storlekBet = storlekBet / 100;
            bet = balans * storlekBet;
            if (balans < 50)
            {
                spelarlista[i].datorbalans -= balans;
                return balans;
            } else if (bet < 50)
            {
                spelarlista[i].datorbalans -= 50;
                return 50;
            } else
            {
                bet1 = Convert.ToInt32(bet);
                bet1 = (bet1 / 5) * 5;
                spelarlista[i].datorbalans -= bet1;
                return bet1;

            }
        }


        #endregion


        #region ritaochf�rgl�ggramar
        private void DrawTranslucentRoundedRectangle(PictureBox klickadRam)
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
            klickadRam.Image = bitmap;
            ramar.Add(klickadRam);
            this.Controls.Add(klickadRam);
        }

        private void DrawFilledRoundedRectangle(PictureBox klickadRam, SolidBrush pensel)
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
                //SolidBrush brush = new SolidBrush(Color.LightBlue); // Change the color as needed
                SolidBrush brush = pensel; // Change the color as needed

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
            //ram.Image = bitmap;
            klickadRam.Image = bitmap;
            ramar.Add(klickadRam);
            this.Controls.Add(klickadRam);
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



        #region datorAI
        private async Task datorAI(int x)
        {
            for (int i = x; i < spelarlista.Count; i++)
            {
                if (i == spelarinfo.spelarNummer)
                {
                    try
                    {
                        spelarlista[i].spelh�g[0].kortsumma.BackColor = Color.Yellow;
                    } catch { }
                    return;
                }
                int a = spelarlista[i].spelh�g.Count;
                for (int j = 0; j < a; j++)
                {
                    splitEllerDouble(i, j);
                    a = spelarlista[i].spelh�g.Count;
                    try
                    {
                        spelarlista[i].spelh�g[j].kortsumma.BackColor = Color.Yellow;
                    } catch { }

                    //if (spelarlista[i].spelh�g.Count > 1) spelarlista[i].spelh�g[j-1].kortsumma.BackColor = SystemColors.Control;
                    if (spelarlista[i].spelh�g[j].splitEllerDouble)
                    {
                        spelaLjud(@"C:\\Black Jack\Audio\kortspelas.wav");
                        try
                        {
                            spelarlista[i].spelh�g[j].kortsumma.BackColor = Color.Yellow;
                        } catch { }
                        await Task.Delay(100);
                        l�ggtillKort(i, j);
                        kollaVinnare(i, j);
                    }
                    else
                    {
                        while (HitOrPass(i, j)) 
                        {
                            spelaLjud(@"C:\\Black Jack\Audio\kortspelas.wav");
                            try
                            {
                                spelarlista[i].spelh�g[j].kortsumma.BackColor = Color.Yellow;
                            } catch { }
                            await Task.Delay(100);
                        }
                        kollaVinnare(i, j);
                    }
                    //if (j == spelarlista[i].spelh�g.Count-1) spelarlista[i].spelh�g[j].kortsumma.BackColor = SystemColors.Control;
                }
            }
            bankAi();
        }

        private async Task bankAi()
        {
            bool bank17orBust = false;
            while (!bank17orBust)
            {
                l�ggtillKort();
                spelaLjud(@"C:\\Black Jack\Audio\kortspelas.wav");
                await Task.Delay(100);
                if (Kortlek.ber�knaKortv�rde(spelarlista[0].spelh�g[0].kortv�rde) > 21)
                {
                    kollaVinnare(Kortlek.ber�knaKortv�rde(spelarlista[0].spelh�g[0].kortv�rde));
                    bank17orBust = true;
                }
                else if (Kortlek.ber�knaKortv�rde(spelarlista[0].spelh�g[0].kortv�rde) > 16 && Kortlek.ber�knaKortv�rde(spelarlista[0].spelh�g[0].kortv�rde) < 22)
                {
                    if (spelarlista[0].spelh�g[0].kortv�rde.Contains(11) && Kortlek.ber�knaKortv�rde(spelarlista[0].spelh�g[0].kortv�rde) == 17)
                    {
                    }
                    else
                    {
                        kollaVinnare(Kortlek.ber�knaKortv�rde(spelarlista[0].spelh�g[0].kortv�rde));
                        bank17orBust = true;
                    }
                }
            }
            p�g�endeRunda = !p�g�endeRunda;
        }

        private void l�ggtillKort() //L�gger till kort f�r banken
        {
            Random rnd = new Random();
            int draKort = rnd.Next(Kortlek.nykortlek.Count);
            //spelarlista[0].spelh�g[0].kortsumma.Location = new Point(spelarlista[i].spelh�g[j].kortsumma.Location.X, spelarlista[i].spelh�g[j].kortsumma.Location.Y - 15);
            spelarlista[0].l�ggtillKortochV�rde(Kortlek.nykortlek[draKort], 0, spelarlista[0].spelh�g[0].spelkort[spelarlista[0].spelh�g[0].spelkort.Count - 1].Location.X +15, spelarlista[0].spelh�g[0].spelkort[0].Location.Y);
            spelarlista[0].spelh�g[0].kortsumma.Text = Kortlek.ber�knaKortv�rde(spelarlista[0].spelh�g[0].kortv�rde).ToString();
            Kortlek.nykortlek.RemoveAt(draKort);
            this.Controls.Add(spelarlista[0].spelh�g[0].spelkort[spelarlista[0].spelh�g[0].spelkort.Count - 1]);
        }

        private void l�ggtillKort(int i, int j) //L�gger till kort f�r spelare
        {
            Random rnd = new Random();
            int draKort = rnd.Next(Kortlek.nykortlek.Count);           
            //spelarlista[i].spelh�g[j].kortsumma.Location = new Point(spelarlista[i].spelh�g[j].kortsumma.Location.X, spelarlista[i].spelh�g[j].kortsumma.Location.Y - 15);
            spelarlista[i].l�ggtillKortochV�rde(Kortlek.nykortlek[draKort], j, spelarlista[i].spelh�g[j].spelkort[spelarlista[i].spelh�g[j].spelkort.Count-1].Location.X, spelarlista[i].spelh�g[j].spelkort[spelarlista[i].spelh�g[j].spelkort.Count - 1].Location.Y-15);
            spelarlista[i].spelh�g[j].kortsumma.Text = Kortlek.ber�knaKortv�rde(spelarlista[i].spelh�g[j].kortv�rde).ToString();
            Kortlek.nykortlek.RemoveAt(draKort);
            this.Controls.Add(spelarlista[i].spelh�g[j].spelkort[spelarlista[i].spelh�g[j].spelkort.Count-1]);
        }

        private bool HitOrPass(int i, int j)
        {
            if (!spelarlista[i].spelh�g[j].kortv�rde.Contains(11))
            {
                switch(Kortlek.ber�knaKortv�rde(spelarlista[i].spelh�g[j].kortv�rde))
                {
                    case 4:
                        l�ggtillKort(i, j);
                        return true;
                    case 5:
                        l�ggtillKort(i, j);
                        return true;
                    case 6:
                        l�ggtillKort(i, j);
                        return true;
                    case 7:
                        l�ggtillKort(i, j);
                        return true;
                    case 8:
                        l�ggtillKort(i, j);
                        return true;
                    case 9:
                        l�ggtillKort(i, j);
                        return true;
                    case 10:
                        l�ggtillKort(i, j);
                        return true;
                    case 11:
                        l�ggtillKort(i, j);
                        return true;
                    case 12:
                        if (spelarlista[0].spelh�g[0].kortv�rde[0] < 4 || spelarlista[0].spelh�g[0].kortv�rde[0] > 6)
                        {
                            l�ggtillKort(i, j);
                            return true;
                        }
                        else return false;
                    case 13:
                        if (spelarlista[0].spelh�g[0].kortv�rde[0] > 6)
                        {
                            l�ggtillKort(i, j);
                            return true;
                        }
                        else return false;
                    case 14:
                        if (spelarlista[0].spelh�g[0].kortv�rde[0] > 6)
                        {
                            l�ggtillKort(i, j);
                            return true;
                        }
                        else return false;
                    case 15:
                        if (spelarlista[0].spelh�g[0].kortv�rde[0] > 6)
                        {
                            l�ggtillKort(i, j);
                            return true;
                        }
                        else return false;
                    case 16:
                        if (spelarlista[0].spelh�g[0].kortv�rde[0] > 6)
                        {
                            l�ggtillKort(i, j);
                            return true;
                        }
                        else return false;
                    default: return false;
                }
            } else
            {
                switch (Kortlek.ber�knaKortv�rde(spelarlista[i].spelh�g[j].kortv�rde))
                {
                    case 18:
                        if (spelarlista[0].spelh�g[0].kortv�rde[0] > 8)
                        {
                            l�ggtillKort(i, j);
                            return true;
                        }
                        else return false;
                    case 19: return false;
                    case 20: return false;
                    case 21: return false;
                    default:
                        if (Kortlek.ber�knaKortv�rde(spelarlista[i].spelh�g[j].kortv�rde) < 22)
                        {
                            l�ggtillKort(i, j);
                            return true;
                        }
                        else return false;
                }
            }
        }

        private void splitEllerDouble(int i, int j)
        {
            bool VellerH = true;
            if (j != 0) VellerH = !VellerH;

            if (!spelarlista[i].spelh�g[j].splitEllerDouble)
            {
                if (spelarlista[i].spelh�g[j].kortv�rde[0] == spelarlista[i].spelh�g[j].kortv�rde[1])
                {                        
                    switch (spelarlista[i].spelh�g[j].kortv�rde[0])
                    {
                        case 2:
                            if (spelarlista[0].spelh�g[0].kortv�rde[0] > 3 && spelarlista[0].spelh�g[0].kortv�rde[0] < 8)
                            {
                                split(i, VellerH);
                            }
                            break;
                        case 3:
                            if (spelarlista[0].spelh�g[0].kortv�rde[0] > 3 && spelarlista[0].spelh�g[0].kortv�rde[0] < 8)
                            {
                                split(i, VellerH);
                            }
                            break;
                        case 6:
                            if (spelarlista[0].spelh�g[0].kortv�rde[0] > 2 && spelarlista[0].spelh�g[0].kortv�rde[0] < 7)
                            {
                                split(i, VellerH);
                            }
                            break;
                        case 7:
                            if (spelarlista[0].spelh�g[0].kortv�rde[0] < 8)
                            {
                                split(i, VellerH);
                            }
                            break;
                        case 8:
                            split(i, VellerH);
                            break;
                        case 9:
                            if (spelarlista[0].spelh�g[0].kortv�rde[0] != 7 || spelarlista[0].spelh�g[0].kortv�rde[0] != 10 || spelarlista[0].spelh�g[0].kortv�rde[0] != 11)
                            {
                                split(i, VellerH);
                            }
                            break;
                        case 11:
                            split(i, VellerH);
                            break;
                    }
                }
                else
                {
                    if (!spelarlista[i].spelh�g[j].kortv�rde.Contains(11))
                    {
                        if (Kortlek.ber�knaKortv�rde(spelarlista[i].spelh�g[j].kortv�rde) == 11)
                        {
                            dubbla(i, VellerH);
                        } else if (Kortlek.ber�knaKortv�rde(spelarlista[i].spelh�g[j].kortv�rde) == 10 && (spelarlista[0].spelh�g[0].kortv�rde[0] != 10 || spelarlista[0].spelh�g[0].kortv�rde[0] != 11))
                        {
                            dubbla(i, VellerH);
                        } else if (Kortlek.ber�knaKortv�rde(spelarlista[i].spelh�g[j].kortv�rde) == 9 && spelarlista[0].spelh�g[0].kortv�rde[0] > 2 && spelarlista[0].spelh�g[0].kortv�rde[0] < 7)
                        {
                            dubbla(i, VellerH);
                        }
                    } 
                    else
                    {
                        if (Kortlek.ber�knaKortv�rde(spelarlista[i].spelh�g[j].kortv�rde) == 19 && spelarlista[0].spelh�g[0].kortv�rde[0] == 6)
                        {
                            dubbla(i, VellerH);
                        } else if (Kortlek.ber�knaKortv�rde(spelarlista[i].spelh�g[j].kortv�rde) == 18 && spelarlista[0].spelh�g[0].kortv�rde[0] < 6)
                        {
                            dubbla(i, VellerH);
                        } else if (Kortlek.ber�knaKortv�rde(spelarlista[i].spelh�g[j].kortv�rde) == 17 && spelarlista[0].spelh�g[0].kortv�rde[0] > 2 && spelarlista[0].spelh�g[0].kortv�rde[0] < 7)
                        {
                            dubbla(i, VellerH);
                        } else if (Kortlek.ber�knaKortv�rde(spelarlista[i].spelh�g[j].kortv�rde) == 16 && spelarlista[0].spelh�g[0].kortv�rde[0] > 3 && spelarlista[0].spelh�g[0].kortv�rde[0] < 7)
                        {
                            dubbla(i, VellerH);
                        } else if (Kortlek.ber�knaKortv�rde(spelarlista[i].spelh�g[j].kortv�rde) == 15 && spelarlista[0].spelh�g[0].kortv�rde[0] > 3 && spelarlista[0].spelh�g[0].kortv�rde[0] < 7)
                        {
                            dubbla(i, VellerH);
                        } else if (Kortlek.ber�knaKortv�rde(spelarlista[i].spelh�g[j].kortv�rde) == 14 && spelarlista[0].spelh�g[0].kortv�rde[0] > 4 && spelarlista[0].spelh�g[0].kortv�rde[0] < 7)
                        {
                            dubbla(i, VellerH);
                        }
                    }
                }
            }
            
        }
        #endregion

        #region spellogik

        private async Task spelaKort()
        {
            foreach (PictureBox p in ramar)
            {
                Controls.Remove(p);
                p.Dispose();
            }
            Random rnd = new Random();
            int draKort;
            spelaLjud(@"C:\\Black Jack\Audio\lekblandas.wav");
            await Task.Delay(500);
            Kortlek.skapaNykortlek();
            for (int i = 0; i < (spelarinfo.antalspelare + 1); i++)
            {
                if (i == 0)
                {
                    draKort = rnd.Next(Kortlek.nykortlek.Count);
                    spelarlista[i].l�ggtillKortochV�rde(Kortlek.nykortlek[draKort], i, kortPosX[i], kortPosY[i]);
                    spelarlista[i].spelh�g[0].kortsumma.Text = spelarlista[i].spelh�g[0].kortv�rde.Sum().ToString();
                    Kortlek.nykortlek.RemoveAt(draKort);
                }
                else
                {
                    //spelaLjud(@"C:\\Black Jack\Audio\kortspelas.wav");
                    int posXskillnad = 0;
                    for (int j = 0; j < spelarlista[i].spelh�g.Count; j++)
                    {
                        int posYskillnad = 0;
                        for (int k = 0; k < 2; k++)
                        {
                            draKort = rnd.Next(Kortlek.nykortlek.Count);                                  
                            //spelarlista[i].spelh�g[j].kortsumma.Location = new Point(spelarlista[i].spelh�g[j].kortsumma.Location.X, spelarlista[i].spelh�g[j].kortsumma.Location.Y - 15);
                            spelarlista[i].l�ggtillKortochV�rde(Kortlek.nykortlek[draKort], j, kortPosX[i] + posXskillnad, kortPosY[i] - posYskillnad);
                            Kortlek.nykortlek.RemoveAt(draKort);
                            posYskillnad += 15;
                        }
                            
                        posXskillnad += 55;
                    }
                }
            }
            knappDouble.Show();
            knappPass.Show();
            for (int i = 0; i < spelarlista[spelarinfo.spelarNummer].spelh�g.Count; i++)
            {
                if (spelarlista[spelarinfo.spelarNummer].spelh�g[i].kortv�rde[0] == spelarlista[spelarinfo.spelarNummer].spelh�g[i].kortv�rde[1]) knappSplit.Show();
            }
            vilkensatsat = true;
            visaSpelkort();
            knappDouble.Show();
            knappPass.Show();
            p�g�endeRunda = true;

        }

        private bool split(int i, bool VellerH)
        {
            int a = 0;
            if (!VellerH)
            {
                if (spelarlista[i].spelh�g.Count == 2) a = 1;
                else a = 2;
            }
            if (i == spelarinfo.spelarNummer)
            {
                if (spelarlista[i].spelh�g[a].betsumma * 2 > spelarinfo.kontobalans)
                {
                    MessageBox.Show("Du har inte marker nog f�r att splitta", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false; ;
                }
                else spelarinfo.kontobalans -= spelarlista[i].spelh�g[a].betsumma;
            }
            else
            {
                if (spelarlista[i].spelh�g[a].betsumma * 2 > spelarlista[i].datorbalans) return false;
                else spelarlista[i].datorbalans -= spelarlista[i].spelh�g[a].betsumma;
            }

            if (VellerH)
            {
                spelarlista[i].spelh�g[a].spelkort[0].Location = new Point(spelarlista[i].spelh�g[a].spelkort[0].Location.X, spelarlista[i].spelh�g[a].spelkort[0].Location.Y - 77);
                spelarlista[i].spelh�g[a].spelkort[1].Location = new Point(spelarlista[i].spelh�g[a].spelkort[0].Location.X - 55, spelarlista[i].spelh�g[a].spelkort[0].Location.Y);
                spelarlista[i].spelh�g[a].betinfo.Location = new Point(spelarlista[i].spelh�g[a].betinfo.Location.X, spelarlista[i].spelh�g[a].betinfo.Location.Y - 77);
                spelarlista[i].spelh�g[a].kortsumma.Location = new Point(spelarlista[i].spelh�g[a].kortsumma.Location.X, spelarlista[i].spelh�g[a].kortsumma.Location.Y - 77);


                spelarlista[i].insertSpelh�g();
                spelarlista[i].spelh�g[a].l�ggtillBetinfo(spelarlista[i].spelh�g[a + 1].betinfo.Location.X - 55, spelarlista[i].spelh�g[a + 1].betinfo.Location.Y);
                spelarlista[i].spelh�g[a].l�ggtillKortsumma(spelarlista[i].spelh�g[a + 1].kortsumma.Location.X - 55, spelarlista[i].spelh�g[a + 1].kortsumma.Location.Y);
                spelarlista[i].spelh�g[a].spelkort.Add(spelarlista[i].spelh�g[a + 1].spelkort[1]);
                spelarlista[i].spelh�g[a].kortv�rde.Add(spelarlista[i].spelh�g[a + 1].kortv�rde[1]);
                spelarlista[i].spelh�g[a].betsumma = spelarlista[i].spelh�g[a + 1].betsumma;
                spelarlista[i].spelh�g[a].betinfo.Text = "$" + spelarlista[i].spelh�g[a].betsumma;
                spelarlista[i].spelh�g[a].kortsumma.Text = Kortlek.ber�knaKortv�rde(spelarlista[i].spelh�g[a].kortv�rde).ToString();
                spelarlista[i].spelh�g[a + 1].spelkort.RemoveAt(1);
                spelarlista[i].spelh�g[a + 1].kortv�rde.RemoveAt(1);
                spelarlista[i].spelh�g[a + 1].kortsumma.Text = Kortlek.ber�knaKortv�rde(spelarlista[i].spelh�g[a + 1].kortv�rde).ToString();
                spelarlista[i].spelh�g[a].splitEllerDouble = true;
                spelarlista[i].spelh�g[a + 1].splitEllerDouble = true;
                this.Controls.Add(spelarlista[i].spelh�g[a].betinfo);
                this.Controls.Add(spelarlista[i].spelh�g[a].kortsumma);

                if (i == spelarinfo.spelarNummer)
                {
                    knappHit.Location = new Point(knappHit.Location.X, knappHit.Location.Y + 30);
                    knappPass.Location = new Point(knappPass.Location.X, knappPass.Location.Y + 30);
                }
                return true;
            }
            else
            {
                spelarlista[i].spelh�g[a].spelkort[0].Location = new Point(spelarlista[i].spelh�g[a].spelkort[0].Location.X, spelarlista[i].spelh�g[a].spelkort[0].Location.Y - 77);
                spelarlista[i].spelh�g[a].spelkort[1].Location = new Point(spelarlista[i].spelh�g[a].spelkort[0].Location.X + 55, spelarlista[i].spelh�g[a].spelkort[0].Location.Y);
                spelarlista[i].spelh�g[a].betinfo.Location = new Point(spelarlista[i].spelh�g[a].betinfo.Location.X, spelarlista[i].spelh�g[a].betinfo.Location.Y - 77);
                spelarlista[i].spelh�g[a].kortsumma.Location = new Point(spelarlista[i].spelh�g[a].kortsumma.Location.X, spelarlista[i].spelh�g[a].kortsumma.Location.Y - 77);

                spelarlista[i].l�ggtillSpelh�g();
                spelarlista[i].spelh�g[a + 1].l�ggtillBetinfo(spelarlista[i].spelh�g[a].betinfo.Location.X + 55, spelarlista[i].spelh�g[a].betinfo.Location.Y);
                spelarlista[i].spelh�g[a + 1].l�ggtillKortsumma(spelarlista[i].spelh�g[a].kortsumma.Location.X + 55, spelarlista[i].spelh�g[a].kortsumma.Location.Y);
                spelarlista[i].spelh�g[a + 1].spelkort.Add(spelarlista[i].spelh�g[a].spelkort[1]);
                spelarlista[i].spelh�g[a + 1].kortv�rde.Add(spelarlista[i].spelh�g[a].kortv�rde[1]);
                spelarlista[i].spelh�g[a + 1].betsumma = spelarlista[i].spelh�g[a].betsumma;
                spelarlista[i].spelh�g[a + 1].betinfo.Text = "$" + spelarlista[i].spelh�g[a + 1].betsumma;
                spelarlista[i].spelh�g[a + 1].kortsumma.Text = Kortlek.ber�knaKortv�rde(spelarlista[i].spelh�g[a + 1].kortv�rde).ToString();
                spelarlista[i].spelh�g[a].spelkort.RemoveAt(1);
                spelarlista[i].spelh�g[a].kortv�rde.RemoveAt(1);
                spelarlista[i].spelh�g[a].kortsumma.Text = Kortlek.ber�knaKortv�rde(spelarlista[i].spelh�g[a].kortv�rde).ToString();
                spelarlista[i].spelh�g[a].splitEllerDouble = true;
                spelarlista[i].spelh�g[a + 1].splitEllerDouble = true;
                this.Controls.Add(spelarlista[i].spelh�g[a + 1].betinfo);
                this.Controls.Add(spelarlista[i].spelh�g[a + 1].kortsumma);

                if (i == spelarinfo.spelarNummer)
                {
                    knappDouble.Location = new Point(knappDouble.Location.X, knappDouble.Location.Y + 30);
                    knappSplit.Location = new Point(knappSplit.Location.X, knappSplit.Location.Y + 30);
                }
                return true;
            }            
        }

        private bool dubbla(int i, bool VellerH)
        {
            int a = 0;
            if (!VellerH)
            {
                if (spelarlista[i].spelh�g.Count == 2) a = 1;
                else a = 2;
            }

            if (i == spelarinfo.spelarNummer)
            {
                if (spelarlista[spelarinfo.spelarNummer].spelh�g[a].betsumma * 2 < spelarinfo.kontobalans)
                {
                    spelarinfo.kontobalans -= spelarlista[spelarinfo.spelarNummer].spelh�g[a].betsumma;
                    spelarlista[spelarinfo.spelarNummer].spelh�g[a].betsumma *= 2;
                    spelarlista[spelarinfo.spelarNummer].spelh�g[a].betinfo.Text = "$" + spelarlista[spelarinfo.spelarNummer].spelh�g[a].betsumma.ToString();
                    spelarlista[spelarinfo.spelarNummer].uppdateraSpelarinfo(spelarinfo.spelarNummer);
                    spelarlista[spelarinfo.spelarNummer].spelh�g[a].splitEllerDouble = true;
                    return true;
                }
                else
                {
                    MessageBox.Show("Du har inte marker nog f�r att dubbla insatsen", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            } else
            {
                if (spelarlista[i].spelh�g[a].betsumma * 2 < spelarlista[i].datorbalans)
                {
                    spelarlista[i].datorbalans -= spelarlista[i].spelh�g[a].betsumma;
                    spelarlista[i].spelh�g[a].betsumma *= 2;
                    spelarlista[i].spelh�g[a].betinfo.Text = "$" + spelarlista[i].spelh�g[a].betsumma.ToString();
                    spelarlista[i].uppdateraSpelarinfo(i);
                    spelarlista[i].spelh�g[a].splitEllerDouble = true;
                    return true;
                } else return false;
            }
        }

        private void kollaVinnare(int bankenskortv�rde) //Kollar allas kort efter att banken har dragit klart
        {
            for (int i = 1; i < spelarlista.Count; i++)
            {
                for (int j = 0; j < spelarlista[i].spelh�g.Count; j++)
                {
                    if (!spelarlista[i].spelh�g[j].kollatVinst)
                    {
                        if (bankenskortv�rde > 21)
                        {

                            if (i == spelarinfo.spelarNummer)
                            {
                                spelarlista[i].spelh�g[j].betinfo.ForeColor = Color.White;
                                spelarlista[i].spelh�g[j].betinfo.BackColor = Color.DarkGreen;
                                spelarlista[i].spelh�g[j].betinfo.Text.Insert(0, "+");
                                spelarinfo.kontobalans += spelarlista[i].spelh�g[j].betsumma;
                                spelarlista[i].spelarinfolabel.Text = spelarlista[i].spelarinfotext + spelarinfo.kontobalans;
                            }
                            else
                            {
                                spelarlista[i].spelh�g[j].betinfo.ForeColor = Color.White;
                                spelarlista[i].spelh�g[j].betinfo.BackColor = Color.DarkGreen;
                                spelarlista[i].spelh�g[j].betinfo.Text.Insert(0, "+");
                                spelarlista[i].datorbalans += spelarlista[i].spelh�g[j].betsumma;
                                spelarlista[i].spelarinfolabel.Text = spelarlista[i].spelarinfotext + spelarlista[i].datorbalans;
                            }
                        } else
                        {
                            if (i == spelarinfo.spelarNummer)
                            {
                                if (bankenskortv�rde >= Kortlek.ber�knaKortv�rde(spelarlista[i].spelh�g[j].kortv�rde)) 
                                {
                                    spelarlista[i].spelh�g[j].betinfo.ForeColor = Color.White;
                                    spelarlista[i].spelh�g[j].betinfo.BackColor = Color.DarkRed;
                                    spelarlista[i].spelh�g[j].betinfo.Text.Insert(0, "-");
                                } else
                                {
                                    spelarlista[i].spelh�g[j].betinfo.ForeColor = Color.White;
                                    spelarlista[i].spelh�g[j].betinfo.BackColor = Color.DarkGreen;
                                    spelarlista[i].spelh�g[j].betinfo.Text.Insert(0, "+");
                                }
                                spelarinfo.kontobalans += spelarlista[i].spelh�g[j].betsumma;
                                spelarlista[i].spelarinfolabel.Text = spelarlista[i].spelarinfotext + spelarinfo.kontobalans;
                            }
                            else
                            {
                                if (bankenskortv�rde >= Kortlek.ber�knaKortv�rde(spelarlista[i].spelh�g[j].kortv�rde))
                                {
                                    spelarlista[i].spelh�g[j].betinfo.ForeColor = Color.White;
                                    spelarlista[i].spelh�g[j].betinfo.BackColor = Color.DarkRed;
                                    spelarlista[i].spelh�g[j].betinfo.Text.Insert(0, "-");
                                }
                                else
                                {
                                    spelarlista[i].spelh�g[j].betinfo.ForeColor = Color.White;
                                    spelarlista[i].spelh�g[j].betinfo.BackColor = Color.DarkGreen;
                                    spelarlista[i].spelh�g[j].betinfo.Text.Insert(0, "+");
                                }
                                spelarlista[i].datorbalans += spelarlista[i].spelh�g[j].betsumma;
                                spelarlista[i].spelarinfolabel.Text = spelarlista[i].spelarinfotext + spelarlista[i].datorbalans;
                            }
                        }
                    }
                }
            }

        }

        private bool kollaVinnare(int i, int j) //Kollar om spelaren blir bust, efter att han dragit ett kort
        {
            if (Kortlek.ber�knaKortv�rde(spelarlista[i].spelh�g[j].kortv�rde) > 21)
            {
                spelarlista[i].spelh�g[j].betinfo.ForeColor = Color.White;
                spelarlista[i].spelh�g[j].betinfo.BackColor = Color.DarkRed;
                spelarlista[i].spelh�g[j].betinfo.Text = spelarlista[i].spelh�g[j].betinfo.Text.Insert(0, "-");
                spelarlista[i].spelh�g[j].kollatVinst = true;
                if (i == spelarinfo.spelarNummer)
                {
                    spelarinfo.kontobalans -= spelarlista[i].spelh�g[j].betsumma;
                    spelarlista[i].spelarinfolabel.Text = "$" + spelarinfo.kontobalans;
                    harDelat2 = false;
                    vilkensatsat = true;
                    return true;
                }
                else
                {
                    spelarlista[i].datorbalans -= spelarlista[i].spelh�g[j].betsumma;
                    spelarlista[i].spelarinfolabel.Text = "$" + spelarlista[i].datorbalans;
                    return true;
                }
            }
            else return false;          
        }

        private void maxBet()
        {

            for (int i = 0; i < kollaKryss.Length; i++)
            {
                kollaKryss[i] = false;
            }
            if (vilkensatsat)
            {
                if (spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma > 0)
                {
                    kollaKryss[5] = true;
                    dollar1000.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\1000$kryss.png");
                }
                else dollar1000.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\1000$.png");

                if (spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma > 500)
                {
                    kollaKryss[4] = true;
                    dollar500.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\500$kryss.png");
                }
                else dollar500.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\500$.png");

                if (spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma > 900)
                {
                    kollaKryss[3] = true;
                    dollar100.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\100$kryss.png");
                }
                else dollar100.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\100$.png");

                if (spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma > 950)
                {
                    kollaKryss[2] = true;
                    dollar50.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\50$kryss.png");
                }
                else dollar50.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\50$.png");

                if (spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma > 990)
                {
                    kollaKryss[1] = true;
                    dollar10.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\10$kryss.png");
                }
                else dollar10.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\10$.png");

                if (spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma > 995)
                {
                    kollaKryss[0] = true;
                    dollar5.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\5$kryss.png");
                }
                else dollar5.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\5$.png");
            }
            else
            {
                if (spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma > 0)
                {
                    kollaKryss[5] = true;
                    dollar1000.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\1000$kryss.png");
                }
                else dollar1000.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\1000$.png");

                if (spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma > 500)
                {
                    kollaKryss[4] = true;
                    dollar500.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\500$kryss.png");
                }
                else dollar500.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\500$.png");

                if (spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma > 900)
                {
                    kollaKryss[3] = true;
                    dollar100.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\100$kryss.png");
                }
                else dollar100.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\100$.png");

                if (spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma > 950)
                {
                    kollaKryss[2] = true;
                    dollar50.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\50$kryss.png");
                }
                else dollar50.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\50$.png");

                if (spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma > 990)
                {
                    kollaKryss[1] = true;
                    dollar10.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\10$kryss.png");
                }
                else dollar10.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\10$.png");

                if (spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma > 995)
                {
                    kollaKryss[0] = true;
                    dollar5.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\5$kryss.png");
                }
                else dollar5.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\5$.png");
            }
        }
        private void nyOmg�ng()
        {
            //foreach (PictureBox p in ramar)
            //{
            //    Controls.Remove(p);
            //    p.Dispose();
            //}

            try
            {
                foreach (System.Windows.Forms.Button b in this.Controls)
                {
                    Controls.Remove(b);
                    b.Dispose();
                }
            } catch { }

            for (int i = 0; i < spelarlista.Count; i++)
            {
                Controls.Remove(spelarlista[i].spelarinfolabel);
                spelarlista[i].spelarinfolabel.Dispose();
                for (int j = 0; j < spelarlista[i].spelh�g.Count; j++) 
                {
                    foreach (PictureBox p in spelarlista[i].spelh�g[j].spelkort)
                    {
                        Controls.Remove(p);
                        p.Dispose();
                    }

                    Controls.Remove(spelarlista[i].spelh�g[j].betinfo);
                    spelarlista[i].spelh�g[j].betinfo.Dispose();
                    Controls.Remove(spelarlista[i].spelh�g[j].kortsumma);
                    spelarlista[i].spelh�g[j].kortsumma.Dispose();
                }
            }

            spelarlista.Clear();
            vilkensatsat = true;
            harDelat2 = false;
            vilkenSpelh�g = 0;
            //p�g�endeRunda = false;
            l�ggtillInitialaRamar();
            //l�ggTillMarkerBilder();
            tooltips();
            datorBet();
        }

        #endregion



        #region l�ggtillbilder

        private async Task visaSpelkort()
        {
            foreach (PictureBox p in ramar)
            {
                Controls.Remove(p);
                p.Dispose();
            }

            for (int i = 0; i < spelarlista.Count; i++)
            {
                for (int j = 0; j < spelarlista[i].spelh�g.Count; j++)
                {
                    for (int k = 0; k < spelarlista[i].spelh�g[j].spelkort.Count; k++)
                    {
                        this.Controls.Add(spelarlista[i].spelh�g[j].spelkort[k]);
                        if (k == 0) spelarlista[i].spelh�g[j].kortsumma.Text = spelarlista[i].spelh�g[j].kortv�rde[0].ToString();
                        else spelarlista[i].spelh�g[j].kortsumma.Text = Kortlek.ber�knaKortv�rde(spelarlista[i].spelh�g[j].kortv�rde).ToString();
                        spelaLjud(@"C:\\Black Jack\Audio\kortspelas.wav");
                        await Task.Delay(75);
                    }
                }
            }

            await Task.Delay(100);
            datorAI(1);
        }

        #endregion

        private void g�mKnappar()
        {
            knappDouble.Hide();
            knappPass.Hide();
            knappHit.Hide();
            knappSplit.Hide();
        }

        #region Mouseclick
        private void dollar5_click(object? sender, MouseEventArgs e)
        {
            if (!p�g�endeRunda)
            {
                if (f�rstaRunda)
                {
                    p�g�endeRunda = !p�g�endeRunda;
                    f�rstaRunda = !f�rstaRunda;
                }
                else
                {
                    nyOmg�ng();
                    p�g�endeRunda = !p�g�endeRunda;
                }
            }

            if (e.Button == MouseButtons.Right)
            {
                if (vilkensatsat)
                {
                    if (spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma != 0)
                    {
                        spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma -= 5;
                        spelarinfo.kontobalans += 5;
                        harGjortBet();
                    }
                    else
                    {
                    }
                }
                else
                {
                    if (spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma != 0)
                    {
                        spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma -= 5;
                        spelarinfo.kontobalans += 5;
                        harGjortBet();
                    }
                    else
                    {
                    }
                }
            }
            else
            {
                if (!kollaKryss[0])
                {
                    if (vilkensatsat)
                    {
                        if (spelarinfo.kontobalans > 5)
                        {
                            spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma += 5;
                            spelarinfo.kontobalans -= 5;
                            harGjortBet();
                        }
                        else
                        {
                            MessageBox.Show("Du har inte tillr�ckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        if (spelarinfo.kontobalans > 5)
                        {
                            spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma += 5;
                            spelarinfo.kontobalans -= 5;
                            harGjortBet();
                        }
                        else
                        {
                            MessageBox.Show("Du har inte tillr�ckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }
            }
            
            maxBet();
            spelaLjudUtanSync(@"C:\\Black Jack\Audio\satsamarker.wav");
        }
        private void dollar10_click(object? sender, MouseEventArgs e)
        {
            if (!p�g�endeRunda)
            {
                if (f�rstaRunda)
                {
                    p�g�endeRunda = !p�g�endeRunda;
                    f�rstaRunda = !f�rstaRunda;
                }
                else
                {
                    nyOmg�ng();
                    p�g�endeRunda = !p�g�endeRunda;
                }
            }

            if (e.Button == MouseButtons.Right)
            {
                if (vilkensatsat)
                {
                    if (10 > spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma)
                    {

                        spelarinfo.kontobalans += spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma;
                        spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma = 0;
                        harGjortBet();
                    }
                    else
                    {
                        spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma -= 10;
                        spelarinfo.kontobalans += 10;
                        harGjortBet();
                    }
                }
                else
                {
                    if (10 > spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma)
                    {
                        spelarinfo.kontobalans += spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma;
                        spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma = 0;
                        harGjortBet();

                    }
                    else
                    {
                        spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma -= 10;
                        spelarinfo.kontobalans += 10;
                        harGjortBet();
                    }
                }
            }
            else
            {
                if (!kollaKryss[1])
                {
                    if (vilkensatsat)
                    {
                        if (spelarinfo.kontobalans >= 10)
                        {
                            spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma += 10;
                            spelarinfo.kontobalans -= 10;
                            harGjortBet();
                        }
                        else
                        {
                            MessageBox.Show("Du har inte tillr�ckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        if (spelarinfo.kontobalans >= 10)
                        {
                            spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma += 10;
                            spelarinfo.kontobalans -= 10;
                            harGjortBet();
                        }
                        else
                        {
                            MessageBox.Show("Du har inte tillr�ckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            
            maxBet();
            spelaLjudUtanSync(@"C:\\Black Jack\Audio\satsamarker.wav");
        }
        private void dollar50_click(object? sender, MouseEventArgs e)
        {
            if (!p�g�endeRunda)
            {
                if (f�rstaRunda)
                {
                    p�g�endeRunda = !p�g�endeRunda;
                    f�rstaRunda = !f�rstaRunda;
                }
                else
                {
                    nyOmg�ng();
                    p�g�endeRunda = !p�g�endeRunda;
                }
            }

            if (e.Button == MouseButtons.Right)
            {
                if (vilkensatsat)
                {
                    if (50 > spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma)
                    {
                        spelarinfo.kontobalans += spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma;
                        spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma = 0;
                        harGjortBet();

                    }
                    else
                    {
                        spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma -= 50;
                        spelarinfo.kontobalans += 50;
                        harGjortBet();
                    }
                }
                else
                {
                    if (50 > spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma)
                    {
                        spelarinfo.kontobalans += spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma;
                        spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma = 0;
                        harGjortBet();

                    }
                    else
                    {
                        spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma -= 50;
                        spelarinfo.kontobalans += 50;
                        harGjortBet();
                    }
                }
            }
            else
            {
                if (!kollaKryss[2])
                {
                    if (vilkensatsat)
                    {
                        if (spelarinfo.kontobalans >= 50)
                        {
                            spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma += 50;
                            spelarinfo.kontobalans -= 50;
                            harGjortBet();
                        }
                        else
                        {
                            MessageBox.Show("Du har inte tillr�ckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        if (spelarinfo.kontobalans >= 50)
                        {
                            spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma += 50;
                            spelarinfo.kontobalans -= 50;
                            harGjortBet();
                        }
                        else
                        {
                            MessageBox.Show("Du har inte tillr�ckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            
            maxBet();
            spelaLjudUtanSync(@"C:\\Black Jack\Audio\satsamarker.wav");
        }
        private void dollar100_click(object? sender, MouseEventArgs e)
        {
            if (!p�g�endeRunda)
            {
                if (f�rstaRunda)
                {
                    p�g�endeRunda = !p�g�endeRunda;
                    f�rstaRunda = !f�rstaRunda;
                }
                else
                {
                    nyOmg�ng();
                    p�g�endeRunda = !p�g�endeRunda;
                }
            }

            if (e.Button == MouseButtons.Right)
            {
                if (vilkensatsat)
                {
                    if (100 > spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma)
                    {
                        spelarinfo.kontobalans += spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma;
                        spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma = 0;
                        harGjortBet();

                    }
                    else
                    {
                        spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma -= 100;
                        spelarinfo.kontobalans += 100;
                        harGjortBet();
                    }
                }
                else
                {
                    if (100 > spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma)
                    {
                        spelarinfo.kontobalans += spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma;
                        spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma = 0;
                        harGjortBet();

                    }
                    else
                    {
                        spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma -= 100;
                        spelarinfo.kontobalans += 100;
                        harGjortBet();
                    }
                }
            }
            else
            {
                if (!kollaKryss[3])
                {
                    if (vilkensatsat)
                    {
                        if (spelarinfo.kontobalans >= 100)
                        {
                            spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma += 100;
                            spelarinfo.kontobalans -= 100;
                            harGjortBet();
                        }
                        else
                        {
                            MessageBox.Show("Du har inte tillr�ckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        if (spelarinfo.kontobalans >= 100)
                        {
                            spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma += 100;
                            spelarinfo.kontobalans -= 100;
                            harGjortBet();
                        }
                        else
                        {
                            MessageBox.Show("Du har inte tillr�ckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            
            maxBet();
            spelaLjudUtanSync(@"C:\\Black Jack\Audio\satsamarker.wav");
        }
        private void dollar500_click(object? sender, MouseEventArgs e)
        {
            if (!p�g�endeRunda)
            {
                if (f�rstaRunda)
                {
                    p�g�endeRunda = !p�g�endeRunda;
                    f�rstaRunda = !f�rstaRunda;
                }
                else
                {
                    nyOmg�ng();
                    p�g�endeRunda = !p�g�endeRunda;
                }
            }

            if (e.Button == MouseButtons.Right)
            {
                if (vilkensatsat)
                {
                    if (500 > spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma)
                    {
                        spelarinfo.kontobalans += spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma;
                        spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma = 0;
                        harGjortBet();

                    }
                    else
                    {
                        spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma -= 500;
                        spelarinfo.kontobalans += 500;
                        harGjortBet();
                    }
                }
                else
                {
                    if (500 > spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma)
                    {
                        spelarinfo.kontobalans += spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma;
                        spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma -= 500;
                        harGjortBet();

                    }
                    else
                    {
                        spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma -= 500;
                        spelarinfo.kontobalans += 500;
                        harGjortBet();
                    }
                }
            }
            else
            {
                if (!kollaKryss[4])
                {
                    if (vilkensatsat)
                    {
                        if (spelarinfo.kontobalans >= 500)
                        {
                            spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma += 500;
                            spelarinfo.kontobalans -= 500;
                            harGjortBet();
                        }
                        else
                        {
                            MessageBox.Show("Du har inte tillr�ckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        if (spelarinfo.kontobalans >= 500)
                        {
                            spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma += 500;
                            spelarinfo.kontobalans -= 500;
                            harGjortBet();
                        }
                        else
                        {
                            MessageBox.Show("Du har inte tillr�ckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            
            maxBet();
            spelaLjudUtanSync(@"C:\\Black Jack\Audio\satsamarker.wav");
        }
        private void dollar1000_click(object? sender, MouseEventArgs e)
        {
            if (!p�g�endeRunda)
            {
                if (f�rstaRunda)
                {
                    p�g�endeRunda = !p�g�endeRunda;
                    f�rstaRunda = !f�rstaRunda;
                }
                else
                {
                    nyOmg�ng();
                    p�g�endeRunda = !p�g�endeRunda;
                }
            }

            if (e.Button == MouseButtons.Right)
            {
                if (vilkensatsat)
                {
                    if (1000 > spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma)
                    {
                        spelarinfo.kontobalans += spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma;
                        spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma = 0;
                        harGjortBet();

                    }
                    else
                    {
                        spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma -= 1000;
                        spelarinfo.kontobalans += 1000;
                        harGjortBet();
                    }
                }
                else
                {
                    if (1000 > spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma)
                    {
                        spelarinfo.kontobalans += spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma;
                        spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma = 0;
                        harGjortBet();

                    }
                    else
                    {
                        spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma -= 1000;
                        spelarinfo.kontobalans += 1000;
                        harGjortBet();
                    }
                }
            }
            else
            {
                if (!kollaKryss[5])
                {
                    if (vilkensatsat)
                    {
                        if (spelarinfo.kontobalans >= 1000)
                        {
                            spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma += 1000;
                            spelarinfo.kontobalans -= 1000;
                            harGjortBet();
                        }
                        else
                        {
                            MessageBox.Show("Du har inte tillr�ckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        if (spelarinfo.kontobalans >= 1000)
                        {
                            spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma += 1000;
                            spelarinfo.kontobalans -= 1000;
                            harGjortBet();
                        }
                        else
                        {
                            MessageBox.Show("Du har inte tillr�ckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            
            maxBet();
            spelaLjudUtanSync(@"C:\\Black Jack\Audio\satsamarker.wav");
        }

        private void knappHit_click(object? sender, MouseEventArgs e)
        {

            if (spelarlista[spelarinfo.spelarNummer].spelh�g[0].betsumma > 0)
            {
                if (p�g�endeRunda)
                {
                    try
                    {
                        if (spelarlista[spelarinfo.spelarNummer].spelh�g[1].betsumma == 0) spelarlista[spelarinfo.spelarNummer].spelh�g.RemoveAt(1);
                    } catch { }



                    if (!harDelat2)
                    {
                        spelaKort();                     
                        harDelat2 = !harDelat2;
                    }
                    else
                    {
                        l�ggtillKort(spelarinfo.spelarNummer, vilkenSpelh�g);
                        if (kollaVinnare(spelarinfo.spelarNummer, vilkenSpelh�g))
                        {
                            vilkenSpelh�g++;
                            try
                            {
                                spelarlista[spelarinfo.spelarNummer].spelh�g[vilkenSpelh�g].kortsumma.BackColor = Color.Yellow;
                            } catch { }
                        }
                        if (vilkenSpelh�g >= spelarlista[spelarinfo.spelarNummer].spelh�g.Count)
                            if (spelarinfo.spelarNummer + 1 > spelarinfo.antalspelare) 
                            {
                                g�mKnappar();
                                bankAi();
                            }
                            else
                            {
                                g�mKnappar();
                                datorAI(spelarinfo.spelarNummer + 1);
                            }

                    }

                }
            }
            else
            {
                MessageBox.Show("Du m�ste satsa n�got", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
         }

        private void knappPass_click(object? sender, MouseEventArgs e)
        {
            if (p�g�endeRunda)
            {
                if (vilkenSpelh�g < spelarlista[spelarinfo.spelarNummer].spelh�g.Count)
                {
                    vilkenSpelh�g++;
                    vilkensatsat = !vilkensatsat;
                    try
                    {
                        spelarlista[spelarinfo.spelarNummer].spelh�g[vilkenSpelh�g].kortsumma.BackColor = Color.Yellow;
                    } catch { }
                }
                if (vilkenSpelh�g >= spelarlista[spelarinfo.spelarNummer].spelh�g.Count)
                {
                    if (spelarinfo.spelarNummer + 1 > spelarinfo.antalspelare)
                    {
                        g�mKnappar();
                        bankAi();
                    }
                    else
                    {
                        g�mKnappar();
                        datorAI(spelarinfo.spelarNummer + 1);
                    }
                }
            }
        }

        private void knappDouble_click(object? sender, MouseEventArgs e)
        {
            if (dubbla(spelarinfo.spelarNummer, vilkensatsat))
            {
                l�ggtillKort(spelarinfo.spelarNummer, vilkenSpelh�g);
                kollaVinnare(spelarinfo.spelarNummer, vilkenSpelh�g);
                vilkenSpelh�g++;
                vilkensatsat = false;
                try
                {
                    spelarlista[spelarinfo.spelarNummer].spelh�g[vilkenSpelh�g].kortsumma.BackColor = Color.Yellow;
                }
                catch { }
            }
            if (vilkenSpelh�g >= spelarlista[spelarinfo.spelarNummer].spelh�g.Count)
            {
                if (spelarinfo.spelarNummer + 1 > spelarinfo.antalspelare)
                {
                    g�mKnappar();
                    bankAi();
                }
                else
                {
                    g�mKnappar();
                    datorAI(spelarinfo.spelarNummer + 1);
                }
            }
        }

        private void knappSplit_click(object? sender, MouseEventArgs e)
        {            
            if (split(spelarinfo.spelarNummer, vilkensatsat))
            {
                if (vilkensatsat)
                {
                    l�ggtillKort(spelarinfo.spelarNummer, vilkenSpelh�g);
                    l�ggtillKort(spelarinfo.spelarNummer, vilkenSpelh�g + 1);
                    vilkensatsat = false;
                    try
                    {
                        spelarlista[spelarinfo.spelarNummer].spelh�g[vilkenSpelh�g].kortsumma.BackColor = Color.Yellow;
                        spelarlista[spelarinfo.spelarNummer].spelh�g[vilkenSpelh�g + 1].kortsumma.BackColor = Color.Yellow;
                    }
                    catch { }
                    vilkenSpelh�g += 2;
                } else
                {
                    l�ggtillKort(spelarinfo.spelarNummer, vilkenSpelh�g);
                    l�ggtillKort(spelarinfo.spelarNummer, vilkenSpelh�g + 1);
                    vilkensatsat = false;
                    try
                    {
                        spelarlista[spelarinfo.spelarNummer].spelh�g[vilkenSpelh�g].kortsumma.BackColor = Color.Yellow;
                        spelarlista[spelarinfo.spelarNummer].spelh�g[vilkenSpelh�g + 1].kortsumma.BackColor = Color.Yellow;
                    }
                    catch { }
                    vilkenSpelh�g += 2;
                }

                if (vilkenSpelh�g >= spelarlista[spelarinfo.spelarNummer].spelh�g.Count)
                {
                    if (spelarinfo.spelarNummer + 1 > spelarinfo.antalspelare)
                    {
                        g�mKnappar();
                        bankAi();
                    }
                    else
                    {
                        g�mKnappar();
                        datorAI(spelarinfo.spelarNummer + 1);
                    }
                }
            }
        }

        private void ram_click(object sender, EventArgs e)
        {
            int x = 0;
            PictureBox klickadRam = sender as PictureBox;
            List<PictureBox> bilderIram = new List<PictureBox>();
            bilderIram.Add(klickadRam);
            bool y = (bool)klickadRam.Tag;

            if (y != true)
            {
                foreach (Control c in this.Controls)
                {
                    if (c is PictureBox)
                    {
                        x++;
                        PictureBox andraRam = (PictureBox)c;
                        if (andraRam.Tag != null)
                        {
                            y = (bool)andraRam.Tag;
                            if (y == true)
                            {
                                if (vilkensatsat) kollaKryssV = kollaKryss;
                                else kollaKryssH = kollaKryss;
                                y = !y;
                                andraRam.Tag = y;
                                bilderIram.Add(andraRam);
                            }
                        }
                    }
                }
                if (vilkensatsat) kollaKryss = kollaKryssH;
                else kollaKryss = kollaKryssV;
                y = (bool)klickadRam.Tag;
                y = !y;
                klickadRam.Tag = y;
                vilkensatsat = !vilkensatsat;
                maxBet();
                this.Controls.Remove(bilderIram[0]);
                this.Controls.Remove(bilderIram[1]);
                DrawFilledRoundedRectangle(bilderIram[0], gr�npensel);
                DrawFilledRoundedRectangle(bilderIram[1], r�dpensel);
            }
        }

        #endregion


    }

}