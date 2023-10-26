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
        Kortlek kortlek = new Kortlek(); //Klass f�r att skapa kortlek och h�mta kortens v�rden
        login loggain; //Klass f�r att hantera val av spelare och antal
        Marker marker = new Marker(); //Klass som hanterar satsning


        List<string> nyKortlek = new List<string>(); //Tar emot kortleken fr�n kortleksklassen
        List<int> markerv�rde = new List<int>(); //Hur m�nga marker spelare har satsat
        List<int> markerv�rde1 = new List<int>(); // Hur m�nga marker spelare har satsat p� andra
        List<int> spelarListaKort = new List<int>(); //Lista �ver spelarens kort

        List<List<List<PictureBox>>> dragnakort = new List<List<List<PictureBox>>>(); //Lista �ver dragna kort f�r varje spelare
        List<PictureBox> pics; //Initierar listan, i dragnakort �ver vilka kort som dragits.
        PictureBox dollar5, dollar10, dollar50, dollar100, dollar500, dollar1000; //F�r bilder p� de olika markerna

        List<List<List<int>>> kortv�rdeDator = new List<List<List<int>>>(); //H�ller stringv�rdet f�r varje kort f�r datorn
        List<List<int>> kortv�rdeSpelare = new List<List<int>>(); //H�ller stringv�rdet f�r varje kort f�r spelaren

        List<int> kortlista; //Initierar listan i kortv�rdeDator och kortv�rdeSpelare
        
        List<int> kortv�rdeTotaltDator = new List<int>(); //H�ller det totala kortv�rdet f�r datorns kort
        List<int> kortv�rdeTotaltSpelare = new List<int>(); //H�ller det totala kortv�rdet f�r spelarens kort

        List<List<Label>> datorLabelLista = new List<List<Label>>(); //H�ller lista �ver alla labels f�r varje panel f�r datorspelare
        List<Label> labels; //Listan f�r den enskilde datorpanelen

        List<Label> spelarLabel = new List<Label>(); //H�ller lista �ver alla labels f�r spelaren

        bool[] kollaKryss = new bool[] { false, false, false, false, false, false }; //True om spelarens inte har tillr�ckligt med marker f�r att betta en vissa val�r
        bool[] kollaKryssV = new bool[] { false, false, false, false, false, false };
        bool[] kollaKryssH = new bool[] { false, false, false, false, false, false };

        int antalMarker = 5000; //Hur m�nga marker spelaren har
        int spelarebet;
        int nummer1; //Anv�nds f�r att h�lla koll p� vilken panel om �r datorspelare
        int spelarNummer; //Vilken plats spelaren sitter p�

        bool p�g�endeRunda = false; //H�ller koll p� om det �r en p�g�ende spelrunda (true) eller om man �r i bettingfasen (false).
        bool spelaresTur = true; 
        bool vilkensatsat = true; //H�lla koll p� om v�nster eller h�ger bet, true = v�nster
        bool vilkenSplit = true; //True f�rsta, false andra 

        Image ritaBild; //Lagrar spelkortsbild
        private Panel bankkortPanel; //Panel f�r banken
        private Panel datorPanel; //Panel f�r datorspelarna
        List<Panel> panelLista = new List<Panel>(); //H�ller lista �ver panel f�r alla
        Panel spelarensPlats = new Panel(); //Panel f�r spelaren

        System.Windows.Forms.Button knappHit, knappPass, knappDouble, knappSplit; //Knappar som spelaren kan trycka p�
        PictureBox ram1; //F�r att l�gga till picturesboxes i panelerna
        Label visaspelarebet; //L�gger till label f�r att visa hur mycket man bettar
        Label visaspelareinfo; //L�gger till anv�ndarnamn och hur mycket spelaren/datorn har i marker
        string spelarNamn = ""; //Sparar vad spelaren heter

        System.Windows.Forms.ToolTip hitTooltip; //Tooltip f�r Hit-knappen
        System.Windows.Forms.ToolTip passTooltip; //Tooltip f�r Pass-knappen
        System.Windows.Forms.ToolTip doubleTooltip; //Tooltip f�r Double-knappen
        System.Windows.Forms.ToolTip splitTooltip; //Tooltip f�r Split-knappen

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

            splitTooltip = new System.Windows.Forms.ToolTip();
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
            spelarNamnMarker();
            skapaSpelarPaneler();
            skapaMarkerBilder();
            tooltips();
            datorBet();

        }

        #region l�ggtillcontrols

        private void l�ggTillPanel(int VspelarPanelposX, int VspelarPanelposY, int i)
        {
            datorPanel = new Panel();
            if (i == 0)
            {
                datorPanel.Size = new Size(50, 200);
                datorPanel.Location = new Point(775, 115);
            }
            else
            {
                datorPanel.Size = new Size(215, 270);
                datorPanel.Location = new Point(VspelarPanelposX, VspelarPanelposY);
            }
            datorPanel.BackColor = Color.Transparent;
            datorPanel.Tag = true; //Bet p� f�rsta
            if (spelarNummer == i)
            {
                this.Controls.Add(datorPanel);
                panelLista.Add(datorPanel);
                l�ggTillKnapp();
            }
            else
            {
                this.Controls.Add(datorPanel);
                panelLista.Add(datorPanel);
            }
        }


        private void l�ggTillPicturebox(int ramPosXskillnad, int i, int j)
        {
            ram1 = new PictureBox();
            if (i == 0)
            {
                ram1.Size = new Size(50, 72);
                ram1.Location = new Point(0, 128);
            }
            else
            {
                ram1.Size = new Size(50, 72);
                ram1.Location = new Point(ramPosXskillnad, 140);
            }
            ram1.BackColor = Color.Transparent;
            ram1.SizeMode = PictureBoxSizeMode.StretchImage;
            datorPanel.Controls.Add(ram1);
            if (j == 0) ram1.Tag = true;
            else ram1.Tag = false;
            if (spelarNummer == i) ram1.Click += new EventHandler(ram_click);
        }

        private void l�ggTillBetLabel(int betPosXskillnad, int i, int j)
        {
            visaspelarebet = new Label();
            visaspelarebet.Location = new Point(betPosXskillnad, 217);
            visaspelarebet.Font = new Font("MS Gothic", 12, FontStyle.Bold);
            visaspelarebet.BackColor = Color.LightGray;
            visaspelarebet.AutoSize = true;        
            visaspelarebet.BringToFront();
            visaspelarebet.Text = "$0";
            
            if (j == 0) visaspelarebet.Tag = "bet1";
            else visaspelarebet.Tag = "bet2";
            
            datorLabelLista[i].Add(visaspelarebet);
            datorPanel.Controls.Add(visaspelarebet);
        }


        private void l�ggTillKortv�rdeLabel(int i, int j, int posX)
        {
            visaspelareinfo = new Label();
            if (i == 0) visaspelareinfo.Location = new Point(15, 108);
            else visaspelareinfo.Location = new Point(posX+15, 120);
            visaspelareinfo.Font = new Font("MS Gothic", 12, FontStyle.Bold);
            visaspelareinfo.BackColor = Color.LightGray;
            visaspelareinfo.AutoSize = true;
            visaspelareinfo.BringToFront();

            if (j == 0) visaspelareinfo.Tag = "kort1";
            else visaspelareinfo.Tag = "kort2";

            datorPanel.Controls.Add(visaspelareinfo);
            datorLabelLista[i].Add(visaspelareinfo);
        }

        private void l�ggTillInfoLabel(int i)
        {
            visaspelareinfo = new Label();
            visaspelareinfo.Location = new Point(35, 240);
            visaspelareinfo.Font = new Font("MS Gothic", 12, FontStyle.Bold);
            visaspelareinfo.BackColor = Color.LightGray;
            visaspelareinfo.AutoSize = true;
            visaspelareinfo.BringToFront();
            if (i == spelarNummer) visaspelareinfo.Text = spelarNamn + ": $" + antalMarker;
            else visaspelareinfo.Text = "Dator" + nummer1 + ": $5000";
            visaspelareinfo.Tag = "spelarinfo";
            datorPanel.Controls.Add(visaspelareinfo);
            datorLabelLista[i].Add(visaspelareinfo);
            
        }

        private void l�ggTillKnapp()
        {
            knappHit = new System.Windows.Forms.Button();
            knappHit.Size = new Size(30, 30);
            knappHit.Location = new Point(20, 140);
            knappHit.FlatStyle = FlatStyle.Flat;
            knappHit.FlatAppearance.BorderSize = 0;
            knappHit.BackColor = Color.Transparent;
            knappHit.BackgroundImage = Image.FromFile(@"C:\\Black Jack\bilder\Knappar\hit.png");
            knappHit.BackgroundImageLayout = ImageLayout.Stretch;
            knappHit.FlatAppearance.MouseOverBackColor = Color.Transparent;
            knappHit.FlatAppearance.MouseDownBackColor = Color.Transparent;
            knappHit.BringToFront();
            datorPanel.Controls.Add(knappHit);
            knappHit.MouseClick += new MouseEventHandler(knappHit_click);

            knappPass = new System.Windows.Forms.Button();
            knappPass.Size = new Size(30, 30);
            knappPass.Location = new Point(20, 175);
            knappPass.FlatStyle = FlatStyle.Flat;
            knappPass.FlatAppearance.BorderSize = 0;
            knappPass.BackColor = Color.Transparent;
            knappPass.BackgroundImage = Image.FromFile(@"C:\\Black Jack\bilder\Knappar\pass.png");
            knappPass.BackgroundImageLayout = ImageLayout.Stretch;
            knappPass.FlatAppearance.MouseOverBackColor = Color.Transparent;
            knappPass.FlatAppearance.MouseDownBackColor = Color.Transparent;
            knappPass.BringToFront();
            datorPanel.Controls.Add(knappPass);
            knappPass.MouseClick += new MouseEventHandler(knappPass_click);
            //knappPass.Hide();

            knappDouble = new System.Windows.Forms.Button();
            knappDouble.Size = new Size(30, 30);
            knappDouble.Location = new Point(170, 140);
            knappDouble.FlatStyle = FlatStyle.Flat;
            knappDouble.FlatAppearance.BorderSize = 0;
            knappDouble.BackColor = Color.Transparent;
            knappDouble.BackgroundImage = Image.FromFile(@"C:\\Black Jack\bilder\Knappar\double.png");
            knappDouble.BackgroundImageLayout = ImageLayout.Stretch;
            knappDouble.FlatAppearance.MouseOverBackColor = Color.Transparent;
            knappDouble.FlatAppearance.MouseDownBackColor = Color.Transparent;
            knappDouble.BringToFront();
            datorPanel.Controls.Add(knappDouble);
            knappDouble.MouseClick += new MouseEventHandler(knappDouble_click);
            //knappDouble.Hide();

            knappSplit = new System.Windows.Forms.Button();
            knappSplit.Size = new Size(30, 30);
            knappSplit.Location = new Point(170, 175);
            knappSplit.FlatStyle = FlatStyle.Flat;
            knappSplit.FlatAppearance.BorderSize = 0;
            knappSplit.BackColor = Color.Transparent;
            knappSplit.BackgroundImage = Image.FromFile(@"C:\\Black Jack\bilder\Knappar\split.png");
            knappSplit.BackgroundImageLayout = ImageLayout.Stretch;
            knappSplit.FlatAppearance.MouseOverBackColor = Color.Transparent;
            knappSplit.FlatAppearance.MouseDownBackColor = Color.Transparent;
            knappSplit.BringToFront();
            datorPanel.Controls.Add(knappSplit);
            knappSplit.MouseClick += new MouseEventHandler(knappSplit_click);
            ////knappSplit.Hide();
        }
 
        private void skapaSpelarPaneler()
        {
            int spelarPanelposX = 225; 
            int[] spelarPanelposY = new int[7] {0, 210, 330, 420, 420, 330, 210 };
            int ramPosXskillnad = 35;
            int betPosXskillnad = 35;
            nummer1 = 0;
            
            Random random = new Random();
            spelarNummer = random.Next(1, spelarinfo.antalspelare+1);
            for (int i = 0; i < spelarinfo.antalspelare+1; i++)
            {
                labels = new List<Label>();
                datorLabelLista.Add(labels);
                if (i == 0)
                {
                    pics = new List<PictureBox>();
                    dragnakort.Add(new List<List<PictureBox>>());
                    dragnakort[i].Add(pics);
                    l�ggTillPanel(spelarPanelposX, spelarPanelposY[i], i);
                    l�ggTillKortv�rdeLabel(i, 0, 0);
                }
                else
                {
                    pics = new List<PictureBox>();
                    dragnakort.Add(new List<List<PictureBox>>());
                    dragnakort[i].Add(pics);
                    pics = new List<PictureBox>();
                    dragnakort.Add(new List<List<PictureBox>>());
                    dragnakort[i].Add(pics);

                    l�ggTillPanel(spelarPanelposX, spelarPanelposY[i], i);

                    for (int j = 0; j < 2; j++)
                    {
                        l�ggTillPicturebox(ramPosXskillnad, i, j);
                        ramPosXskillnad += 60;

                        l�ggTillBetLabel(betPosXskillnad, i, j);
                        l�ggTillKortv�rdeLabel(i, j, betPosXskillnad);
                        betPosXskillnad += 60;

                        if (i == spelarNummer)
                        {
                            if (j == 0) DrawFilledRoundedRectangle(ram1, gr�npensel);
                            else DrawFilledRoundedRectangle(ram1, r�dpensel);
                        }
                        else
                        {
                            DrawTranslucentRoundedRectangle(ram1);
                        }
                    }
                    if (i == spelarNummer) nummer1++;
                    
                    l�ggTillInfoLabel(i);
                    nummer1++;

                }
                ramPosXskillnad = 55;
                betPosXskillnad = 55;
                if (i == 0) spelarPanelposX = 120;
                else spelarPanelposX += 235;
            }

        }

        #endregion

        #region bettar
        private void harGjortBet()
        {
            if (vilkensatsat)
            {
                for (int i = 0; i < datorLabelLista[spelarNummer].Count; i++)
                {
                    if ((string)datorLabelLista[spelarNummer][i].Tag == "bet1") datorLabelLista[spelarNummer][i].Text = "$" + markerv�rde.Sum();
                }
            }
            else
            {
                for (int i = 0; i < datorLabelLista[spelarNummer].Count; i++)
                {
                    if ((string)datorLabelLista[spelarNummer][i].Tag == "bet2") datorLabelLista[spelarNummer][i].Text = "$" + markerv�rde1.Sum();
                }
            }
        }

        private void datorBet()
        {
            int balans;
            int bet;
            string x;
            for (int i = 0; i < datorLabelLista.Count; i++)
            {
                if (datorLabelLista[i].Count > 0)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        balans = h�mtaBalans(i);
                        bet = betAI(balans);
                        for (int k = 0; k < datorLabelLista[i].Count; k++)
                        {
                            if ((string)datorLabelLista[i][k].Tag == "bet1" && j == 0)
                            {
                                datorLabelLista[i][k].Text = "$" + bet.ToString();

                            } else if ((string)datorLabelLista[i][k].Tag == "bet2" && j == 1)
                            {
                                datorLabelLista[i][k].Text = "$" + bet.ToString();
                            }
                            if ((string) datorLabelLista[i][k].Tag == "spelarinfo" && j == 0)
                            {
                                datorLabelLista[i][k].Text = datorLabelLista[i][k].Text.Replace(datorLabelLista[i][k].Text.Substring(9), (balans - bet).ToString());
                            } else if ((string)datorLabelLista[i][k].Tag == "spelarinfo" && j == 1)
                            {
                                datorLabelLista[i][k].Text = datorLabelLista[i][k].Text.Replace(datorLabelLista[i][k].Text.Substring(9), (balans - bet).ToString());

                            }
                        }
                    }
                }
            }
        }

        private int betAI(int balans)
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
                return balans;
            } else if (bet < 50)
            {
                return balans;
            } else
            {
                bet1 = Convert.ToInt32(bet);
                bet1 = (bet1 / 5) * 5;
            }
            return bet1;
        }

        private int h�mtaBalans(int i)
        {
            int balans = 0;
            string x = "";
            for (int j = 0; j < datorLabelLista[i].Count; j++)
            {
                if ((string)datorLabelLista[i][j].Tag == "spelarinfo")
                {
                    x = datorLabelLista[i][j].Text.Substring(9);
                }
            }
            int.TryParse(x, out balans);
            return balans;
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
            int posX = 55;
            int posY = 140;
            int x = 0;
            if (markerv�rde.Sum() > 0)
            {

                string genv�g;
                Random rnd = new Random();
                int draKort;
                if (!p�g�endeRunda)
                {
                    //spelaLjud(@"C:\\Black Jack\Audio\lekblandas.wav");
                    resetKort();
                    nyKortlek = kortlek.Nykortlek();
                    for (int h = 0; h < (spelarinfo.antalspelare + 1); h++)
                    {
                        posX = 55;
                        posY = 140;
                        kortv�rdeDator.Add(new List<List<int>>());


                        if (h == 0)
                        {
                            kortlista = new List<int>();
                            kortv�rdeDator[h].Add(kortlista);
                            //spelaLjud(@"C:\\Black Jack\Audio\kortspelas.wav");
                            draKort = rnd.Next(nyKortlek.Count);
                            kortv�rdeDator[h][0].Add(kortlek.h�mtaKortv�rde(nyKortlek[draKort])); //L�gger till kortv�rdet av det dragna kortet
                            kortv�rdeTotaltDator.Add(kortlek.ber�knaKortv�rde(kortv�rdeDator[0][0])); //H�mtar summan av alla kort
                            h�mtaSpelkortsBilder(draKort,0, 0, 0, 128); //Laddar listan med Picturebox
                        }
                        else
                        {
                            for (int i = 0; i < 2; i++) //Loopar korth�g
                            {
                                for (int j = 0; j < 2; j++) //Delar ut tv� kort i korth�gen
                                {
                                    //spelaLjud(@"C:\\Black Jack\Audio\kortspelas.wav");
                                    draKort = rnd.Next(nyKortlek.Count);
                                    kortlista = new List<int>();
                                    kortv�rdeDator[h].Add(kortlista);
                                    kortv�rdeDator[h][i].Add(kortlek.h�mtaKortv�rde(nyKortlek[draKort])); //L�gger till kortv�rdet av det dragna kortet

                                    h�mtaSpelkortsBilder(draKort, h, i, posX, posY); //Laddar listan med Picturebox
                                    nyKortlek.RemoveAt(draKort);
                                    //await Task.Delay(50);
                                    posY -= 15;
                                }
                                posY = 140;
                                posX += 55;
                            }
                        }
                    }

                    visaF�rstaTv�Spelkort();
                    knappDouble.Show();
                    knappPass.Show();
                    p�g�endeRunda = true;
                }

            }
            else
            {
                MessageBox.Show("Du m�ste satsa n�got", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void split()
        {
            int x = 0;
            foreach (Control c in panelLista[spelarNummer].Controls) 
            {
                //Loopa med vilkenspelare, VellerH best�mmer vilken h�g - j�mf�r tagg (special f�r spelaren)
                //L�gg till ny pictureboxlist o dragnakort, flytta ett kort till nya h�g (insert first list/last) o ta bort fr�n gamla listan
                //L�gg till ny label i datorlabellista, flytta label och skapa ny f�r nya h�g, kolla balans och uppdatera info (insert first /last)
                if (c is PictureBox)
                {
                    if (x == 0)
                    {
                        dragnakort[spelarNummer][x][0].Location = new Point(dragnakort[spelarNummer][x][0].Location.X, dragnakort[spelarNummer][x][0].Location.Y-92);
                        pics = new List<PictureBox>();
                        dragnakort[spelarNummer].Add(pics);
                        dragnakort[spelarNummer][x][1].Location = new Point(dragnakort[spelarNummer][x][0].Location.X - 55, dragnakort[spelarNummer][x][0].Location.Y);
                        label1.Text = dragnakort[spelarNummer].Count.ToString(); 
                        dragnakort[spelarNummer][2].Add(dragnakort[spelarNummer][x][1]);
                        dragnakort[spelarNummer][x].RemoveAt(1);


                        //datorLabelLista[spelarNummer].Add(); Flytta betlabel och skapa en ny f�r nya bilden samt kolla att spelaren har tillr�cklig balans
                        

                    }
                    x++;
                }
            }
        }

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
            //kortv�rdeBank = 0;
            kortv�rdeSpelare.Clear();
            kortv�rdeDator.Clear();
            spelarListaKort.Clear();
            //bankkort.Clear();
            //spelarkort.Clear();
            //bankListaKort.Clear();
            nyKortlek.Clear();
        }

        #endregion



        #region l�ggtillbilder

        private void visaF�rstaTv�Spelkort()
        {
            foreach (Panel panel in panelLista)
            {
                foreach (Control c in panel.Controls)
                {
                    if (c is PictureBox)
                    {
                        panel.Controls.Remove(c);
                        c.Dispose();
                    }
                }
            }
            panelLista[0].Controls.Add(dragnakort[0][0][0]); //Bankens kort
            int x;
            for (int h = 1; h < panelLista.Count; h++) 
            {
                for (int i = 0; i < dragnakort[h].Count; i++)
                {
                    for (int j= 0; j < dragnakort[h][i].Count; j++)    
                    {
                        panelLista[h].Controls.Add(dragnakort[h][i][j]);
                    }
                }
            }

            for (int k = 0; k < datorLabelLista.Count; k++)
            {
                if (k == 0)
                {
                    for (int l = 0; l < datorLabelLista[k].Count; l++)
                    {
                        if ((string)datorLabelLista[k][l].Tag == "kort1")
                        {
                            datorLabelLista[k][l].Text = kortlek.ber�knaKortv�rde(kortv�rdeDator[0][0]).ToString();
                        }
                    }
                }
                else
                {
                    for (int l = 0; l < datorLabelLista[k].Count; l++)
                    {
                        if ((string)datorLabelLista[k][l].Tag == "kort1")
                        {
                            datorLabelLista[k][l].Text = kortlek.ber�knaKortv�rde(kortv�rdeDator[k][0]).ToString();
                            datorLabelLista[k][l].Location = new Point(datorLabelLista[k][l].Location.X, datorLabelLista[k][l].Location.Y - 15);
                        }
                        else if ((string)datorLabelLista[k][l].Tag == "kort2")
                        {
                            datorLabelLista[k][l].Text = kortlek.ber�knaKortv�rde(kortv�rdeDator[k][1]).ToString();
                            datorLabelLista[k][l].Location = new Point(datorLabelLista[k][l].Location.X, datorLabelLista[k][l].Location.Y - 15);
                        }
                    }
                }
            }
        }

        private void h�mtaSpelkortsBilder(int x, int i, int j, int posX, int posY)
        {
            PictureBox kort = new PictureBox();
            kort.Size = new Size(50, 72);
            kort.Location = new Point(posX, posY);
            kort.BringToFront();
            string genv�g = @"C:\\Black Jack\bilder\Spelkort\" + nyKortlek[x] + ".png";
            ritaBild = Image.FromFile(genv�g);
            kort.Image = ritaBild;
            dragnakort[i][j].Add(kort);
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
                if (vilkensatsat)
                {
                    if (antalMarker > 0)
                    {
                        markerv�rde = marker.raderaMarker(markerv�rde, 5);
                        harGjortBet();
                    }
                    else
                    {
                    }
                }
                else
                {
                    if (antalMarker > 0)
                    {
                        markerv�rde1 = marker.raderaMarker(markerv�rde1, 5);
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
                        if (antalMarker > 5)
                        {
                            markerv�rde.Add(5);
                            markerv�rde = marker.sortera(markerv�rde);
                            harGjortBet();
                        }
                        else
                        {
                            MessageBox.Show("Du har inte tillr�ckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        if (antalMarker > 5)
                        {
                            markerv�rde1.Add(5);
                            markerv�rde1 = marker.sortera(markerv�rde1);
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
                        markerv�rde.Clear();
                        harGjortBet();
                    }
                    else
                    {
                        markerv�rde = marker.raderaMarker(markerv�rde, 10);
                        harGjortBet();
                    }
                }
                else
                {
                    if (10 > markerv�rde.Sum())
                    {
                        markerv�rde1.Clear();
                        harGjortBet();

                    }
                    else
                    {
                        markerv�rde1 = marker.raderaMarker(markerv�rde1, 10);
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
                        if (antalMarker >= 10)
                        {
                            markerv�rde.Add(10);
                            markerv�rde = marker.sortera(markerv�rde);
                            harGjortBet();
                        }
                        else
                        {
                            MessageBox.Show("Du har inte tillr�ckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        if (antalMarker >= 10)
                        {
                            markerv�rde1.Add(10);
                            markerv�rde1 = marker.sortera(markerv�rde1);
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
                        markerv�rde.Clear();
                        harGjortBet();

                    }
                    else
                    {
                        markerv�rde = marker.raderaMarker(markerv�rde, 50);
                        harGjortBet();
                    }
                }
                else
                {
                    if (50 > markerv�rde1.Sum())
                    {
                        markerv�rde1.Clear();
                        harGjortBet();

                    }
                    else
                    {
                        markerv�rde1 = marker.raderaMarker(markerv�rde1, 50);
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
                        if (antalMarker >= 50)
                        {
                            markerv�rde.Add(50);
                            markerv�rde = marker.sortera(markerv�rde);
                            harGjortBet();
                        }
                        else
                        {
                            MessageBox.Show("Du har inte tillr�ckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        if (antalMarker >= 50)
                        {
                            markerv�rde1.Add(50);
                            markerv�rde1 = marker.sortera(markerv�rde1);
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
                        markerv�rde.Clear();
                        harGjortBet();

                    }
                    else
                    {
                        markerv�rde = marker.raderaMarker(markerv�rde, 100);
                        harGjortBet();
                    }
                }
                else
                {
                    if (100 > markerv�rde1.Sum())
                    {
                        markerv�rde1.Clear();
                        harGjortBet();

                    }
                    else
                    {
                        markerv�rde1 = marker.raderaMarker(markerv�rde1, 100);
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
                        if (antalMarker >= 100)
                        {
                            markerv�rde.Add(100);
                            markerv�rde = marker.sortera(markerv�rde);
                            harGjortBet();
                        }
                        else
                        {
                            MessageBox.Show("Du har inte tillr�ckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        if (antalMarker >= 100)
                        {
                            markerv�rde1.Add(100);
                            markerv�rde1 = marker.sortera(markerv�rde1);
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
                        markerv�rde.Clear();
                        harGjortBet();

                    }
                    else
                    {
                        markerv�rde = marker.raderaMarker(markerv�rde, 500);
                        harGjortBet();
                    }
                }
                else
                {
                    if (500 > markerv�rde1.Sum())
                    {
                        markerv�rde1.Clear();
                        harGjortBet();

                    }
                    else
                    {
                        markerv�rde1 = marker.raderaMarker(markerv�rde1, 500);
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
                        if (antalMarker >= 500)
                        {
                            markerv�rde.Add(500);
                            markerv�rde = marker.sortera(markerv�rde);
                            harGjortBet();
                        }
                        else
                        {
                            MessageBox.Show("Du har inte tillr�ckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        if (antalMarker >= 500)
                        {
                            markerv�rde1.Add(500);
                            markerv�rde1 = marker.sortera(markerv�rde1);
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
                        markerv�rde.Clear();
                        harGjortBet();

                    }
                    else
                    {
                        markerv�rde = marker.raderaMarker(markerv�rde, 1000);
                        harGjortBet();
                    }
                }
                else
                {
                    if (1000 > markerv�rde1.Sum())
                    {
                        markerv�rde1.Clear();
                        harGjortBet();

                    }
                    else
                    {
                        markerv�rde1 = marker.raderaMarker(markerv�rde1, 1000);
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
                        if (antalMarker >= 1000)
                        {
                            markerv�rde.Add(1000);
                            markerv�rde = marker.sortera(markerv�rde);
                            harGjortBet();
                        }
                        else
                        {
                            MessageBox.Show("Du har inte tillr�ckligt med marker", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        if (antalMarker >= 1000)
                        {
                            markerv�rde1.Add(1000);
                            markerv�rde1 = marker.sortera(markerv�rde1);
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
            //datorBet();
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
            spelarebet *= 2;
            visaspelarebet.Text = "$" + spelarebet;
            knappDouble.Hide();
            knappPass.Hide();
            spelaKort();
            passa();
        }

        private void knappSplit_click(object sender, MouseEventArgs e)
        {
            split();
        }

        private void ram_click(object sender, EventArgs e)
        {
            PictureBox klickadRam = sender as PictureBox;
            Panel panel = klickadRam.Parent as Panel;
            List<PictureBox> bilderIram = new List<PictureBox>();
            bilderIram.Add(klickadRam);
            foreach (Control c in panel.Controls)
            {
                if (c is PictureBox) {
                    PictureBox andraRam = (PictureBox)c;
                    if ((bool)andraRam.Tag == true) 
                    {
                        if (vilkensatsat) kollaKryssV = kollaKryss;
                        else kollaKryssH = kollaKryss;
                        bool x = (bool)andraRam.Tag;
                        x = !x;
                        andraRam.Tag = x;
                        bilderIram.Add(andraRam);
                    }
                }
            }
            if (vilkensatsat) kollaKryss = kollaKryssH;
            else kollaKryss = kollaKryssV;
            klickadRam.Tag = true;
            vilkensatsat = !vilkensatsat;
            maxBet();
            DrawFilledRoundedRectangle(bilderIram[0], gr�npensel);
            DrawFilledRoundedRectangle(bilderIram[1], r�dpensel);
        }


        #endregion



    }

}