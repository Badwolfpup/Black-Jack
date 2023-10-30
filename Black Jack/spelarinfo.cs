using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Black_Jack
{
    public static class spelarinfo
    {
        public static string spelare { get; set; }
        public static int antalspelare { get; set; }

        public static int kontobalans { get; set; }

        public static string spelarnamn { get; set; }

        public static int spelarNummer { get; set; }
        static spelarinfo()
        {
            Random random = new Random();
            spelarNummer = random.Next(1, antalspelare + 1);
            
        }

        public static void spelarNamnMarker()
        {

            string namn = "";
            string marker = "";
            bool namnellermarker = true;
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
                spelarnamn = namn;
                int x = 0;
                int.TryParse(marker, out x);
                kontobalans = x;
            }
            catch (Exception e)
            {
                if (spelarnamn == "")
                {
                    Environment.Exit(0);
                }
            }


        }
    }
}
