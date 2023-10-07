using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Black_Jack
{
    internal class Marker
    {
        List<int> värdelista;


        public List<int> sorteraMarker(List<int> markerVärde) 
        {
            värdelista = markerVärde;
            int räknare = 0;
            int x = värdelista.Last();
            if (värdelista.Last() !=5 && värdelista.Count > 1)
            {
                while (värdelista.Last() < värdelista[räknare] && räknare < värdelista.Count)
                {
                    räknare++;
                }
                värdelista.Insert(räknare, värdelista.Last());
                värdelista.RemoveAt(värdelista.Count - 1);
            }
            förstaSortering(räknare);
            return värdelista;
        }
        private void förstaSortering(int räkna)
        {
            int räknare = räkna;
            int summa = 0;
            int antalMarker = 0;
            switch (värdelista[räknare])
            {
                case 5:
                    for (int i = räknare; i < värdelista.Count; i++)
                    {
                        summa += värdelista[i];
                        if (summa == 10)
                        {
                            värdelista.Insert(räknare, 10);
                            värdelista.RemoveRange(räknare + 1, 2);
                            sorteraMarker(värdelista);
                            summa = 0;
                            break;
                        }
                    }
                    break;
                case 10:
                    for (int i = räknare; i < värdelista.Count; i++)
                    {
                        summa += värdelista[i];
                        antalMarker++;
                        if (summa == 25)
                        {
                            värdelista.Insert(räknare, 25);
                            värdelista.RemoveRange(räknare + 1, antalMarker);
                            sorteraMarker(värdelista);
                            summa=0;
                            break;
                        }
                        else if (summa == 50)
                        {
                            värdelista.Insert(räknare, 50);
                            värdelista.RemoveRange(räknare + 1, antalMarker);
                            sorteraMarker(värdelista);
                            summa = 0;
                            break;
                        }
                    }
                    break;
                case 25:
                    for (int i = räknare; i < värdelista.Count; i++)
                    {
                        summa += värdelista[i];
                        antalMarker++;
                        if (summa == 50)
                        {
                            värdelista.Insert(räknare, 50);
                            värdelista.RemoveRange(räknare + 1, antalMarker);
                            sorteraMarker(värdelista);
                            summa = 0;
                            break;
                        }
                        else if (summa == 100)
                        {
                            värdelista.Insert(räknare, 100);
                            värdelista.RemoveRange(räknare + 1, antalMarker);
                            sorteraMarker(värdelista);
                            summa = 0;
                            break;
                        }
                    }
                    break;
                case 50:
                    for (int i = räknare; i < värdelista.Count; i++)
                    {
                        summa += värdelista[i];
                        antalMarker++;
                        if (summa == 100)
                        {
                            värdelista.Insert(räknare, 100);
                            värdelista.RemoveRange(räknare + 1, antalMarker);
                            sorteraMarker(värdelista);
                            summa = 0;
                            break;
                        }
                    }
                    break;
                case 100:
                    for (int i = räknare; i < värdelista.Count; i++)
                    {
                        summa += värdelista[i];
                        antalMarker++;
                        if (summa == 500)
                        {
                            värdelista.Insert(räknare, 500);
                            värdelista.RemoveRange(räknare + 1, antalMarker);
                            sorteraMarker(värdelista);
                            summa = 0;  
                            break;
                        }
                    }
                    break;
            }
        }
        private void bytValör(int räknare)
        {
 
            int summa = 0;
            int antalRäknadeMarker = 0;

            for (int i = värdelista.Count -1; i > 0; i--)
            {
                summa += värdelista[i];
                antalRäknadeMarker++;
                switch (summa)
                {
                    case 10:
                        if (antalRäknadeMarker > 1)
                        {
                            värdelista.Insert(i, 10);
                            värdelista.RemoveRange(i + 1, värdelista.Count - i - 1);
                            bytValör(räknare);
                        }
                        break;
                    case 25:
                        if (antalRäknadeMarker > 1)
                        {
                            värdelista.Insert(i, 25);
                            värdelista.RemoveRange(i + 1, värdelista.Count - i - 1);
                            bytValör(räknare);
                        }
                        break;
                    case 50:
                        if (antalRäknadeMarker > 1)
                        {
                            värdelista.Insert(i, 50);
                            värdelista.RemoveRange(i + 1, värdelista.Count - i - 1);
                            bytValör(räknare);
                        }
                        break;
                    case 100:
                        if (antalRäknadeMarker > 1)
                        {
                            värdelista.Insert(i, 100);
                            värdelista.RemoveRange(i + 1, värdelista.Count - i - 1);
                            bytValör(räknare);
                        }
                        break;
                    case 500:
                        if (antalRäknadeMarker > 1)
                        {
                            värdelista.Insert(i, 500);
                            värdelista.RemoveRange(i + 1, värdelista.Count - i - 1);
                            bytValör(räknare);
                        }
                        break; 
                }
 
            }

        }

        
    }


}
