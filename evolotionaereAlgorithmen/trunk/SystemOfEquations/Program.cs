using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SystemOfEquations
{
    class Program
    {
        static List<Tierchen> Elterngeneration = new List<Tierchen>();
        static List<Tierchen> Kindgeneration = new List<Tierchen>();
        static List<Tierchen> TierchenHistory = new List<Tierchen>();

        static List<Intervall> intervall;
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
                intervall = new List<Intervall>();

                bool couldParsed = false;
                bool loadDocument = false;
                int gene = 0;

                // Überspringe, falls keine start Generation übergeben wurde
                if ( (args.Length > 0) )
                {
                    Console.WriteLine("Soll das übergebenene Dokument geladen werden? (y/n)");
                    string answer = Console.ReadLine();
                    if (answer == "y")
                    {
                        loadDocument = true;
                        try
                        {
                            // Elterngeneration einladen
                            XDocument doc = XDocument.Load(args[0]);
                            Elterngeneration =
                                // lade alle "tier"-Elemente 
                                doc.Root.Descendants("tier").Select(tier =>
                                    // erzeuge für jedes ein Tierchen Object
                                    new Tierchen(
                                        // lade innerhalb des "tier"-Elementes alle "gen"-Elemente
                                        tier.Descendants("gen").Select(gen =>
                                            // erzeuge für jedes ein Allel
                                            new Allel(
                                                // lade innerhalb des "gen"-Elementes alle "allel"-Elemente
                                                gen.Descendants("allel").Select(allel => 
                                                    // parse den bool-Wert
                                                    bool.Parse(allel.Value)).ToList(),
                                                // Intervall steht als Attribut innerhalb des "gen"-Elementes
                                                new Intervall(
                                                    int.Parse(gen.Attribute("interval_start").Value),
                                                    int.Parse(gen.Attribute("interval_end").Value))
                                            )
                                        ).ToList()
                                    )
                                ).ToList();
                            elternSize = Elterngeneration.Count();
                            Console.WriteLine("gelesene Größe der Elterngeneration:");
                            Console.WriteLine(elternSize);
                        }
                        catch (Exception) { loadDocument = false; }
                        
                    }
                }
                /* 
                 * Programmbereiche auskommentiert um die Parametereingabe zu vereinfachen
                 */
                if (!loadDocument)
                {
                    //while (!couldParsed)
                    //{
                    //    Console.WriteLine("Größe der Elterngeneration?");
                    //    string lenght = Console.ReadLine();
                    //    couldParsed = Int32.TryParse(lenght, out elternSize);
                    //}
                    elternSize = 100;
                    Console.WriteLine("Größe Elterngeneration = {0}", elternSize);
                }
                //couldParsed = false;
                //while (!couldParsed)
                //{
                //    Console.WriteLine("Wie viele werden davon Rekombiniert?");
                //    string lenght = Console.ReadLine();
                //    couldParsed = Int32.TryParse(lenght, out countOfRecombinations);
                //    couldParsed = couldParsed ? countOfRecombinations < elternSize : false;
                //}
                countOfRecombinations = 50;
                Console.WriteLine("Anzahl Rekombinationen = {0}", countOfRecombinations); 

                //couldParsed = false;
                //if (!loadDocument)
                //{
                //    while (!couldParsed)
                //    {
                //        Console.WriteLine("Länge des Binärstrings?");
                //        string lenght = Console.ReadLine();
                //        couldParsed = Int32.TryParse(lenght, out binärStringLenght);
                //    }
                //}
                /* Was sagt die binäre Stringlänge aus? Ist das [000] [001] ... oder [000000000]?
                 */
                binärStringLenght = 9;
                Console.WriteLine("Binärstringlänge = {0}", binärStringLenght);

                //couldParsed = false;
                //if (!loadDocument)
                //{
                //    while (!couldParsed)
                //    {
                //        Console.WriteLine("Anzahl der Gene?");
                //        string lenght = Console.ReadLine();
                //        couldParsed = Int32.TryParse(lenght, out gene);
                //    }
                //}
                gene = 1;
                Console.WriteLine("Anzahl der Gene = {0}", gene);

                //couldParsed = false;
                //if (!loadDocument)
                //{
                //    while (!couldParsed)
                //    {
                //        Console.WriteLine("Größe des {0}. Intervals?", (intervall.Count() + 1));
                //        string lenght = Console.ReadLine();
                //        Intervall actInterval;
                //        couldParsed = Intervall.TryParse(lenght, out actInterval);
                //        if (couldParsed)
                //        {
                //            intervall.Add(actInterval);
                //            couldParsed = (intervall.Count() >= gene);
                //        }
                //    }
                //}
                Intervall actIntervall;
                couldParsed = Intervall.TryParse("3", out actIntervall);
                intervall.Add(actIntervall);
                couldParsed = Intervall.TryParse("3", out actIntervall);
                intervall.Add(actIntervall);
                couldParsed = Intervall.TryParse("3", out actIntervall);
                intervall.Add(actIntervall);
                // hier funktioniert noch die Ausgabe nicht richtig
                Console.WriteLine("Intervalle = {0}", intervall);

                couldParsed = false;
                //while (!couldParsed)
                //{
                //    Console.WriteLine("Größe der History?");
                //    string lenght = Console.ReadLine();
                //    couldParsed = Int32.TryParse(lenght, out historySize);
                //}
                //couldParsed = false;
                /*
                 * Wie definiert sich die Größe? Sind es Mengen von Tierchen oder Mengen von Generationen?
                 */
                historySize = 2;

                while (!couldParsed)
                {
                    Console.WriteLine("Generationen die erzeugt werden sollen?");
                    string lenght = Console.ReadLine();
                    couldParsed = Int32.TryParse(lenght, out countOfGenerations);
                }                
                // es wurde keine Anfangs-Generation übergeben
                if (!loadDocument)
                {
                    Elterngeneration = new List<Tierchen>();
                    // erzeuge Elterngeneration
                    while (Elterngeneration.Count < elternSize)
                    {
                        Elterngeneration.Add(Tierchen.RandomTier(binärStringLenght, intervall, funktionCount));
                        /*
                         * Wie kommt das mit dem Distinct zustande ?
                         * Ist ToList() eine Methode einer über TierchenComparer liegenden Klasse ?
                         * Welche voraussetzungen müssen erfüllt sein ?
                         */
                        Elterngeneration = Elterngeneration.Distinct(new TierchenComparer()).ToList();
                    }
                    try
                    {
                        // Elterngeneration im Programm Verzeichnis unter dem Namen "last.xml" speichern
                        // xml-Dokument erzeugen
                        XDocument doc = new XDocument(
                            // erzeuge des Wurzelelement "generation"
                            new XElement("generation",
                                // für jedes tier in der Generation
                                Elterngeneration.Select(tier =>
                                    // erzeuge "tier"-Element
                                    new XElement("tier",
                                        // für jedes gen im tierchen
                                        tier.GenCode.Select(gen =>
                                            // erzeuge "gen"-Element
                                            new XElement("gen",
                                                // füge "interval_start"-Attribut hinzu
                                                new XAttribute("interval_start",
                                                    gen.Interval.start
                                                ),
                                                // füge "interval_end"-Attribut hinzu
                                                new XAttribute("interval_end",
                                                    gen.Interval.end
                                                ),
                                                // für jedes allel (Bit) im Gen
                                                gen.BinärCode.Select(allel =>
                                                    // speichere bool'schen Wert 
                                                    new XElement("allel",
                                                        allel.ToString()
                                                    )
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        );
                        // dokument speichern
                        doc.Save("last.xml");
                    }
                    catch (Exception) { }
                }
                
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
