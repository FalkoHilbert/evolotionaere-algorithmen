using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemOfEquations
{

    public class Tierchen
    {
        public List<Allel> GenCode;
        public double Wert;
        public Problem Problem;

        public Tierchen(List<Allel> genCode)
        {
            if (genCode.Count != 0)
            {
                this.GenCode = genCode;
                this.Bewerte();
            }
        }

        public Tierchen(Problem problem, List<Allel> genCode)
        {
            // TODO: Complete member initialization
            this.Problem = problem;
            if (genCode.Count != 0)
            {
                this.GenCode = genCode;
                this.Bewerte();
            }
        }

        public Tierchen Mutation()
        {
            /* Führe eine Mutation aus
             * Bestimme dazu die Allelmenge 
             * 
             */
            Random randomizer = new Random();
            var AllelSize = GenCode.Select( allel => allel.Size ).Distinct();
            var InterValList = GenCode.Select( allel => allel.Interval ).ToList();
            if ( AllelSize.Count() == 1 )
            {
                var allelLenght = AllelSize.FirstOrDefault();
                var gencode = CompleteGenCode;
                int Stelle1 = randomizer.Next(gencode.Count);
                int Stelle2 = Stelle1;
                if (Stelle1 < gencode.Count - 1)
                    Stelle2 = randomizer.Next(Stelle1 + 1, gencode.Count);
                gencode.Reverse(Stelle1, Stelle2 - Stelle1);
                // gesamtliste wieder trennen
                var newTier =  new Tierchen(this.Problem,GenCode);
                newTier.CompleteGenCode = gencode;
                return newTier;
            }
            else return this;
        }

        public Tierchen InzestMutation()
        {
            Random randomizer = new Random();
            var AllelSize = GenCode.Select(allel => allel.Size).Distinct();
            var InterValList = GenCode.Select(allel => allel.Interval).ToList();
            if (AllelSize.Count() == 1)
            {
                var allelLenght = AllelSize.FirstOrDefault();
                var gencode = CompleteGenCode;
                int Stelle1 = randomizer.Next(gencode.Count);
                int Stelle2 = Stelle1;
                if (Stelle1 < gencode.Count-1)
                    Stelle2 = randomizer.Next(Stelle1 + 1, gencode.Count);
                var tmpValue = gencode[Stelle1];
                gencode[Stelle1] = gencode[Stelle2];
                gencode[Stelle2] = tmpValue;
                // gesamtliste wieder trennen
                var newTier = new Tierchen(this.Problem, GenCode);
                newTier.CompleteGenCode = gencode;
                return newTier;
            }
            else return this;
        }

        

        public double Bewerte()
        {
            Wert = Problem.Löse(GenCode);
            //Wert = Math.Sqrt( Math.Pow(funktion1(), 2) + Math.Pow(funktion2(), 2) + Math.Pow(funktion3(), 2));
            return Wert;
        }


        public static bool OnePointRecombination(Random randomizer, Tierchen mutter, Tierchen vater, out Tierchen kind1, out Tierchen kind2)
        {
            /* Ein-Punkt-Rekombination wählt zwei Tierchen sowie zufällig je ein Allel ihres Gens.
             * Kinder setzen sich aus einem Teil der Mutter/Vater sowie einem Teil des Vaters zusammen.
             * Übergeben den Kindern zunächst den vollständigen Gencode und Entferne anschließend
             * an einer zufälligen Stelle den übergebliebenen Code, füge anschließend einen anderen Code
             * (des Vaters/Mutter) in das gleiche Tierchen ein.
             */
            kind1 = null;
            kind2 = null;
            if (mutter.GenCode.Count() == vater.GenCode.Count() 
                && mutter.GenCode.Count() > 0 
                && mutter.CompleteGenCode.Count() == vater.CompleteGenCode.Count()
                && mutter.CompleteGenCode.Count() > 0 )
            {
                // Gib eine Folge von eindeutigen (unsortierten?) Allelen des jew. Tierchens aus
                // mehrere Allele in einem Tierchen sind möglich - müssen aber extra berücksichtigt werden
                var AllelSizeMutter = mutter.GenCode.Select(allel => allel.Size).Distinct();
                var AllelSizeVater = mutter.GenCode.Select(allel => allel.Size).Distinct();
                if (AllelSizeMutter.Count() == 1 && AllelSizeVater.Count() == 1 )
                {
                    var genCodeKind1 = mutter.CompleteGenCode;
                    var genCodeKind2 = vater.CompleteGenCode;
                    int splitIndex = randomizer.Next(0, genCodeKind1.Count());
                    int GenCodeSize = genCodeKind1.Count();
                    genCodeKind1.RemoveRange(splitIndex, GenCodeSize - splitIndex);
                    genCodeKind1.AddRange(vater.CompleteGenCode.GetRange(splitIndex, GenCodeSize - splitIndex));
                    genCodeKind2.RemoveRange(splitIndex, GenCodeSize - splitIndex);
                    genCodeKind2.AddRange(mutter.CompleteGenCode.GetRange(splitIndex, GenCodeSize - splitIndex));
                    kind1 = new Tierchen(mutter.Problem, mutter.GenCode);
                    kind1.CompleteGenCode = genCodeKind1;
                    kind2 = new Tierchen(vater.Problem, vater.GenCode);
                    kind2.CompleteGenCode = genCodeKind2;
                }
            }
            return false;
        }

        public static bool NPointRecombination(Random randomizer, Tierchen mutter, Tierchen vater, out Tierchen kind1, out Tierchen kind2, int recombinationPoints = 1)
        {
            /* N-Punkt-Rekombination wählt zwei Tierchen sowie zufällig je n Allele ihres Gens.
             * Kinder setzen sich aus einem Teil der Mutter/Vater sowie einem Teil des Vaters zusammen.
             * Übergeben den Kindern zunächst den vollständigen Gencode und Entferne anschließend
             * an einer zufälligen Stelle den übergebliebenen Code, füge anschließend einen anderen Code
             * (des Vaters/Mutter) in das gleiche Tierchen ein.
             */

            kind1 = null;
            kind2 = null;
            if (mutter.GenCode.Count() == vater.GenCode.Count()
                && mutter.GenCode.Count() > 0
                && mutter.CompleteGenCode.Count() == vater.CompleteGenCode.Count()
                && mutter.CompleteGenCode.Count() > 0)
            {


                if (recombinationPoints <= mutter.CompleteGenCode.Count())
                {
                    bool chooseFirstFromMother = true;
                    // Gib eine Folge von eindeutigen (unsortierten?) Allelen des jew. Tierchens aus
                    // mehrere Allele in einem Tierchen sind möglich - müssen aber extra berücksichtigt werden
                    var AllelSizeMutter = mutter.GenCode.Select(allel => allel.Size).Distinct();
                    var AllelSizeVater = mutter.GenCode.Select(allel => allel.Size).Distinct();
                    if (AllelSizeMutter.Count() == 1 && AllelSizeVater.Count() == 1)
                    {
                        var genCodeKind1 = mutter.CompleteGenCode;
                        var genCodeKind2 = vater.CompleteGenCode;

                        // erstelle Split-indizes
                        var SplitIndizes = new List<int>();
                        for (int i = 0; i < recombinationPoints; i++)
                        {
                            SplitIndizes.Add( randomizer.Next(i == 0?0:SplitIndizes.LastOrDefault()+1, genCodeKind1.Count()+1 - (recombinationPoints - i)));
                        }
                        int GenCodeSize = genCodeKind1.Count();
                        SplitIndizes.Sort();
                        // für jeden Splitindex  gencode zusammensetzen
                        foreach (var splitIndex in SplitIndizes)
                        {
                            if (chooseFirstFromMother)
                            {
                                genCodeKind1.RemoveRange(splitIndex, GenCodeSize - splitIndex);
                                genCodeKind1.AddRange(vater.CompleteGenCode.GetRange(splitIndex, GenCodeSize - splitIndex));
                                genCodeKind2.RemoveRange(splitIndex, GenCodeSize - splitIndex);
                                genCodeKind2.AddRange(mutter.CompleteGenCode.GetRange(splitIndex, GenCodeSize - splitIndex));
                            }
                            else 
                            {
                                genCodeKind1.RemoveRange(splitIndex, GenCodeSize - splitIndex);
                                genCodeKind1.AddRange(mutter.CompleteGenCode.GetRange(splitIndex, GenCodeSize - splitIndex));
                                genCodeKind2.RemoveRange(splitIndex, GenCodeSize - splitIndex);
                                genCodeKind2.AddRange(vater.CompleteGenCode.GetRange(splitIndex, GenCodeSize - splitIndex));
                            }
                            chooseFirstFromMother = !chooseFirstFromMother;
                        }
                        kind1 = new Tierchen(mutter.Problem, mutter.GenCode);
                        kind1.CompleteGenCode = genCodeKind1;
                        kind2 = new Tierchen(vater.Problem, vater.GenCode);
                        kind2.CompleteGenCode = genCodeKind2;
                    }
                }
            }
            return false;
        }

        public static Tierchen RandomTier(Random randomizer, int AllelLenght, List<Intervall> interval, int GenLenght, Problem problem)
        {
            var genCode = new List<Allel>();
            for (int i = 0; i < GenLenght; i++)
            {
                var allel = new Allel(AllelLenght, interval[i]);
                var allelVal = new List<byte>();
                for (int j = 0; j < AllelLenght; j++)
                {
                    allelVal.Add((byte)randomizer.Next(0,2));
                    
                }
                allel.Add(allelVal);
                genCode.Add(allel);
            }
            var tier = new Tierchen(problem, genCode);
            tier.Bewerte();
            return tier;
        }

        public override string ToString()
        {
            var text = "";
            foreach( var gen in GenCode)
            {
                text += gen.BinärCodeString + ", ";
            }
            text = text.Substring(0, text.Length - 2);
            return text;
        }

        public string ToNicerString()
        {
            var text = "";
            foreach (var gen in GenCode)
            {
                text += gen.DecimalValue.ToString("0.000") + "; ";
            }
            text = text.Substring(0, text.Length - 2) + ";" + Wert.ToString("####0.#####");
            return text;
        }

        public List<bool> CompleteGenCode
        {
            get 
            {
                var AllelSize = GenCode.Select( allel => allel.Size ).Distinct();
                if ( AllelSize.Count() == 1 )
                {
                    var allelLenght = AllelSize.FirstOrDefault();
                    var gencode = new List<bool>();
                    // alle allele in eine gemeinsame Liste packen
                    foreach (var bit in new List<Allel>(GenCode))
                    {
                        gencode.AddRange(bit.BinärCode);
                     }
                    return gencode;
                }
                return GenCode.SelectMany(allel => allel.BinärCode.Select(x => x)).ToList();
                
            }
            set 
            {
                var AllelSize = GenCode.Select( allel => allel.Size ).Distinct();
                var InterValList = GenCode.Select( allel => allel.Interval ).ToList();
                if (AllelSize.Count() == 1)
                {
                    var allelLenght = AllelSize.FirstOrDefault();
                    int i = 0;
                    var newGenCode = new List<Allel>();
                    while (i < value.Count())
                    {
                        newGenCode.Add(new Allel(value.GetRange(i, allelLenght), InterValList.GetRange(i / allelLenght, 1).FirstOrDefault()));
                        i += allelLenght;
                    }
                    this.GenCode = newGenCode;
                    this.Bewerte();
                }
            }
        }
    }
}