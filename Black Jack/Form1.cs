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

namespace Black_Jack
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        login loggain; //Klass f�r att hantera val av spelare och antal
        Marker marker = new Marker(); //Klass som hanterar satsning
        List<Spelare> spelarlista = new List<Spelare>();
        Spelare spelare;

        List<string> nyKortlek = new List<string>(); //Tar emot kortleken fr�n kortleksklassen
        List<int> markerv�rde = new List<int>(); //Hur m�nga marker spelare har satsat
        List<int> markerv�rde1 = new List<int>(); // Hur m�nga marker spelare har satsat p� andra

        int[] kortPosX = new int[7] { 756, 155, 385, 615, 845, 1075, 1305 };
        int[] kortPosY = new int[7] { 250, 340, 460, 550, 550, 460, 340 };

        //F�r bilder p� de olika markerna
        PictureBox dollar5 = new PictureBox(); 
        PictureBox dollar10 = new PictureBox();
        PictureBox dollar50 = new PictureBox();
        PictureBox dollar100 = new PictureBox();
        PictureBox dollar500 = new PictureBox();
        PictureBox dollar1000 = new PictureBox(); 

        //H�ller kolla p� om det �verstiger maxbet p� $1000
        bool[] kollaKryss = new bool[] { false, false, false, false, false, false }; //True om spelarens inte har tillr�ckligt med marker f�r att betta en vissa val�r
        bool[] kollaKryssV = new bool[] { false, false, false, false, false, false }; //Sparar den icke-aktive beth�gen
        bool[] kollaKryssH = new bool[] { false, false, false, false, false, false }; 


        bool p�g�endeRunda = false; //H�ller koll p� om det �r en p�g�ende spelrunda (true) eller om man �r i bettingfasen (false).
        bool vilkensatsat = true; //H�lla koll p� om v�nster eller h�ger bet, true = v�nster
        bool spelaresTur = false;

        //System.Windows.Forms.Button knappHit, knappPass, knappDouble, knappSplit; //Knappar som spelaren kan trycka p�
        System.Windows.Forms.Button knappHit = new System.Windows.Forms.Button();
        System.Windows.Forms.Button knappPass = new System.Windows.Forms.Button();
        System.Windows.Forms.Button knappDouble = new System.Windows.Forms.Button();
        System.Windows.Forms.Button knappSplit = new System.Windows.Forms.Button();

        System.Windows.Forms.ToolTip hitTooltip = new System.Windows.Forms.ToolTip(); //Tooltip f�r Hit-knappen
        System.Windows.Forms.ToolTip passTooltip = new System.Windows.Forms.ToolTip(); //Tooltip f�r Pass-knappen
        System.Windows.Forms.ToolTip doubleTooltip = new System.Windows.Forms.ToolTip(); //Tooltip f�r Double-knappen
        System.Windows.Forms.ToolTip splitTooltip = new System.Windows.Forms.ToolTip(); //Tooltip f�r Split-knappen

        SolidBrush r�dpensel = new SolidBrush(Color.IndianRed); //F�rgl�gger d�r spelarens satsar
        SolidBrush gr�npensel = new SolidBrush(Color.LightGreen); //F�rgl�gger d�r spelarens satsar
   



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
                if (spelarlista[i].Contains(spelarinfo.spelarnamn))
                {
                    x = spelarlista[i];
                    int y = x.IndexOf(',');
                    x = x.Remove(y);
                    x += ", " + spelarinfo.kontobalans.ToString();
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



        private void bytSpelareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loggain.ShowDialog();

        }

        private void tooltips()
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

            loggain = new login();
            loggain.ShowDialog();

            this.Size = new System.Drawing.Size(1600, 900);
            this.Location = new Point(0, 0);
            this.StartPosition = FormStartPosition.Manual;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.DoubleBuffered = true;
            Image bakgrundbild = Image.FromFile(@"C:\\Black Jack\bilder\blackjackbord.png");
            Bitmap nystorlek = new Bitmap(bakgrundbild, 1440, 647);
            this.BackgroundImage = nystorlek;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            Random random = new Random();
            spelarinfo.spelarNummer = random.Next(1, spelarinfo.antalspelare + 1);

            l�ggtillInitialaRamar();
            skapaMarkerBilder();
            tooltips();
            datorBet();

        }

        #region l�ggtillcontrols

   
        private void l�ggTillKnapp(int posX, int posY)
        {
            //knappHit = new System.Windows.Forms.Button();
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

            //knappPass = new System.Windows.Forms.Button();
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
            //knappPass.Hide();

            //knappDouble = new System.Windows.Forms.Button();
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
            //knappDouble.Hide();

            //knappSplit = new System.Windows.Forms.Button();
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
            ////knappSplit.Hide();
        }

        private void l�ggtillInitialaRamar()
        {
            //int spelarPanelposX = 155;
            int datornummer = 0;
            int posXskillnad = 0;


            for (int i = 0; i < spelarinfo.antalspelare+1; i++)
            {
                if (i == 0)
                {
                    spelarlista.Add(spelare = new Spelare(i));
                    spelarlista[i].l�ggtillSpelh�g();
                }
                else
                {
                    label1.Text += i.ToString();
                    if (i != spelarinfo.spelarNummer) datornummer++;
                    spelarlista.Add(spelare = new Spelare(i));
                    spelarlista[i].l�ggtillSpelarinfo(i, datornummer, kortPosX[i], kortPosY[i] + 102);
                    this.Controls.Add(spelarlista[i].spelarinfolabel);

                    for (int j = 0; j < 2; j++)
                    {
                        spelarlista[i].l�ggtillSpelh�g();
                        spelarlista[i].l�ggtillBetinfo(j, kortPosX[i] + posXskillnad, kortPosY[i] + 77);
                        spelarlista[i].l�ggtillKortSumma(j, kortPosX[i] + 15 + posXskillnad, kortPosY[i] - 20);
                        this.Controls.Add(spelarlista[i].spelh�g[j].betinfo);
                        this.Controls.Add(spelarlista[i].spelh�g[j].kortsumma);
                        PictureBox p = new PictureBox();
                        p.Size = new Size(50, 72);
                        p.Location = new Point(kortPosX[i] + posXskillnad, kortPosY[i]);
                        p.BackColor = Color.Transparent;
                        p.SizeMode = PictureBoxSizeMode.StretchImage;
                        if (i == spelarinfo.spelarNummer) p.Click += new EventHandler(ram_click);
                        if (i == spelarinfo.spelarNummer)
                        {
                            
                            if (j == 0)
                            {
                                l�ggTillKnapp(p.Location.X - 35, p.Location.Y);
                                p.Tag = true;
                                DrawFilledRoundedRectangle(p, gr�npensel);
                            }
                            else
                            {
                                p.Tag = false;
                                DrawFilledRoundedRectangle(p, r�dpensel);
                            }
                        }
                        else
                        {
                            DrawTranslucentRoundedRectangle(p);
                        }
                        posXskillnad += 55;
                        Random r = new Random();
                        int bet1eller2 = r.Next(1, 11);
                        if (bet1eller2 < 4 && i != spelarinfo.spelarNummer) break;
                    }
                    posXskillnad = 0;
                }

            }

        }

        #endregion

        #region bettar
        private void harGjortBet()
        {
            if (vilkensatsat)
            {
                spelarlista[spelarinfo.spelarNummer].spelh�g[0].betinfo.Text = "$" + markerv�rde.Sum().ToString();
                spelarlista[spelarinfo.spelarNummer].uppdateraSpelarinfo(spelarinfo.spelarNummer);
            }
            else
            {
                spelarlista[spelarinfo.spelarNummer].spelh�g[1].betinfo.Text = "$" + markerv�rde1.Sum().ToString();
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
                        spelarlista[i].spelh�g[j].betinfo.Text = "$:" + betAI(spelarlista[i].datorbalans, i).ToString();
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


        private void rensaKort()
        {

        }

        #region spellogik
        private async Task spelaKort()
        {
            if (markerv�rde.Sum() > 0)
            {
                label1.Text = "";
                string genv�g;
                Random rnd = new Random();
                int draKort;
                if (!p�g�endeRunda)
                {
                    //spelaLjud(@"C:\\Black Jack\Audio\lekblandas.wav");
                    resetKort();
                    nyKortlek = Kortlek.Nykortlek();
                    for (int i = 0; i < (spelarinfo.antalspelare + 1); i++)
                    {
                        if (i == 0)
                        {
                            //spelaLjud(@"C:\\Black Jack\Audio\kortspelas.wav");
                            draKort = rnd.Next(nyKortlek.Count);
                            nyKortlek.RemoveAt(draKort);
                            spelarlista[i].l�ggtillKortochV�rde(nyKortlek[draKort], i, kortPosX[i], kortPosY[i]);
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
                                    draKort = rnd.Next(nyKortlek.Count);
                                    nyKortlek.RemoveAt(draKort);
                                    spelarlista[i].l�ggtillKortochV�rde(nyKortlek[draKort], j, kortPosX[i] + posXskillnad, kortPosY[i] - posYskillnad);
                                    posYskillnad += 15;
                                }
                                posXskillnad += 55;
                            }
                        }
                    }

                    visaSpelkort();
                    knappDouble.Show();
                    knappPass.Show();
                    p�g�endeRunda = true;
                    await Task.Delay(500);
                }

            }
            else
            {
                MessageBox.Show("Du m�ste satsa n�got", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //private void split(int vilkenspelare, bool VellerH)
        //{
        //    //Koppla listan med kort till bild i taggen
        //    int x;
        //    if (VellerH) x = 0;
        //    else x = 1;
        //    foreach (Control c in panelLista[vilkenspelare].Controls) 
        //    {
        //        if (c is PictureBox)
        //        {
        //            if (VellerH) //V�nsterh�gen
        //            {
        //                dragnakort[vilkenspelare][x][0].Location = new Point(dragnakort[vilkenspelare][x][0].Location.X, dragnakort[vilkenspelare][x][0].Location.Y - 77); //Flyttar upp f�rsta kortet i h�gen
        //                dragnakort[vilkenspelare][x][1].Location = new Point(dragnakort[vilkenspelare][x][0].Location.X - 55, dragnakort[vilkenspelare][x][0].Location.Y); //Flyttar upp, och till v�nster, andra kortet 
        //                pics = new List<PictureBox>();
        //                dragnakort[vilkenspelare].Insert(0, pics); //l�gger till en ny lista f�r picturebox, f�rst i listan
        //                dragnakort[vilkenspelare][x].Add(dragnakort[vilkenspelare][x + 1][0]); //Kopierar ett av korten till nya listan
        //                dragnakort[vilkenspelare][x + 1].RemoveAt(0); //Tar bort kortet som kopierats

        //                for (int i = 0; i < datorLabelLista[vilkenspelare].Count; i++)
        //                {
        //                    if ((string)datorLabelLista[vilkenspelare][i].Tag == "bet1")
        //                    {
        //                        datorLabelLista[vilkenspelare][i].Location = new Point(datorLabelLista[vilkenspelare][i].Location.X - 55, datorLabelLista[vilkenspelare][i].Location.Y - 77);
        //                        visaspelarebet = new Label();
        //                        visaspelarebet.Location = new Point(datorLabelLista[vilkenspelare][i].Location.X + 55, datorLabelLista[vilkenspelare][i].Location.Y);
        //                        visaspelarebet.Font = new Font("MS Gothic", 12, FontStyle.Bold);
        //                        visaspelarebet.BackColor = Color.LightGray;
        //                        visaspelarebet.AutoSize = true;
        //                        visaspelarebet.BringToFront();
        //                        visaspelarebet.Text = datorLabelLista[vilkenspelare][i].Text;
        //                        visaspelarebet.Tag = "bet2";
        //                        for (int j = 0; j < datorLabelLista[vilkenspelare].Count; j++)
        //                        {
        //                            if ((string)datorLabelLista[vilkenspelare][j].Tag == "bet2") datorLabelLista[vilkenspelare][j].Tag = "bet3";
        //                        }
        //                        datorLabelLista[vilkenspelare].Add(visaspelarebet);
        //                        panelLista[vilkenspelare].Controls.Add(visaspelarebet);
        //                        knappHit.Location = new Point(knappHit.Location.X, knappHit.Location.Y + 20);
        //                        knappPass.Location = new Point(knappPass.Location.X, knappPass.Location.Y + 20);
                               
        //                        kortlista = new List<int>();
        //                        kortv�rdeDator[vilkenspelare].Insert(1, kortlista);
        //                        kortv�rdeDator[vilkenspelare][1].Add(kortv�rdeDator[vilkenspelare][0][1]);
        //                        kortv�rdeDator[vilkenspelare][0].RemoveAt(1);

        //                        break;
        //                    }
        //                }
        //                for (int i = 0; i < datorLabelLista[vilkenspelare].Count; i++) 
        //                { 
        //                   if ((string)datorLabelLista[vilkenspelare][i].Tag == "kort1")
        //                   {
        //                        datorLabelLista[vilkenspelare][i].Location = new Point(datorLabelLista[vilkenspelare][i].Location.X - 55, datorLabelLista[vilkenspelare][i].Location.Y - 62);
        //                        visaspelarebet = new Label();
        //                        visaspelarebet.Location = new Point(datorLabelLista[vilkenspelare][i].Location.X + 55, datorLabelLista[vilkenspelare][i].Location.Y);
        //                        visaspelarebet.Font = new Font("MS Gothic", 12, FontStyle.Bold);
        //                        visaspelarebet.BackColor = Color.LightGray;
        //                        visaspelarebet.AutoSize = true;
        //                        visaspelarebet.BringToFront();
        //                        visaspelarebet.Text = datorLabelLista[vilkenspelare][i].Text;
        //                        visaspelarebet.Tag = "kort2";
        //                        for (int j = 0; j < datorLabelLista[vilkenspelare].Count; j++)
        //                        {
        //                            if ((string)datorLabelLista[vilkenspelare][j].Tag == "kort2") datorLabelLista[vilkenspelare][j].Tag = "kort3";
        //                        }
        //                        datorLabelLista[vilkenspelare].Add(visaspelarebet);
        //                        panelLista[vilkenspelare].Controls.Add(visaspelarebet);
        //                        break;
        //                    }
        //                }
        //                break;
        //            }

        //            else
        //            {
        //                if (dragnakort[vilkenspelare].Count > 2) x = 2;
        //                dragnakort[vilkenspelare][x][0].Location = new Point(dragnakort[vilkenspelare][x][0].Location.X, dragnakort[vilkenspelare][x][0].Location.Y - 77); //Flyttar upp f�rsta kortet i h�gen
        //                dragnakort[vilkenspelare][x][1].Location = new Point(dragnakort[vilkenspelare][x][0].Location.X + 55, dragnakort[vilkenspelare][x][0].Location.Y); //Flyttar upp, och till v�nster, andra kortet 
        //                pics = new List<PictureBox>();
        //                dragnakort[vilkenspelare].Add(pics); //l�gger till en ny lista f�r picturebox, sist i listan
        //                dragnakort[vilkenspelare][x + 1].Add(dragnakort[vilkenspelare][x][1]); //Kopierar ett av korten till nya listan
        //                dragnakort[vilkenspelare][x].RemoveAt(1); //Tar bort kortet som kopierats


        //                string bet2eller3 = "bet2";
        //                string kort2eller3 = "kort2";
        //                for (int i = 0; i < datorLabelLista[vilkenspelare].Count; i++)
        //                {
        //                    if ((string)datorLabelLista[vilkenspelare][i].Tag == "bet3") bet2eller3 = "bet3";
        //                    else if ((string)datorLabelLista[vilkenspelare][i].Tag == "kort3") kort2eller3 = "kort3";
        //                }

        //                for (int i = 0; i < datorLabelLista[vilkenspelare].Count; i++)
        //                {
        //                    if ((string)datorLabelLista[vilkenspelare][i].Tag == bet2eller3)
        //                    {
        //                        datorLabelLista[vilkenspelare][i].Location = new Point(datorLabelLista[vilkenspelare][i].Location.X, datorLabelLista[vilkenspelare][i].Location.Y - 77);
        //                        visaspelarebet = new Label();
        //                        visaspelarebet.Location = new Point(datorLabelLista[vilkenspelare][i].Location.X + 55, datorLabelLista[vilkenspelare][i].Location.Y);
        //                        visaspelarebet.Font = new Font("MS Gothic", 12, FontStyle.Bold);
        //                        visaspelarebet.BackColor = Color.LightGray;
        //                        visaspelarebet.AutoSize = true;
        //                        visaspelarebet.BringToFront();
        //                        visaspelarebet.Text = datorLabelLista[vilkenspelare][i].Text;
        //                        if (bet2eller3 == "bet2") visaspelarebet.Tag = "bet3";
        //                        else visaspelarebet.Tag = "bet4";

        //                        datorLabelLista[vilkenspelare].Add(visaspelarebet);
        //                        panelLista[vilkenspelare].Controls.Add(visaspelarebet);
        //                        knappDouble.Location = new Point(knappDouble.Location.X, knappDouble.Location.Y + 20);
        //                        knappSplit.Location = new Point(knappSplit.Location.X, knappSplit.Location.Y + 20);

        //                        kortlista = new List<int>();
        //                        kortv�rdeDator[vilkenspelare].Add(kortlista);
        //                        kortv�rdeDator[vilkenspelare][x+1].Add(kortv�rdeDator[vilkenspelare][x][1]);
        //                        kortv�rdeDator[vilkenspelare][x].RemoveAt(1);

        //                        for (int a = 0; a < kortv�rdeDator[vilkenspelare].Count; a++)
        //                        {
        //                            for (int b = 0; b < kortv�rdeDator[vilkenspelare][a].Count; b++) label1.Text += kortv�rdeDator[vilkenspelare][a][b].ToString();
        //                        }
        //                        break;
        //                    }
        //                }

        //                for (int i = 0; i < datorLabelLista[vilkenspelare].Count; i++)
        //                {
        //                    if ((string)datorLabelLista[vilkenspelare][i].Tag == kort2eller3)
        //                    {
        //                        datorLabelLista[vilkenspelare][i].Location = new Point(datorLabelLista[vilkenspelare][i].Location.X, datorLabelLista[vilkenspelare][i].Location.Y - 62);
        //                        visaspelarebet = new Label();
        //                        visaspelarebet.Location = new Point(datorLabelLista[vilkenspelare][i].Location.X + 55, datorLabelLista[vilkenspelare][i].Location.Y);
        //                        visaspelarebet.Font = new Font("MS Gothic", 12, FontStyle.Bold);
        //                        visaspelarebet.BackColor = Color.LightGray;
        //                        visaspelarebet.AutoSize = true;
        //                        visaspelarebet.BringToFront();
        //                        visaspelarebet.Text = datorLabelLista[vilkenspelare][i].Text;
        //                        if (kort2eller3 == "kort2") visaspelarebet.Tag = "kort3";
        //                        else visaspelarebet.Tag = "kort4";
        //                        datorLabelLista[vilkenspelare].Add(visaspelarebet);
        //                        panelLista[vilkenspelare].Controls.Add(visaspelarebet);
        //                        break;
        //                    }
        //                }
                        
        //            }
        //            break;
        //        }
        //    }



        //    for (int j = 0; j < datorLabelLista[vilkenspelare].Count; j++)
        //    {
        //        if ((string)datorLabelLista[vilkenspelare][j].Tag == "kort1") datorLabelLista[vilkenspelare][j].Text = kortlek.ber�knaKortv�rde(kortv�rdeDator[vilkenspelare][0]).ToString();
        //        if ((string)datorLabelLista[vilkenspelare][j].Tag == "kort2") datorLabelLista[vilkenspelare][j].Text = kortlek.ber�knaKortv�rde(kortv�rdeDator[vilkenspelare][1]).ToString();
        //        if ((string)datorLabelLista[vilkenspelare][j].Tag == "kort3") datorLabelLista[vilkenspelare][j].Text = kortlek.ber�knaKortv�rde(kortv�rdeDator[vilkenspelare][2]).ToString();
        //        if ((string)datorLabelLista[vilkenspelare][j].Tag == "kort4") datorLabelLista[vilkenspelare][j].Text = kortlek.ber�knaKortv�rde(kortv�rdeDator[vilkenspelare][3]).ToString();
        //    }
        //}

        private void passa()
        {
            spelaresTur = false;
            kollaVinnare();
        }

        private void kollaVinnare()
        {
        //    if (kortv�rdeSpelare > 21)
        //    {
        //        antalMarker -= spelarebet;
        //        p�g�endeRunda = false;
        //        totalBet.Text = "Du f�rlorade $" + markerv�rde.Sum();
        //        bankrulle.Text = spelarNamn + " du har $" + antalMarker;
        //        spelaresTur = true;
        //        spelaLjudUtanSync(@"C:\\Black Jack\Audio\f�rlust.wav");
        //        uppdateraFil();
        //        markerv�rde.Clear();
        //        maxBet();
        //    }
        //    else if (kortv�rdeBank > 21)
        //    {
        //        antalMarker += spelarebet;
        //        p�g�endeRunda = false;
        //        totalBet.Text = "Du vann $" + markerv�rde.Sum();
        //        bankrulle.Text = spelarNamn + " du har $" + antalMarker;
        //        spelaresTur = true;
        //        spelaLjudUtanSync(@"C:\\Black Jack\Audio\vinst.wav");
        //        uppdateraFil();
        //        markerv�rde.Clear();
        //        maxBet();
        //    }
        //    else if (kortv�rdeBank >= kortv�rdeSpelare && kortv�rdeBank > 16 && kortv�rdeBank < 22)
        //    {
        //        antalMarker -= spelarebet;
        //        p�g�endeRunda = false;
        //        totalBet.Text = "Du f�rlorade $" + markerv�rde.Sum();
        //        bankrulle.Text = spelarNamn + " du har $" + antalMarker;
        //        spelaresTur = true;
        //        spelaLjudUtanSync(@"C:\\Black Jack\Audio\f�rlust.wav");
        //        uppdateraFil();
        //        markerv�rde.Clear();
        //        maxBet();
        ////    }
        //    else if (!spelaresTur) spelaKort();
        }

        private void maxBet()
        {

            for (int i = 0; i < kollaKryss.Length; i++)
            {
                kollaKryss[i] = false;
            }
            if (vilkensatsat)
            {
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
            else
            {
                if (markerv�rde1.Sum() > 0)
                {
                    kollaKryss[5] = true;
                    dollar1000.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\1000$kryss.png");
                }
                else dollar1000.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\1000$.png");

                if (markerv�rde1.Sum() > 500)
                {
                    kollaKryss[4] = true;
                    dollar500.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\500$kryss.png");
                }
                else dollar500.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\500$.png");

                if (markerv�rde1.Sum() > 900)
                {
                    kollaKryss[3] = true;
                    dollar100.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\100$kryss.png");
                }
                else dollar100.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\100$.png");

                if (markerv�rde1.Sum() > 950)
                {
                    kollaKryss[2] = true;
                    dollar50.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\50$kryss.png");
                }
                else dollar50.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\50$.png");

                if (markerv�rde1.Sum() > 990)
                {
                    kollaKryss[1] = true;
                    dollar10.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\10$kryss.png");
                }
                else dollar10.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\10$.png");

                if (markerv�rde1.Sum() > 995)
                {
                    kollaKryss[0] = true;
                    dollar5.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\5$kryss.png");
                }
                else dollar5.Image = Image.FromFile(@"C:\\Black Jack\bilder\Spelmarker\5$.png");
            }
        }
        private void resetKort()
        {

        }

        #endregion



        #region l�ggtillbilder

        private void visaSpelkort()
        {
            foreach (Control c in Controls)
            {
                if (c is PictureBox)
                {
                    Controls.Remove(c);
                    c.Dispose();
                }
            }

            for (int i = 0; i < spelarlista.Count; i++)
            {
                for (int j = 0; j < spelarlista[i].spelh�g.Count; j++)
                {
                    for (int k = 0; k < spelarlista[i].spelh�g[j].spelkort.Count; k++)
                    {
                        this.Controls.Add(spelarlista[i].spelh�g[j].spelkort[k]);
                    }
                }
            }
        }


        private void skapaMarkerBilder()
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
                if (vilkensatsat)
                {
                    if (spelarlista[spelarinfo.spelarNummer].spelh�g[0].betinfo.Text != "$0")
                    {
                        markerv�rde = marker.raderaMarker(markerv�rde, 5);
                        spelarinfo.kontobalans += 5;
                        harGjortBet();
                    }
                    else
                    {
                    }
                }
                else
                {
                    if (spelarlista[spelarinfo.spelarNummer].spelh�g[1].betinfo.Text != "$0")
                    {
                        markerv�rde1 = marker.raderaMarker(markerv�rde1, 5);
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
                            markerv�rde.Add(5);
                            markerv�rde = marker.sortera(markerv�rde);
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
                            markerv�rde1.Add(5);
                            markerv�rde1 = marker.sortera(markerv�rde1);
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
        private void dollar10_click(object sender, MouseEventArgs e)
        {
            if (!p�g�endeRunda)
            {
                rensaKort();
            }
            if (e.Button == MouseButtons.Right)
            {
                if (vilkensatsat)
                {
                    if (10 > markerv�rde.Sum())
                    {
                        spelarinfo.kontobalans += markerv�rde.Sum();
                        markerv�rde.Clear();
                        harGjortBet();
                    }
                    else
                    {
                        markerv�rde = marker.raderaMarker(markerv�rde, 10);
                        spelarinfo.kontobalans += 10;
                        harGjortBet();
                    }
                }
                else
                {
                    if (10 > markerv�rde.Sum())
                    {
                        spelarinfo.kontobalans += markerv�rde1.Sum();
                        markerv�rde1.Clear();
                        harGjortBet();

                    }
                    else
                    {
                        markerv�rde1 = marker.raderaMarker(markerv�rde1, 10);
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
                            markerv�rde.Add(10);
                            markerv�rde = marker.sortera(markerv�rde);
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
                            markerv�rde1.Add(10);
                            markerv�rde1 = marker.sortera(markerv�rde1);
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
        private void dollar50_click(object sender, MouseEventArgs e)
        {
            if (!p�g�endeRunda)
            {
                rensaKort();
            }
            if (e.Button == MouseButtons.Right)
            {
                if (vilkensatsat)
                {
                    if (50 > markerv�rde.Sum())
                    {
                        spelarinfo.kontobalans += markerv�rde.Sum();
                        markerv�rde.Clear();
                        harGjortBet();

                    }
                    else
                    {
                        markerv�rde = marker.raderaMarker(markerv�rde, 50);
                        spelarinfo.kontobalans += 50;
                        harGjortBet();
                    }
                }
                else
                {
                    if (50 > markerv�rde1.Sum())
                    {
                        spelarinfo.kontobalans += markerv�rde1.Sum();
                        markerv�rde1.Clear();
                        harGjortBet();

                    }
                    else
                    {
                        markerv�rde1 = marker.raderaMarker(markerv�rde1, 50);
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
                            markerv�rde.Add(50);
                            markerv�rde = marker.sortera(markerv�rde);
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
                            markerv�rde1.Add(50);
                            markerv�rde1 = marker.sortera(markerv�rde1);
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
        private void dollar100_click(object sender, MouseEventArgs e)
        {
            if (!p�g�endeRunda)
            {
                rensaKort();
            }
            if (e.Button == MouseButtons.Right)
            {
                if (vilkensatsat)
                {
                    if (100 > markerv�rde.Sum())
                    {
                        spelarinfo.kontobalans += markerv�rde.Sum();
                        markerv�rde.Clear();
                        harGjortBet();

                    }
                    else
                    {
                        markerv�rde = marker.raderaMarker(markerv�rde, 100);
                        spelarinfo.kontobalans += 100;
                        harGjortBet();
                    }
                }
                else
                {
                    if (100 > markerv�rde1.Sum())
                    {
                        spelarinfo.kontobalans += markerv�rde1.Sum();
                        markerv�rde1.Clear();
                        harGjortBet();

                    }
                    else
                    {
                        markerv�rde1 = marker.raderaMarker(markerv�rde1, 100);
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
                            markerv�rde.Add(100);
                            markerv�rde = marker.sortera(markerv�rde);
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
                            markerv�rde1.Add(100);
                            markerv�rde1 = marker.sortera(markerv�rde1);
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
        private void dollar500_click(object sender, MouseEventArgs e)
        {
            if (!p�g�endeRunda)
            {
                rensaKort();
            }
            if (e.Button == MouseButtons.Right)
            {
                if (vilkensatsat)
                {
                    if (500 > markerv�rde.Sum())
                    {
                        spelarinfo.kontobalans += markerv�rde.Sum();
                        markerv�rde.Clear();
                        harGjortBet();

                    }
                    else
                    {
                        markerv�rde = marker.raderaMarker(markerv�rde, 500);
                        spelarinfo.kontobalans += 500;
                        harGjortBet();
                    }
                }
                else
                {
                    if (500 > markerv�rde1.Sum())
                    {
                        spelarinfo.kontobalans += markerv�rde1.Sum();
                        markerv�rde1.Clear();
                        harGjortBet();

                    }
                    else
                    {
                        markerv�rde1 = marker.raderaMarker(markerv�rde1, 500);
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
                            markerv�rde.Add(500);
                            markerv�rde = marker.sortera(markerv�rde);
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
                            markerv�rde1.Add(500);
                            markerv�rde1 = marker.sortera(markerv�rde1);
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
        private void dollar1000_click(object sender, MouseEventArgs e)
        {
            if (!p�g�endeRunda)
            {
                rensaKort();
            }
            if (e.Button == MouseButtons.Right)
            {
                if (vilkensatsat)
                {
                    if (1000 > markerv�rde.Sum())
                    {
                        spelarinfo.kontobalans += markerv�rde.Sum();
                        markerv�rde.Clear();
                        harGjortBet();

                    }
                    else
                    {
                        markerv�rde = marker.raderaMarker(markerv�rde, 1000);
                        spelarinfo.kontobalans += 1000;
                        harGjortBet();
                    }
                }
                else
                {
                    if (1000 > markerv�rde1.Sum())
                    {
                        spelarinfo.kontobalans += markerv�rde1.Sum();
                        markerv�rde1.Clear();
                        harGjortBet();

                    }
                    else
                    {
                        markerv�rde1 = marker.raderaMarker(markerv�rde1, 1000);
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
                            markerv�rde.Add(1000);
                            markerv�rde = marker.sortera(markerv�rde);
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
                            markerv�rde1.Add(1000);
                            markerv�rde1 = marker.sortera(markerv�rde1);
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

        private void knappHit_click(object sender, MouseEventArgs e)
        {
            if (markerv�rde.Sum() == 0) spelarlista[spelarinfo.spelarNummer].spelh�g.RemoveAt(0);
            if (markerv�rde1.Sum() == 0) spelarlista[spelarinfo.spelarNummer].spelh�g.RemoveAt(1);
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
            knappDouble.Hide();
            knappPass.Hide();
            spelaKort();
            passa();
        }

        private void knappSplit_click(object sender, MouseEventArgs e)
        {
            //split(spelarNummer, false);
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