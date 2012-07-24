using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemOfEquations
{
    class Program
    {
        static List<Tierchen> Elterngeneration = new List<Tierchen>();
        static List<Tierchen> Kindgeneration = new List<Tierchen>();
        static List<Tierchen> TierchenHistory = new List<Tierchen>();

        static List<Interval> interval;
        static int binärStringLenght = 0;
        static int countOfRecombinations = 0;
        static int elternSize = 0;
        static int funktionCount = 3;
        static bool repeat = true;
        static bool restart = true;
        static int historySize = 10;
        static int countOfGenerations = 1;

        static void Main(string[] args)
        {
            while (restart)
            {
                restart = false;
                repeat = true;
                Console.Clear();
                TierchenHistory = new List<Tierchen>();
                interval = new List<Interval>();

                bool couldParsed = false;
                while (!couldParsed)
                {
                    Console.WriteLine("Größe der Elterngeneration?");
                    string lenght = Console.ReadLine();
                    couldParsed = Int32.TryParse(lenght, out elternSize);
                }
                couldParsed = false;
                while (!couldParsed)
                {
                    Console.WriteLine("Wie viele werden davon Rekombiniert?");
                    string lenght = Console.ReadLine();
                    couldParsed = Int32.TryParse(lenght, out countOfRecombinations);
                    couldParsed = couldParsed ? countOfRecombinations < elternSize : false;
                }
                couldParsed = false;
                while (!couldParsed)
                {
                    Console.WriteLine("Länge des Binärstrings?");
                    string lenght = Console.ReadLine();
                    couldParsed = Int32.TryParse(lenght, out binärStringLenght);
                }
                couldParsed = false;
                int gene = 0;
                while (!couldParsed)
                {
                    Console.WriteLine("Anzahl der Gene?");
                    string lenght = Console.ReadLine();
                    couldParsed = Int32.TryParse(lenght, out gene);
                }
                couldParsed = false;
                while (!couldParsed )
                {
                    Console.WriteLine("Größe des {0}. Intervals?", (interval.Count()+1));
                    string lenght = Console.ReadLine();
                    Interval actInterval;
                    couldParsed = Interval.TryParse(lenght, out actInterval);
                    if (couldParsed)
                    {
                        interval.Add(actInterval);
                        couldParsed = (interval.Count() >= gene);
                    }
                }
                couldParsed = false;
                while (!couldParsed)
                {
                    Console.WriteLine("Größe der History?");
                    string lenght = Console.ReadLine();
                    couldParsed = Int32.TryParse(lenght, out historySize);
                }
                couldParsed = false;
                while (!couldParsed)
                {
                    Console.WriteLine("Generationen die erzeugt werden sollen?");
                    string lenght = Console.ReadLine();
                    couldParsed = Int32.TryParse(lenght, out countOfGenerations);
                }

                Elterngeneration = new List<Tierchen>();
                // erzeuge Elterngeneration
                while (Elterngeneration.Count < elternSize)
                {
                    Elterngeneration.Add(Tierchen.RandomTier(binärStringLenght, interval, funktionCount));
                    /*
                     * Wie kommt das mit dem Distinct zustande ?
                     * Ist ToList() eine Methode einer über TierchenComparer liegenden Klasse ?
                     * Welche voraussetzungen müssen erfüllt sein ?
                     */
                    Elterngeneration = Elterngeneration.Distinct(new TierchenComparer()).ToList();
                }
                // tier spiegelt den Inhalt welcher Variable in der Tierchen.Elterngeneration wieder?
                // oder ist das nur ein Platzhalter/Objekt für ein Element der Liste?
                foreach (var tier in Elterngeneration)
                {
                    Console.WriteLine("Tier: {0} | Wert:\t{1}", tier.ToString(), tier.Wert.ToString("####0.#####"));
                }


                Random randomizer = new Random();
                int counter = 0;
                while (repeat )
                {
                    repeat = false;
                    TierchenHistory.AddRange(Elterngeneration);
                    TierchenHistory = TierchenHistory.Distinct(new TierchenComparer()).OrderBy(tier => tier.Wert).Take(historySize).ToList();


                    Kindgeneration = new List<Tierchen>();
                    // Füge zur Kindgeneration zwei Kinder über eine Ein-Punkt-Rekombination hinzu

                    while (Kindgeneration.Count() < countOfRecombinations)
                    {
                        var index1 = randomizer.Next(0, Elterngeneration.Count);
                        var index2 = index1;
                        while (index2 == index1)
                        {
                            index2 = randomizer.Next(0, Elterngeneration.Count);
                        }

                        Tierchen Kind1;
                        Tierchen Kind2;
                        Tierchen.OnePointRecombination(Elterngeneration[index1], Elterngeneration[index2], out Kind1, out Kind2);
                        Kindgeneration.Add(Kind1);
                        Kindgeneration.Add(Kind2);
                        Kindgeneration = Kindgeneration.Distinct(new TierchenComparer()).ToList();
                    }

                    while (Kindgeneration.Count() < elternSize)
                    {
                        var index1 = randomizer.Next(0, Elterngeneration.Count);
                        var wahrscheinlichkeit = randomizer.NextDouble();
                        Tierchen kind;
                        if (wahrscheinlichkeit <= 0.3) kind = Elterngeneration[index1].InzestMutation();
                        else kind = Elterngeneration[index1].Mutation();

                        Kindgeneration.Add(kind);

                        //Vermutlich ein Filter, dass es keine Duplikate gibt?
                        Kindgeneration = Kindgeneration.Distinct(new TierchenComparer()).ToList();
                    }
                    Elterngeneration.Clear();
                    Elterngeneration.AddRange(Kindgeneration);
                    Kindgeneration.Clear();

                    foreach (var tier in Elterngeneration)
                    {
                        Console.WriteLine("Tier: {0} | Wert:\t{1}", tier.ToString(), tier.Wert.ToString("####0.#####"));
                    }

                    if (counter >= countOfGenerations)
                    {
                        counter = 0;
                        Console.WriteLine("Soll das Beste aller erzeugten Individuen gezeigt werden? (y/n)");
                        var input = Console.ReadLine();
                        if (input == "y")
                        {
                            int countOfShownTierchen = 0;
                            couldParsed = false;
                            while (!couldParsed)
                            {
                                Console.WriteLine("Wie viele sollen gezeigt werden?");
                                string lenght = Console.ReadLine();                                
                                couldParsed = Int32.TryParse(lenght, out countOfShownTierchen);
                            }
                            foreach (var tier in TierchenHistory.OrderBy(tier => tier.Wert).Take(countOfShownTierchen).ToList())
                            {
                                Console.WriteLine("Tier: {0}", tier.ToNicerString());
                            }
                        }

                        Console.WriteLine("Wiederholen? (y/n)");
                        input = Console.ReadLine();
                        if (input == "y") repeat = true;
                        else
                        {
                            repeat = false;
                            Console.WriteLine("Neustarten? (y/n)");
                            input = Console.ReadLine();
                            if (input == "y") restart = true;
                            else restart = false;
                        }
                    }
                    else
                    {
                        repeat = true;
                        counter++;
                    }
                }
            }
        }
    }
}
