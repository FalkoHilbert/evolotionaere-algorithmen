﻿using System;
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

        static List<Intervall> intervalle;
        static int binärStringLenght = 0;
        static int countOfRecombinations = 0;
        static int elternSize = 0;
        static int funktionCount = 3;
        static bool repeat = true;
        static bool repeat2 = true;
        static bool restart = true;
        static int historySize = 10;
        static int countOfGenerations = 1;
        static Problem problem = null;

        static void Main(string[] args)
        {
            while (restart)
            {
                restart = false;
                repeat = true;
                Console.Clear();
                TierchenHistory = new List<Tierchen>();
                intervalle = new List<Intervall>();

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
                                        // lade Problem
                                        new Problem(int.Parse(tier.Attribute("problemType").Value)),
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
                    // Problem abfragen
                    couldParsed = false;
                    while (!couldParsed)
                    {
                        Console.WriteLine("Welches Problem soll gelöst werden?\r\n"
                                        + "[1] lineares Gleichungssystem\r\n"
                                        + "[2] Griewank-Funktion\r\n"
                                        + "[3] Ackley-Funktion\r\n"
                                        + "[4] C-Funktion");
                        string lenght = Console.ReadLine();
                        couldParsed = Problem.TryParse(lenght, out problem);
                    }
                }

                if (!loadDocument)
                {
                    couldParsed = false;
                    while (!couldParsed)
                    {
                        Console.WriteLine("Größe der Elterngeneration?");
                        string lenght = Console.ReadLine();
                        couldParsed = Int32.TryParse(lenght, out elternSize);
                    }
                    //elternSize = 100;
                    Console.WriteLine("Größe Elterngeneration = {0}", elternSize);
                }
                couldParsed = false;
                while (!couldParsed)
                {
                    Console.WriteLine("Wie viele werden davon Rekombiniert?");
                    string lenght = Console.ReadLine();
                    couldParsed = Int32.TryParse(lenght, out countOfRecombinations);
                    couldParsed = couldParsed ? countOfRecombinations < elternSize : false;
                }
                //countOfRecombinations = 50;
                Console.WriteLine("Anzahl Rekombinationen = {0}", countOfRecombinations);

                couldParsed = false;
                if (!loadDocument)
                {
                    while (!couldParsed)
                    {
                        Console.WriteLine("Länge des Binärstrings?");
                        string lenght = Console.ReadLine();
                        couldParsed = Int32.TryParse(lenght, out binärStringLenght);
                    }
                    /* ### Was sagt die binäre Stringlänge aus? Ist das [000] [001] ... oder [000000000]? ###
                     * !!! gibt die Anzahl der stellen innerhalb eines Allels an
                     * !!! Bsp: Länge = 6 -> Gen: [001001] [011011] [010010]
                     */
                    // binärStringLenght = 9;
                    // Console.WriteLine("Binärstringlänge = {0}", binärStringLenght);
                }

                couldParsed = false;
                if (!loadDocument)
                {
                    while (!couldParsed)
                    {
                        Console.WriteLine("Anzahl der Gene?");
                        string lenght = Console.ReadLine();
                        couldParsed = Int32.TryParse(lenght, out gene);
                    }
                    //gene = 1;
                    //Console.WriteLine("Anzahl der Gene = {0}", gene);
                }


                couldParsed = false;
                if (!loadDocument)
                {
                    Console.WriteLine("Möchten Sie verschiedene Intervalle für jedes Gen angeben? [y/n]");
                    string input = Console.ReadLine();
                    if (input == "y")
                    {
                        while (!couldParsed)
                        {
                            Console.WriteLine("Größe des {0}. Intervals?", (intervalle.Count() + 1));
                            string lenght = Console.ReadLine();
                            Intervall actInterval;
                            couldParsed = Intervall.TryParse(lenght, out actInterval);
                            if (couldParsed)
                            {
                                intervalle.Add(actInterval);
                                couldParsed = (intervalle.Count() >= gene);
                            }
                        }
                    }
                    else
                    {
                        while (!couldParsed)
                        {
                            Console.WriteLine("Größe aller Intervalle?");
                            string lenght = Console.ReadLine();
                            Intervall actInterval;
                            couldParsed = Intervall.TryParse(lenght, out actInterval);
                            if (couldParsed)
                            {
                                while (intervalle.Count() < gene)
                                {
                                    intervalle.Add(actInterval);
                                }                         
                            }
                        }                    
                    }
                    //Intervall actIntervall;
                    //couldParsed = Intervall.TryParse("3", out actIntervall);
                    //intervall.Add(actIntervall);
                    //couldParsed = Intervall.TryParse("3", out actIntervall);
                    //intervall.Add(actIntervall);
                    //couldParsed = Intervall.TryParse("3", out actIntervall);
                    //intervall.Add(actIntervall);

                    // ### hier funktioniert noch die Ausgabe nicht richtig ###
                    // !!! liegt daran, dass interval ein Object ist und c# keine automatische object ausgabe hat
                    // !!! lösung wäre entweder die toString Methode im Object zu deklarieren oder per hand den Start und das ende auszugeben
                    // !!! HINWEIS: die Methode TryParse in der Intervall Klasse versucht aus einem String wie: "[-1,1]" oder "-1,1" ein Intervall auszulesen 
                    //couldParsed = Intervall.TryParse("3,5", out actIntervall);
                    //intervall.Add(actIntervall);
                    Console.WriteLine("Intervalle = {0}", String.Join(",", intervalle.Select(o => o.ToString()).ToArray()));
                }

                couldParsed = false;
                while (!couldParsed)
                {
                    Console.WriteLine("Größe der History?");
                    string lenght = Console.ReadLine();
                    couldParsed = Int32.TryParse(lenght, out historySize);
                }
                couldParsed = false;
                /*
                 * ### Wie definiert sich die Größe? Sind es Mengen von Tierchen oder Mengen von Generationen? ###
                 * !!! Größe der History bestimmt, wie viel besten Tierchen gesoeichert werden sollen, damit die History nicht unnötig voll läuft
                 * !!! Beispiel: Größe der History auf 10 gesetzt bewirkt, dass die History ( nach Wertigkeit sortiert ) die besten 10 Tierchen enthält
                 */
                //historySize = 2;

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
                        Elterngeneration.Add(Tierchen.RandomTier(binärStringLenght, intervalle, gene, problem));
                        /*
                         * ### Wie kommt das mit dem Distinct zustande ? ###
                         * ### Ist ToList() eine Methode einer über TierchenComparer liegenden Klasse ? ###
                         * ### Welche voraussetzungen müssen erfüllt sein ? ###
                         * !!! Hatten wir ja geklärt, oder?
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
                                        // erzeuge Problem-Attribute
                                        new XAttribute("problemType",
                                            (int)(tier.Problem.ProblemType)
                                            ),
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

                /*
                 * Ab hier beginnen die Methoden
                 * Zunächst wird Selektiert
                 */ 
                Random randomizer = new Random();
                int counter = 0;
                while (repeat )
                {
                    repeat = false;
                    // Sichere die aktuelle Elterngeneration in die History
                    TierchenHistory.AddRange(Elterngeneration);

                    //Eliminiere die Doppelten
                    TierchenHistory = TierchenHistory.Distinct(new TierchenComparer()).OrderBy(tier => tier.Wert).Take(historySize).ToList();
                    
                    Kindgeneration = new List<Tierchen>();

                    //Rufe Ein-Punkt-Rekombination auf
                    Kindgeneration = EvolutionAlgorithms.einPunktRekombination(randomizer, countOfRecombinations, Kindgeneration, Elterngeneration);

                    Kindgeneration = EvolutionAlgorithms.mutiereKinder(randomizer, Kindgeneration, Elterngeneration, elternSize);

                    /*
                     * Der folgende Bereich sollte nicht direkt beschritten werden
                     * Die Methoden sollten aufrufbar / wählbar sein
                     */
                    // ### [NEW] ist ein wenig Kritisch, da diese Frage vor jedem Erteugen einer neuen Generation gestellt wird
                    // ### das heißt wenn man auf einen Schlag 100 Generationen erzeugen will wird diese Schleife immer durch diese Frage unterbrochen
                    // ### Frage Stellen, bevor ein neuen Durchlauf beginnt (siehe Frage nach Wiederholung)
                    repeat2 = true;
                    while (repeat2)
                    {
                        Console.WriteLine("Welche Selektion soll ausgeführt werden?\r\n[n]: keine (Kinder als Elten übernehmen)\r\n[k]: Komma\r\n[p]: Plus\r\n[...]");
                        var input = Console.ReadLine();
                        if (input == "n")
                        {
                            repeat2 = false;
                            //Lösche alle Eltern
                            Elterngeneration.Clear();

                            //Kindgeneration ist neue Elterngeneration
                            Elterngeneration.AddRange(Kindgeneration);

                            //Jetzt kann die Kindgeneration gelöscht werden
                            Kindgeneration.Clear();
                        }
                        else if (input == "k")
                        {
                            repeat2 = false;
                            EvolutionAlgorithms.commaSelection(Kindgeneration, Wahlverfahren.determenistic);
                            //KommaSelektion(randomizer);
                        }
                        else if (input == "p")
                        {
                            repeat2 = false;
                            EvolutionAlgorithms.plusSelection(Elterngeneration,Kindgeneration, Wahlverfahren.determenistic);
                            //KommaSelektion(randomizer);
                        }
                    }                    


                    foreach (var tier in Elterngeneration)
                    {
                        Console.WriteLine("Tier: {0} | Wert:\t{1}", tier.ToString(), tier.Wert.ToString("####0.#####"));
                    }
                    counter++;

                    if (counter >= countOfGenerations)
                    {
                        // letzte Sicherung der Elterngeneration in die History
                        TierchenHistory.AddRange(Elterngeneration);

                        //Eliminiere die Doppelten
                        TierchenHistory = TierchenHistory.Distinct(new TierchenComparer()).OrderBy(tier => tier.Wert).Take(historySize).ToList();

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
                    }
                }
            }
        }


/*
        private static void KommaSelektion(Random randomizer, List<Tierchen> kindGen, List<Tierchen> elternGen)
        {
            /* Wähle die Menge an notwendigen Kindern per Zufall nach der Form:
             * µ/7 <= r <= µ/5
             *
            Console.WriteLine("Beginne mit Kommaselektion");
            int mengeEltern = randomizer.Next(elternGen.Count() / 7, elternGen.Count() / 5);

            Console.WriteLine("Anzahl der Elterntiere: {0}, Anzahl der Kindertiere: {1}", mengeEltern, elternSize);
            
            
            while (Kindgeneration.Count() < mengeEltern)
            {
                //erzeuge temporäre Kindgeneration, die dann Elterngeneration sind
                int index = randomizer.Next(0, Elterngeneration.Count);

                Kindgeneration.Add(Elterngeneration[index]);
            }
            // Sichere die aktuelle Elterngeneration in die History
            TierchenHistory.AddRange(Elterngeneration);

            // ### HIER NOCH EIN DISTINCT NÖTIG??? ###
            // !!! ich denke, dass es garnicht notwendig ist die Generation in die History zu sichern, da dies ja generell oben vor jedem Schleifen durchlauf passiert

            //Lösche alle Eltern
            Elterngeneration.Clear();

            //Kindgeneration ist neue Elterngeneration
            Elterngeneration.AddRange(Kindgeneration);

            Kindgeneration.Clear();

            //Erzeuge aus den Selektierten Eltern die neue Kindgeneration von alter Stärke
            while (Kindgeneration.Count() < elternSize)
            {
                /*
                 * Hier passiert jetzt eine Rekombination
                 * Wahl zwischen Ein-Punkt / N-Punkt
                 * ### Auswahl = Globale Variable? ==> Führe immer die gleiche Methode aus ###
                 * !!! Hierfür sollte genauso wie bei der Frage nach der Selektion eine Frage vor dem erzeugen einer Menge von Generationen eine Abfrage kommen
                 *
                einPunktRekombination(randomizer, elternSize);

            }
        }
*/
    }
}
