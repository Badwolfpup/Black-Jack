using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Black_Jack
{
	public static class Kortlek //Klass som hanterar kortleken
	{
		public static List<string> nykortlek { get; set; }

		static Kortlek()
        {
			nykortlek = new List<string>();
		}
		public static List<string> skapaNykortlek() //Blandar en ny kortlek, 4 kortlekar tjock. Varje kort sparas med med två tecken - färg + varlör
		{			
			char[] färger = new char[4] { 'H', 'S', 'R', 'K' }; //Representerar färgerna på kortern (H=hjärter, S=spader, R=ruter, K=klöver)
			for (int h = 0; h < 4; h++)
			{
				for (int i = 0; i < 4; i++)
				{
					for (int j = 2; j < 15; j++)
					{
						if (j < 10)
						{
							string kort = färger[i].ToString() + j.ToString();
							nykortlek.Add(kort);
						}
						else if (j == 10)
						{
							string kort = färger[i].ToString() + "T"; //Representerar 10 med T
							nykortlek.Add(kort);
						}
						else if (j == 11)
						{
							string kort = färger[i].ToString() + "J"; //Representerar knekt med J
                            nykortlek.Add(kort);
						}
						else if (j == 12)
						{
							string kort = färger[i].ToString() + "Q"; //Representerar dam med Q
                            nykortlek.Add(kort);
						}
						else if (j == 13)
						{
							string kort = färger[i].ToString() + "K"; //Representerar kung med K
                            nykortlek.Add(kort);
						}
						else
						{
							string kort = färger[i].ToString() + "A"; //Representerar ess med A
                            nykortlek.Add(kort);
						}
					}
				}
			}
			return nykortlek;
		}

		public static int hämtaKortvärde(string kort) //Tar fram det numeriska värdet av varje draget kort
		{
			int värde = 0;
			if (!Char.IsLetter(kort[1])) //Kollar om värdet representeras av en siffra
			{
				string y = (kort[1]).ToString();
				int.TryParse(y, out värde);
			}
			else if (kort[1] == 'A') //Kollar om värdet representeras av A
            {
				värde = 11;
			}
			else värde = 10; //Kollar om värdet representeras av INTE siffra eller A (10-kung)

            return värde;
		}

		public static int beräknaKortvärde(List<int> kort) //Räknar ut det totala värdet av de dragna korten
		{
			int värde = 0 ;
			kort.Sort();
			foreach (int i in kort)
			{
				if (i == 11) //Kollar om kortet är ett ess
				{
					if (värde + 11 > 21) //Omvandlar 11 till 1 om man annars skulle gå bust
                    {
						värde += 1; 
					}
					else värde += 11;

				} else värde += i;
			}
			return värde;
		}


	}
}
