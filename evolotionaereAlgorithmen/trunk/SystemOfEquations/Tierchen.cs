using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemOfEquations
{

    public class Tierchen
    {
        public List<Allel> GenCode;
        public double Wert;

        public Tierchen Mutation()
        {
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
                var newTier =  new Tierchen(GenCode);
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
                var newTier = new Tierchen(GenCode);
                newTier.CompleteGenCode = gencode;
                return newTier;
            }
            else return this;
        }

        public Tierchen(List<Allel> genCode)
        {
            if (genCode.Count != 0)
            {
                this.GenCode = genCode;
                this.Bewerte();
            }
        }

        public double Bewerte()
        {
            Wert = Math.Sqrt( Math.Pow(funktion1(), 2) + Math.Pow(funktion2(), 2) + Math.Pow(funktion3(), 2));
            return Wert;
        }

        private double funktion1()
        {
            return Math.Pow(GenCode[0].DecimalValue, 2) + 2 * Math.Pow(GenCode[1].DecimalValue, 2) - 4;
        }
        private double funktion2()
        {
            return Math.Pow(GenCode[0].DecimalValue, 2) + 2 * Math.Pow(GenCode[1].DecimalValue, 2) + GenCode[2].DecimalValue - 8;
        }
        private double funktion3()
        {
            return Math.Pow(GenCode[0].DecimalValue - 1, 2) + Math.Pow(2 * GenCode[1].DecimalValue - Math.Sqrt(2), 2) + Math.Pow(GenCode[2].DecimalValue - 5, 2) - 4;
        }

        public static bool OnePointRecombination(Tierchen mutter, Tierchen vater, out Tierchen kind1, out Tierchen kind2)
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
                // wähle Zahl z
                Random randomizer = new Random();

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
                    kind1 = new Tierchen( mutter.GenCode);
                    kind1.CompleteGenCode = genCodeKind1;
                    kind2 = new Tierchen( vater.GenCode);
                    kind2.CompleteGenCode = genCodeKind2;
                }
            }
            return false;
        }

        public static Tierchen RandomTier(int AllelLenght, List<Intervall> interval, int GenLenght )
        {
            // generate Gen
            Random randomizer = new Random();
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
            var tier = new Tierchen(genCode);
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
                text += gen.DecimalValue.ToString("0.000") + ", ";
            }
            text = "(" + text.Substring(0, text.Length - 2) + ") | Wert:\t" + Wert.ToString("####0.#####");
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