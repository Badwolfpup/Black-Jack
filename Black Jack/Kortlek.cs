using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Black_Jack
{
	public static class Kortlek
	{

		public static List<string> Nykortlek()
		{
			List<string> kortlek = new List<string>();
			char[] färger = new char[4] { 'H', 'S', 'R', 'K' };
			for (int h = 0; h < 4; h++)
			{
				for (int i = 0; i < 4; i++)
				{
					for (int j = 2; j < 15; j++)
					{
						if (j < 10)
						{
							string kort = färger[i].ToString() + j.ToString();
							kortlek.Add(kort);
						}
						else if (j == 10)
						{
							string kort = färger[i].ToString() + "T";
							kortlek.Add(kort);
						}
						else if (j == 11)
						{
							string kort = färger[i].ToString() + "J";
							kortlek.Add(kort);
						}
						else if (j == 12)
						{
							string kort = färger[i].ToString() + "Q";
							kortlek.Add(kort);
						}
						else if (j == 13)
						{
							string kort = färger[i].ToString() + "K";
							kortlek.Add(kort);
						}
						else
						{
							string kort = färger[i].ToString() + "A";
							kortlek.Add(kort);
						}
					}
				}
			}
			return kortlek;
		}

		public static int hämtaKortvärde(string kort)
		{
			int värde = 0;
			if (!Char.IsLetter(kort[1]))
			{
				string y = (kort[1]).ToString();
				int.TryParse(y, out värde);
			}
			else if (kort[1] == 'A')
			{
				värde = 11;
			}
			else värde = 10;

			return värde;
		}

		public static int beräknaKortvärde(List<int> kort)
		{
			int värde = 0 ;
			kort.Sort();
			foreach (int i in kort)
			{
				if (i == 11) 
				{
					if (värde + 11 > 21)
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
