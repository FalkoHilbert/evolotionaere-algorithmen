using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelSalesman
{
    class Program
    {
        static List<T> CreateEmptyGenericList<T>(T example)
        {
            return new List<T>();
        }

        static int OnlyUnderValue = 45;
        
        static Dictionary<int, Dictionary<int,int>> BewertungsMatrix = new Dictionary<int,Dictionary<int,int>>()
                                                   {
                                                       {0,new Dictionary<int, int>() { {0,0},  {1,5},  {2,8},  {3,11}, {4,4},  {5,7} } },
                                                       {1,new Dictionary<int, int>() { {0,5},  {1,0},  {2,10}, {3,4},  {4,9},  {5,12} } },
                                                       {2,new Dictionary<int, int>() { {0,8},  {1,10}, {2,0},  {3,6},  {4,17}, {5,8} } },
                                                       {3,new Dictionary<int, int>() { {0,11}, {1,4},  {2,6},  {3,0},  {4,6},  {5,5} } },
                                                       {4,new Dictionary<int, int>() { {0,4},  {1,9},  {2,17}, {3,6},  {4,0},  {5,11} } },
                                                       {5,new Dictionary<int, int>() { {0,7},  {1,12}, {2,8},  {3,5},  {4,11}, {5,0} } }
                                                   };

        static List<Tierchen> Elterngeneration = new List<Tierchen>();
        static List<Tierchen> Kindgeneration = new List<Tierchen>();

        static void Main(string[] args)
        {
        start:
            Console.Clear();
            Elterngeneration.Clear();
            Kindgeneration.Clear();

            Elterngeneration.Add( new Tierchen(new List<int>() { 0, 1, 2, 3, 4, 5 }));
            Elterngeneration.Add( new Tierchen(new List<int>() { 4, 1, 2, 5, 0, 3}));
            Elterngeneration.Add( new Tierchen(new List<int>() { 0, 4, 1, 2, 3, 5 }));
            Elterngeneration.Add( new Tierchen(new List<int>() { 4, 3, 2, 1, 0, 5 }));
            Elterngeneration.Add( new Tierchen(new List<int>() { 5, 1, 0, 3, 4, 2 }));
            Elterngeneration.Add( new Tierchen(new List<int>() { 1, 3, 5, 2, 0, 4 }));
            Elterngeneration.Add( new Tierchen(new List<int>() { 5, 3, 2, 1, 0, 4 }));
            Elterngeneration.Add( new Tierchen(new List<int>() { 1, 4, 2, 3, 0, 5 }));
            Elterngeneration.Add( new Tierchen(new List<int>() { 3, 2, 1, 5, 4, 0 }));
            Elterngeneration.Add( new Tierchen(new List<int>() { 2, 3, 4, 1, 0, 5 }));
            Random randomizer = new Random();
            //while (Elterngeneration.Count < 10)
            //{
            //    var index1 = randomizer.Next(0,Elterngeneration.Count);
            //    var index2 = index1;
            //    while ( index2 == index1)
            //    {
            //        index2 = randomizer.Next(0,Elterngeneration.Count);
            //    }
            //    var newTierchen = EvolutionAlgorithms.Paaren(Elterngeneration[index1], Elterngeneration[index2]);
            //    newTierchen.InzestMutation();
            //    newTierchen.Mutation();
            //    newTierchen.Bewerte(BewertungsMatrix);
            //    Elterngeneration.Add(newTierchen);
            //}
            Console.WriteLine("Elterntierchen");
            int index = 0;
            foreach (var tierchen in Elterngeneration)
            {
                tierchen.Bewerte(BewertungsMatrix);
                Console.Write(string.Format("Tierchen {0}: ", index));
                foreach (var item in tierchen.GenCode)
                {
                    Console.Write(item);
                }
                Console.WriteLine();
                index++;
            }
            int generation = 1;
            string escape = "";
            while ( true)
            {
                generation = 1;
                while (generation < 10)
                {
                    /*if (generation % 100 == 0)*/
                    if (Kindgeneration.Count > 0) Console.WriteLine("Generation {0} Durchschnitt: {1}", generation, Kindgeneration.Average(tier => tier.Wert));
                    while (Kindgeneration.Count < 40*generation)
                    {
                        var index1 = randomizer.Next(0, Elterngeneration.Count);
                        var index2 = index1;
                        while (index2 == index1)
                        {
                            index2 = randomizer.Next(0, Elterngeneration.Count);
                        }
                        var newTierchen = EvolutionAlgorithms.Paaren(Elterngeneration[index1], Elterngeneration[index2]);
                        if (escape.Contains("mutation")) newTierchen.Mutation();
                        if (escape.Contains("extrem")) newTierchen.InzestMutation();
                        newTierchen.Bewerte(BewertungsMatrix);
                        int wahrscheinlichKeit = randomizer.Next(0, 101);
                        if ((wahrscheinlichKeit <= 30 && newTierchen.Wert <= OnlyUnderValue) || escape.Contains("noselection"))
                        {
                            Kindgeneration.Add(newTierchen);
                        }
                    }                    
                    generation++;
                }
KommaOrPlusChoice: 
                Console.Write("Welches Selektionsverfahren soll verwendet werden?\r\n" +
                                  "1: keine --> alle Erzeugten Kinder werden in die Elterngeneration übernommen\r\n" +
                                  "2: Komma-Selection\r\n" +
                                  "3: Plus-Selection\r\n");
                var selection = Console.ReadLine();
                if ( !(selection == "1" || selection == "2" || selection == "3") )
                {
                    Console.WriteLine("keine gültige Eingabe");
                    goto KommaOrPlusChoice;
                }
SelectionChoice: 
                var choice = "";
                if (selection != "1")
                {
                    Console.Write("Welches Auswahlverfahren soll verwendet werden?\r\n" +
                                      "1: deterministische Selection\r\n" +
                                      "2: Roulette Selection\r\n" +
                                      "3: fittnessproportionale Selektion\r\n" +
                                      "4: rangbasierte Selektion\r\n" +
                                      "5: q-fache Turnierselektion\r\n" +
                                      "6: q-stufige zweifache Turnierselektion\r\n");
                    choice = Console.ReadLine();
                }
                else choice = "1";
                var wahl = Wahlverfahren.determenistic;
                if (choice == "1" || choice == "2" || choice == "3" || choice == "4" || choice == "5" || choice == "6")
                    wahl = (Wahlverfahren)int.Parse(choice);
                else
                {
                    Console.WriteLine("keine gültige Eingabe");
                    goto SelectionChoice;
                }
                
                switch (selection)
                {
                    case "1":
                        Elterngeneration.AddRange(Kindgeneration);
                        break;
                    case "2":
                        Elterngeneration = EvolutionAlgorithms.commaSelection(Kindgeneration,wahl);
                        break;
                    case "3":
                        Elterngeneration = EvolutionAlgorithms.plusSelection(Elterngeneration, Kindgeneration, wahl);
                        break;
                }
                Kindgeneration.Clear();

                Console.WriteLine("Gesamtpopulationsumfang {0}:", Elterngeneration.Count);
                int minimalWert = Elterngeneration.Min(x => x.Wert);
                var minTierchen = Elterngeneration.Where(tier => tier.Wert == minimalWert).ToList();
                var minTierchenGeneration = CreateEmptyGenericList(new {gen ="" ,wert = 0 });
                foreach (var tierchen in minTierchen)
                {
                    minTierchenGeneration.Add(new { gen = tierchen.GenCodeString, wert = tierchen.Wert });

                }
                Console.WriteLine(string.Format("Ergebnis Tierchen: "));
                int counter = 1;
                foreach (var tierchen in minTierchenGeneration.Distinct().OrderBy(x => x.gen).ToList())
                {
                    Console.WriteLine("Tierchen({0}) Gen: {1} | Wert: {2} ", counter, tierchen.gen, tierchen.wert);
                    counter++;
                }

                Console.WriteLine("Umfang des Gen-Pools: {0}", Elterngeneration.OrderBy(tier => tier.GenCodeString).Select(tier => tier.GenCodeString).Distinct().Count());
                Console.WriteLine("Genpool anzeigen? (y/n)");
                escape = Console.ReadLine();
                if ( escape.StartsWith( "y") )
                {
                    Console.WriteLine("kompletter Gen-Pool:");
                    index = 0;
                    foreach (var Gencode in  Elterngeneration.Distinct(new GenCodeComparer()).OrderBy(tier => tier.GenCodeString).ToList())
                    {
                        Console.WriteLine("Tierchen {0}: {1} (Anzahl: {2})", index, Gencode.GenCodeString, Elterngeneration.Count(tier => tier.GenCodeString == Gencode.GenCodeString) );
                        index++;
                    }
                }
                var Most20Tierchen = Elterngeneration.GroupBy(tier => tier.GenCodeString).OrderByDescending(group => group.Count());
                counter = 0;
                Console.Clear();
                Console.WriteLine("Besten 20 Tierchen: ");
                Console.WriteLine("");
                foreach (var tier in Most20Tierchen)
                {
                    if (tier.FirstOrDefault().Wert == minimalWert)
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                    else if (tier.FirstOrDefault().Wert >= OnlyUnderValue)
                        Console.BackgroundColor = ConsoleColor.Red;
                    else
                        Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("Gencode: {0} | Anzahl: {1}| Wert: {2}", tier.FirstOrDefault().GenCodeString, tier.Count(), tier.FirstOrDefault().Wert );
                    counter++;
                    if(counter >= 20 ) break;
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ReadLine();
                Console.Write("Wiederholen (y/n)\r\n"+
                                   "Parameter: \r\n" +
                                   "mutation:\tnächster Durchgang mit normaler Mutation\r\n"+
                                   "extrem:\tnächster Durchgang mit extremer Mutation\r\n"+
                                   "noselection:\tübernimmt alle erzeugten Kinder in Kindgenereation\r\n");
                escape = Console.ReadLine();
                if (escape.StartsWith("n")) break;
            }
            Console.WriteLine("Verfahren neustarten? (y/n)");
            var exit = Console.ReadLine();
            if (exit == "y") goto start;
        }
    }
}
