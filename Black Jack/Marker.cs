using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Black_Jack
{
    internal class Marker //Klass som använder för att hantera de enskilda markerna då de fortfarande visades med bilder på markern
    {
        List<int> värdelista;
        int räknare;
        bool harBytt = false;

        public List<int> sortera(List<int> markerVärde)
        {
            värdelista = markerVärde;
            värdelista = sorteraMarker(värdelista);
            while (bytValör())
            {
                harBytt = bytValör();
            }
            return värdelista;

        }

        public List<int> sorteraMarker(List<int> markerVärde) //Sorterade in en satsad marker på rätt plats
        {
            värdelista = markerVärde;
            räknare = 0;
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
            return värdelista;
        }

        public List<int> raderaMarker(List<int> markerVärde, int tabortvärde) //Raderade valören samt omvandlade en högre valör till lägre valörer, om det behövdes
        {
            värdelista = markerVärde;
            for (int i = värdelista.Count-1; i > -1; i--)
            {
                int x = värdelista[i];
                if (x == tabortvärde)
                {
                    värdelista.RemoveAt(i);
                    break;
                } else if ((tabortvärde < värdelista[i]))
                {
                    switch(tabortvärde)
                    {
                        case 5:
                            if (värdelista[i] / tabortvärde == 200)
                            {
                                värdelista.RemoveAt(i);
                                värdelista.Add(500);
                                for (int j = 1; j < 5; j++)
                                {
                                    värdelista.Insert(i + j, 100);
                                }
                                värdelista.Add(50);
                                for (int k = 6; k < 10; k++)
                                {
                                    värdelista.Insert(i + k, 10);
                                }
                                värdelista.Add(5);
                            }
                            else if (värdelista[i] / tabortvärde == 100)
                            {
                                värdelista.RemoveAt(i);
                                for (int j = 0; j < 4; j++)
                                {
                                    värdelista.Insert(i + j, 100);
                                }
                                värdelista.Add(50);
                                for (int k = 5; k < 9; k++)
                                {
                                    värdelista.Insert(i + k, 10);
                                }
                                värdelista.Add(5);
                            }
                            else if (värdelista[i] / tabortvärde == 20)
                            {
                                värdelista.RemoveAt(i);
                                värdelista.Add(50);
                                for (int j = 1; j < 5; j++)
                                {
                                    värdelista.Insert(i + j, 10);
                                }
                                värdelista.Add(5);
                            }
                            else if (värdelista[i] / tabortvärde == 10)
                            {
                                värdelista.RemoveAt(i);
                                for (int j = 0; j < 4; j++)
                                {
                                    värdelista.Insert(i + j, 10);
                                }
                                värdelista.Add(5);
                            }
                            else
                            {
                                värdelista.RemoveAt(i);
                                värdelista.Add(5);
                            }
                            break;
                        case 10:
                            if (värdelista[i] / tabortvärde == 100)
                            {
                                värdelista.RemoveAt(i);
                                värdelista.Add(500);
                                for (int j = 1; j < 5; j++)
                                {
                                    värdelista.Insert(i + j, 100);
                                }
                                värdelista.Add(50);
                                for (int k = 6; k < 10; k++)
                                {
                                    värdelista.Insert(i + k, 10);
                                }
                            } else if (värdelista[i] / tabortvärde == 50)
                            {
                                värdelista.RemoveAt(i);
                                for (int j = 0; j < 4; j++)
                                {
                                    värdelista.Insert(i + j, 100);
                                }
                                värdelista.Add(50);
                                for (int k = 5; k < 9; k++)
                                {
                                    värdelista.Insert(i + k, 10);
                                }
                            } else if (värdelista[i] / tabortvärde == 10)
                            {
                                värdelista.RemoveAt(i);
                                värdelista.Add(50);
                                for (int j = 1; j < 5; j++)
                                {
                                    värdelista.Insert(i + j, 10);
                                }
                            } else
                            {
                                värdelista.RemoveAt(i);
                                for (int j = 0; j < 4; j++)
                                {
                                    värdelista.Insert(i + j, 10);
                                }
                            }
                            break;
                        case 50:
                            if (värdelista[i] / tabortvärde == 20)
                            {
                                värdelista.RemoveAt(i);
                                värdelista.Add(500);
                                for (int j = 1; j < 5; j++)
                                {
                                    värdelista.Insert(i + j, 100);
                                }
                                värdelista.Add(50);
                            } else if (värdelista[i] / tabortvärde == 10)
                            {
                                värdelista.RemoveAt(i);
                                 for (int j = 0; j < 4; j++)
                                {
                                    värdelista.Insert(i + j, 100);
                                }
                                värdelista.Add(50);
                            } else
                            {
                                värdelista.RemoveAt(i);
                                värdelista.Add(50);
                            }
                                break;
                        case 100:
                            if (värdelista[i]/tabortvärde == 5)
                            {
                                värdelista.RemoveAt(i);
                                for (int j = 0; j<4; j++)
                                {
                                    värdelista.Insert(i + j, 100);
                                }
                            } else
                            {
                                värdelista.RemoveAt(i);
                                for (int j = 0; j < 9; j++)
                                {
                                    värdelista.Insert(i + j, 100);
                                }
                            }
                            break;
                        case 500:
                            värdelista[i] = 500;
                            break;
                        case 1000:
                            värdelista.Clear();
                            break;
                    }
                    break;
                }
            }
            if (värdelista.Count > 0)
            {
                bytValör();
                värdelista = sorteraMarker(värdelista);
            }
            return värdelista;
        }


        private bool bytValör() //Bytte till lämpligt marker
        {
            int summa = 0;
            int antalRäknadeMarker = 0;
            do
            {
                harBytt = false;
                for (int j = värdelista.Count - 1; j >= 0; j--)
                {
                    for (int i = j; i > -1; i--)
                    {

                        summa += värdelista[i];
                        antalRäknadeMarker++;
                        switch (summa)
                        {
                            case 10:
                                if (antalRäknadeMarker > 1)
                                {
                                    värdelista.Insert(i, 10);
                                    värdelista.RemoveRange(i + 1, 2);
                                    harBytt = true;
                                }
                                break;
                            case 50:
                                if (antalRäknadeMarker > 1)
                                {
                                    värdelista.Insert(i, 50);
                                    värdelista.RemoveRange(i + 1, 5);
                                    harBytt = true;
                                }
                                break;
                            case 100:
                                if (antalRäknadeMarker > 1)
                                {
                                    värdelista.Insert(i, 100);
                                    värdelista.RemoveRange(i + 1, 2);
                                    harBytt = true;
                                }
                                break;
                            case 500:
                                if (antalRäknadeMarker > 1)
                                {
                                    värdelista.Insert(i, 500);
                                    värdelista.RemoveRange(i + 1, 5);
                                    harBytt = true;
                                }
                                break;
                            case 1000:
                                if (antalRäknadeMarker > 1)
                                {
                                    värdelista.Insert(i, 1000);
                                    värdelista.RemoveRange(i + 1, 2);
                                    harBytt = true;
                                }
                                break;
                        }
                        if (harBytt)
                        {
                            summa = 0;
                            antalRäknadeMarker = 0;
                            break;
                        }
                        if (i == 0)
                        {
                            summa = 0;
                            antalRäknadeMarker = 0;
                        }
                    
                    }
                    if (harBytt) break;
                }
            } while (harBytt);
            return harBytt;
        } 
    }
}
