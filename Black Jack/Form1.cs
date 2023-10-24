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
        Kortlek kortlek = new Kortlek();
        List<string> nyKortlek = new List<string>();
        List<int> markerv�rde = new List<int>(); //Hur m�nga marker spelare har satsat
        List<int> markerv�rde1 = new List<int>(); // Hur m�nga marker spelare har satsat p� andra
        List<int> spelarListaKort = new List<int>();
        List<PictureBox> bankkort = new List<PictureBox>();
        List<PictureBox> spelarkort = new List<PictureBox>();
        List<List<PictureBox>> dragnakort = new List<List<PictureBox>>(); //Lista �ver dragna kort f�r varje spelare
        List<List<int>> kortv�rdeDator = new List<List<int>>(); //H�ller stringv�rdet f�r varje kort f�r datorn
        List<int> dator; //Initierar listan i kortv�rdeDator
        List<int> kortv�rdeTotaltDator = new List<int>(); //H�ller det totala kortv�rdet f�r datorns kort
        List<Panel> panelLista = new List<Panel>(); //H�ller lista �ver panel f�r alla
        List<List<Label>> datorLabelLista = new List<List<Label>>(); //H�ller lista �ver alla labels f�r varje panel f�r datorspelare
        List<Label> spelarLabel = new List<Label>(); //H�ller lista �ver alla labels f�r spelaren
        List<Label> labels; //Listan f�r den enskilde datorpanelen
        List<PictureBox> pics; //Lista �ver vilka kort datorn har
        bool[] kollaKryss = new bool[] { false, false, false, false, false, false }; //True om spelarens inte har tillr�ckligt med marker f�r att betta en vissa val�r
        List<int> kortv�rdeSpelare = new List<int>(); //H�ller stringv�rdet f�r varje kort f�r spelaren
        int kortv�rdeTotaltSpelare; //H�ller det totala kortv�rdet f�r spelarens kort
        int antalMarker = 5000; //Hur m�nga marker spelaren har
        int spelarebet;
        int nummer1;
        int spelarNummer;
        bool p�g�endeRunda = false;
        bool spelaresTur = true;
        bool vilkensatsat; //H�lla koll p� om v�nster eller h�ger bet, true = v�nster
        Control betLabel;
        Image ritaBild;
        List<Tuple<Image, Point>> imagesToDraw = new List<Tuple<Image, Point>>();
        private Panel bankkortPanel;
        private Panel datorPanel;
        Panel spelarensPlats = new Panel();
        PictureBox dollar5, dollar10, dollar50, dollar100, dollar500, dollar1000;
        System.Windows.Forms.Button knappHit, knappPass, knappDouble, knappSplit;
        PictureBox ram1;
        Marker marker = new Marker();
        Label totalBet = new Label();
        Label betInfo = new Label();
        Label bankrulle = new Label();
        Label visaspelarebet;
        Label visaspelareinfo;
        string spelarNamn = "";
        login loggain;
        System.Windows.Forms.ToolTip hitTooltip;
        System.Windows.Forms.ToolTip passTooltip;
        System.Windows.Forms.ToolTip doubleTooltip;
        System.Windows.Forms.ToolTip splitTooltip;
        System.Windows.Forms.ToolTip insuranceTooltip;
        SolidBrush r�dpensel = new SolidBrush(Color.IndianRed);
        SolidBrush gr�npensel = new SolidBrush(Color.LightGreen);
        int nummer = 0;
        double mitten;
        int f�rsta;
        int v�nster;
        int h�ger;



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

            this.Size = new System.Drawing.Size(1440, 800);
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

            totalBet.Location = new Point(this.Width / 2 - 40, this.Height / 2 + 85);
            totalBet.Font = new Font("MS Gothic", 14, FontStyle.Bold);
            totalBet.BackColor = Color.Transparent;
            totalBet.AutoSize = true;
            this.Controls.Add(totalBet);
            totalBet.BringToFront();





            skapaSpelarPaneler();
            skapaMarkerBilder();
            tooltips();
            datorBet();

        }

        #region l�ggtillcontrols
        private void l�ggTillBankPanel()
        {
            pics = new List<PictureBox>();
            dragnakort.Add(pics);
            labels = new List<Label>();
            datorLabelLista.Add(labels);
            bankkortPanel = new Panel();
            bankkortPanel.Size = new Size(50, 72);
            bankkortPanel.Location = new Point(this.Width / 2 - 50, this.Height / 2 - 185);
            bankkortPanel.BackColor = Color.Transparent;
            this.Controls.Add(bankkortPanel);
            panelLista.Add(bankkortPanel);
        }

        private void l�ggTillPanel(int VspelarPanelposX, int VspelarPanelposY, int spelarnummer, int i)
        {
            datorPanel = new Panel();
            datorPanel.Size = new Size(180, 270);
            datorPanel.Location = new Point(VspelarPanelposX, VspelarPanelposY);
            datorPanel.BackColor = Color.Transparent;
            datorPanel.Tag = true; //Bet p� f�rsta
            if (spelarnummer == i)
            {
                spelarensPlats = datorPanel;
                this.Controls.Add(spelarensPlats);
                panelLista.Add(spelarensPlats);
                l�ggTillKnapp();
            }
            else
            {
                this.Controls.Add(datorPanel);
                panelLista.Add(datorPanel);
            }
        }


        private void l�ggTillPicturebox(int ramPosXskillnad, int i)
        {
            ram1 = new PictureBox();
            ram1.Size = new Size(50, 72);
            ram1.Location = new Point(ramPosXskillnad, 140);
            ram1.BackColor = Color.Transparent;
            ram1.SizeMode = PictureBoxSizeMode.StretchImage;
            datorPanel.Controls.Add(ram1);
            if (spelarNummer == i) ram1.Click += new EventHandler(ram_click);
        }

        private void l�ggTillBetLabel(int betPosXskillnad, int i)
        {
            visaspelarebet = new Label();
            visaspelarebet.Location = new Point(betPosXskillnad, 217);
            visaspelarebet.Font = new Font("MS Gothic", 12, FontStyle.Bold);
            visaspelarebet.BackColor = Color.LightGray;
            visaspelarebet.AutoSize = true;        
            visaspelarebet.BringToFront();
            visaspelarebet.Text = "$0";
            if (i == spelarNummer)
            {
                datorPanel.Controls.Add(visaspelarebet);
                spelarLabel.Add(visaspelarebet);
            }
            else
            {
                datorPanel.Controls.Add(visaspelarebet);
                datorLabelLista[nummer1].Add(visaspelarebet);
            }
        }

        private void l�ggTillSpelarinfoLabel()
        {
            visaspelareinfo = new Label();
            visaspelareinfo.Location = new Point(35, 240);
            visaspelareinfo.Font = new Font("MS Gothic", 12, FontStyle.Bold);
            visaspelareinfo.BackColor = Color.LightGray;
            visaspelareinfo.AutoSize = true;
            visaspelareinfo.BringToFront();
            visaspelareinfo.Text = spelarNamn + ": $" + antalMarker;
            datorPanel.Controls.Add(visaspelareinfo);
            spelarLabel.Add(visaspelareinfo);
        }

        private void l�ggTillDatorinfoLabel(int spelarpos)
        {
            visaspelareinfo = new Label();
            visaspelareinfo.Location = new Point(35, 240);
            visaspelareinfo.Font = new Font("MS Gothic", 12, FontStyle.Bold);
            visaspelareinfo.BackColor = Color.LightGray;
            visaspelareinfo.AutoSize = true;
            visaspelareinfo.BringToFront();
            visaspelareinfo.Text = "Dator" + (spelarpos+1) + ": $5000";
            datorPanel.Controls.Add(visaspelareinfo);
            datorLabelLista[spelarpos].Add(visaspelareinfo);
            
        }

        private void l�ggTillKnapp()
        {
            knappHit = new System.Windows.Forms.Button();
            knappHit.Size = new Size(30, 30);
            knappHit.Location = new Point(0, 140);
            knappHit.FlatStyle = FlatStyle.Flat;
            knappHit.FlatAppearance.BorderSize = 0;
            knappHit.BackColor = Color.Transparent;
            knappHit.BackgroundImage = Image.FromFile(@"C:\\Black Jack\bilder\Knappar\hit.png");
            knappHit.BackgroundImageLayout = ImageLayout.Stretch;
            knappHit.FlatAppearance.MouseOverBackColor = Color.Transparent;
            knappHit.FlatAppearance.MouseDownBackColor = Color.Transparent;
            knappHit.BringToFront();
            spelarensPlats.Controls.Add(knappHit);
            knappHit.MouseClick += new MouseEventHandler(knappHit_click);

            knappPass = new System.Windows.Forms.Button();
            knappPass.Size = new Size(30, 30);
            knappPass.Location = new Point(0, 175);
            knappPass.FlatStyle = FlatStyle.Flat;
            knappPass.FlatAppearance.BorderSize = 0;
            knappPass.BackColor = Color.Transparent;
            knappPass.BackgroundImage = Image.FromFile(@"C:\\Black Jack\bilder\Knappar\pass.png");
            knappPass.BackgroundImageLayout = ImageLayout.Stretch;
            knappPass.FlatAppearance.MouseOverBackColor = Color.Transparent;
            knappPass.FlatAppearance.MouseDownBackColor = Color.Transparent;
            knappPass.BringToFront();
            spelarensPlats.Controls.Add(knappPass);
            knappPass.MouseClick += new MouseEventHandler(knappPass_click);
            //knappPass.Hide();

            knappDouble = new System.Windows.Forms.Button();
            knappDouble.Size = new Size(30, 30);
            knappDouble.Location = new Point(150, 140);
            knappDouble.FlatStyle = FlatStyle.Flat;
            knappDouble.FlatAppearance.BorderSize = 0;
            knappDouble.BackColor = Color.Transparent;
            knappDouble.BackgroundImage = Image.FromFile(@"C:\\Black Jack\bilder\Knappar\double.png");
            knappDouble.BackgroundImageLayout = ImageLayout.Stretch;
            knappDouble.FlatAppearance.MouseOverBackColor = Color.Transparent;
            knappDouble.FlatAppearance.MouseDownBackColor = Color.Transparent;
            knappDouble.BringToFront();
            spelarensPlats.Controls.Add(knappDouble);
            knappDouble.MouseClick += new MouseEventHandler(knappDouble_click);
            //knappDouble.Hide();

            knappSplit = new System.Windows.Forms.Button();
            knappSplit.Size = new Size(30, 30);
            knappSplit.Location = new Point(150, 175);
            knappSplit.FlatStyle = FlatStyle.Flat;
            knappSplit.FlatAppearance.BorderSize = 0;
            knappSplit.BackColor = Color.Transparent;
            knappSplit.BackgroundImage = Image.FromFile(@"C:\\Black Jack\bilder\Knappar\split.png");
            knappSplit.BackgroundImageLayout = ImageLayout.Stretch;
            knappSplit.FlatAppearance.MouseOverBackColor = Color.Transparent;
            knappSplit.FlatAppearance.MouseDownBackColor = Color.Transparent;
            knappSplit.BringToFront();
            spelarensPlats.Controls.Add(knappSplit);
            knappSplit.MouseClick += new MouseEventHandler(knappSplit_click);
            //knappSplit.Hide();
        }
 
        private void skapaSpelarPaneler()
        {
            int spelarPanelposX = 110; 
            int[] spelarPanelposY = new int[7] { 210, 330, 360, 385, 360, 330, 210 };
            int ramPosXskillnad = 35;
            int betPosXskillnad = 35;
            nummer1 = 0;
            l�ggTillBankPanel();
            Random random = new Random();
            spelarNummer = random.Next(spelarinfo.antalspelare);
            for (int i = 1; i < spelarinfo.antalspelare; i++)
            {
                labels = new List<Label>();
                datorLabelLista.Add(labels);
                labels = new List<Label>();
                datorLabelLista.Add(labels);
                pics = new List<PictureBox>();
                dragnakort.Add(pics);
                pics = new List<PictureBox>();
                dragnakort.Add(pics);

                l�ggTillPanel(spelarPanelposX, spelarPanelposY[i], spelarNummer, i);

                for (int j = 0; j < 2; j++)
                {
                    l�ggTillPicturebox(ramPosXskillnad, i);
                    ramPosXskillnad += 60;

                    l�ggTillBetLabel(betPosXskillnad, i);
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
                if (i != spelarNummer)
                {
                    l�ggTillDatorinfoLabel(nummer1);
                    nummer1++;
                }
                else l�ggTillSpelarinfoLabel();

                ramPosXskillnad = 35;
                betPosXskillnad = 35;
                spelarPanelposX += 180;
            }

        }

        #endregion

        #region bettar
        private void harGjortBet()
        {
            vilkensatsat = (bool)spelarensPlats.Tag;

            if (vilkensatsat)
            {
                spelarLabel[0].Text = "$" + markerv�rde.Sum();
            }
            else
            {
                spelarLabel[1].Text = "$" + markerv�rde1.Sum();
            }
        }

        private void datorBet()
        {
            int balans;
            int bet;
            string x;
            label1.Text = "";
            for (int i = 0; i < datorLabelLista.Count; i++)
            {
                if (datorLabelLista[i].Count > 0)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        balans = h�mtaBalans(i);
                        bet = betAI(balans);
                        datorLabelLista[i][j].Text = "$" + bet.ToString();
                        datorLabelLista[i][2].Text = datorLabelLista[i][2].Text.Replace(datorLabelLista[i][2].Text.Substring(9), (balans - bet).ToString());
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
            string x;
            x = datorLabelLista[0].Count.ToString() + datorLabelLista[1].Count.ToString() + datorLabelLista[2].Count.ToString();
            //label1.Text = datorLabelLista[0][0].Text + datorLabelLista[0][1].Text + datorLabelLista[0][2].Text + datorLabelLista[1][0].Text + datorLabelLista[1][1].Text + datorLabelLista[1][2].Text;
            x = datorLabelLista[i][2].Text.Substring(9);
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

        private void visaBankKort()
        {

        }

        private void laddaKort()
        {

        }



        #region spellogik
        private async Task spelaKort()
        {
            int posX = 35;
            int posY = 140;
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
                        posX = 35;
                        posY = 140;
                        List<int> dator = new List<int>();
                        kortv�rdeDator.Add(dator);
                        if (h == 0)
                        {
                            //spelaLjud(@"C:\\Black Jack\Audio\kortspelas.wav");
                            draKort = rnd.Next(nyKortlek.Count);
                            kortv�rdeDator[h].Add(kortlek.h�mtaKortv�rde(nyKortlek[draKort])); //L�gger till kortv�rdet av det dragna kortet
                            kortv�rdeTotaltDator.Add(kortlek.ber�knaKortv�rde(kortv�rdeDator[h])); //H�mtar summan av alla kort
                            h�mtaSpelkortsBilder(draKort, 0, 0, 0); //Laddar listan med Picturebox

                        }
                        else
                        {

                            for (int i = 0; i < 2; i++)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    //spelaLjud(@"C:\\Black Jack\Audio\kortspelas.wav");
                                    draKort = rnd.Next(nyKortlek.Count);
                                    if (h == spelarNummer)
                                    {
                                        kortv�rdeSpelare.Add(kortlek.ber�knaKortv�rde(spelarListaKort)); //L�gger till kortv�rdet av det dragna kortet
                                        kortv�rdeTotaltSpelare = kortlek.ber�knaKortv�rde(kortv�rdeSpelare); //H�mtar summan av alla kort
                                    }
                                    else
                                    {
                                        kortv�rdeDator[h].Add(kortlek.h�mtaKortv�rde(nyKortlek[draKort])); //L�gger till kortv�rdet av det dragna kortet
                                        kortv�rdeTotaltDator.Add(kortlek.ber�knaKortv�rde(kortv�rdeDator[h])); //H�mtar summan av alla kort
                                    }
                                    h�mtaSpelkortsBilder(draKort, h, posX, posY); //Laddar listan med Picturebox
                                    
                                    nyKortlek.RemoveAt(draKort);
                                    //await Task.Delay(50);
                                    posY -= 15;
                                }
                                
                                posY = 140;
                                posX += 60;
                            }
                        }
                    }
                    visaF�rstaTv�Spelkort();
                    knappDouble.Show();
                    knappPass.Show();
                    p�g�endeRunda = true;
                }
                //else if (spelaresTur)
                //{
                //    spelaLjud(@"C:\\Black Jack\Audio\kortspelas.wav");
                //    draKort = rnd.Next(nyKortlek.Count);
                //    spelarListaKort.Add(kortlek.h�mtaKortv�rde(nyKortlek[draKort]));
                //    kortv�rdeSpelare = kortlek.ber�knaKortv�rde(spelarListaKort);
                //    skapaSpelarkortsBilder(draKort);
                //    visaSpelarKort(draKort);
                //    nyKortlek.RemoveAt(draKort);
                //    await Task.Delay(500);

                //    //kollaVinnare();
                //}
                //else
                //{
                //    spelaLjud(@"C:\\Black Jack\Audio\kortspelas.wav");
                //    draKort = rnd.Next(nyKortlek.Count);
                //    bankListaKort.Add(kortlek.h�mtaKortv�rde(nyKortlek[draKort]));
                //    kortv�rdeBank = kortlek.ber�knaKortv�rde(bankListaKort);
                //    skapaBankkortsBilder(draKort);
                //    visaBankKort(); ;
                //    nyKortlek.RemoveAt(draKort);
                //    await Task.Delay(500);
                //    kollaVinnare();

                //}
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
            //kortv�rdeBank = 0;
            kortv�rdeSpelare.Clear();
            kortv�rdeDator.Clear();
            spelarListaKort.Clear();
            bankkort.Clear();
            spelarkort.Clear();
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
                    panel.Controls.Remove(c);
                    c.Dispose();
                }
            }
            label1.Text = "";
            panelLista[0].Controls.Add(dragnakort[0][0]); //Bankens kort
            int x;
            for (int i = 1; i < panelLista.Count; i++) 
            {
                x = 1;
                for (int j = dragnakort[i].Count -1; j > -1; j--)
                {
                    
                    panelLista[i].Controls.Add(dragnakort[x][j]);
                    panelLista[i].Controls.Add(dragnakort[(x + 1)][j]);
                    x += 2;
                    //if (i == 0)
                    //{
                    //    panelLista[i].Controls.Add(dragnakort[i][j]);
                    //    panelLista[i].Controls.Add(dragnakort[(i + 1)][j]);
                    //}
                    //else
                    //{
                    //    panelLista[i].Controls.Add(dragnakort[(i*2)][j]);
                    //    panelLista[i].Controls.Add(dragnakort[((i * 2) +1)][j]);
                    //}
                }
            }
        }

        private void h�mtaSpelkortsBilder(int x, int i, int posX, int posY)
        {
            PictureBox kort = new PictureBox();
            kort.Size = new Size(50, 72);
            kort.Location = new Point(posX, posY);
            kort.BringToFront();
            string genv�g = @"C:\\Black Jack\bilder\Spelkort\" + nyKortlek[x] + ".png";
            ritaBild = Image.FromFile(genv�g);
            kort.Image = ritaBild;
            dragnakort[i].Add(kort);
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
            vilkensatsat = (bool)spelarensPlats.Tag;
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
            vilkensatsat = (bool)spelarensPlats.Tag;
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
            vilkensatsat = (bool)spelarensPlats.Tag;
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
            vilkensatsat = (bool)spelarensPlats.Tag;
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
            vilkensatsat = (bool)spelarensPlats.Tag;
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
            vilkensatsat = (bool)spelarensPlats.Tag;
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
            //spelarebet = markerv�rde.Sum();
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

        }

        private void ram_click(object sender, EventArgs e)
        {
            bool y = (bool)spelarensPlats.Tag;
            PictureBox klickadRam = sender as PictureBox;
            List<PictureBox> bilderIram = new List<PictureBox>();
            bilderIram.Add(klickadRam);
            int x = 0;
            foreach (Control c in spelarensPlats.Controls)
            {
                if (c is PictureBox && c != klickadRam)
                {
                    y = !y;
                    spelarensPlats.Tag = y;
                    bilderIram.Add((PictureBox)c);
                    if (x == 3) betLabel = spelarensPlats.Controls[1];
                    else betLabel = spelarensPlats.Controls[0];
                }
                x++;
            }

            DrawFilledRoundedRectangle(bilderIram[0], gr�npensel);
            DrawFilledRoundedRectangle(bilderIram[1], r�dpensel);
        }


        #endregion



    }

}