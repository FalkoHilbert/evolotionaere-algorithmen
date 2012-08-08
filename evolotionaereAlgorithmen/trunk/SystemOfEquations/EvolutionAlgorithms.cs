using System;
using System.Linq;
using System.Collections.Generic;

namespace SystemOfEquations
{

public class EvolutionAlgorithms
{
    #region Rekombination/Mutation

    private static int MAX_VERSUCHE_ZUM_FINDEN_NICHT_DOPPELTEN = 10;

    public static List<Tierchen> mutiereKinder(Random randomizer, List<Tierchen> kindGen, List<Tierchen> elternGen, int countOfMutations)
    {
        while (kindGen.Count() < countOfMutations)
        {
            var index1 = randomizer.Next(0, elternGen.Count);
            var wahrscheinlichkeit = randomizer.NextDouble();
            Tierchen kind;

            if (wahrscheinlichkeit <= 0.3)
            {
                kind = elternGen[index1].InzestMutation();
            }
            else
            {
                kind = elternGen[index1].Mutation();
            }

            kindGen.Add(kind);
            kindGen = kindGen.Distinct(new TierchenComparer()).ToList();
        }
        return kindGen;
    }

    public static List<Tierchen> NPunktRekombination(Random randomizer, int recombinations, int pointOfRecombination, List<Tierchen> kindGen, List<Tierchen> elternGen)
    {
        // Füge zur Kindgeneration zwei Kinder über eine Ein-Punkt-Rekombination hinzu
        // Wiederhole solange bis die Anzahl der Rekombinationen erreicht
        bool verbieteDoppelte = true;
        int lastCount = 0;
        int versuchsZähler = 0;
        while (kindGen.Count() < recombinations)
        {
            var index1 = randomizer.Next(0, elternGen.Count);
            var index2 = index1;
            while (index2 == index1)
            {
                index2 = randomizer.Next(0, elternGen.Count);
            }

            Tierchen Kind1;
            Tierchen Kind2;
            Tierchen.NPointRecombination(randomizer, elternGen[index1], elternGen[index2], out Kind1, out Kind2, pointOfRecombination);
            kindGen.Add(Kind1);
            kindGen.Add(Kind2);
            if (verbieteDoppelte)
            {
                kindGen = kindGen.Distinct(new TierchenComparer()).ToList();
                if (kindGen.Count() == lastCount)
                    if (versuchsZähler <= MAX_VERSUCHE_ZUM_FINDEN_NICHT_DOPPELTEN) versuchsZähler++;
                    else verbieteDoppelte = false;
                else versuchsZähler = 0;
            }
            lastCount = kindGen.Count();
        }
        return kindGen;
    }

    #endregion
    #region Selektionen

    public static List<Tierchen> commaSelection(Random randomizer, List<Tierchen> Kinder, Wahlverfahren wahl)
    {
        var newEltern = new List<Tierchen>();
        int anzahlKinder = Kinder.Count;
        int anzahlNeueEltern = randomizer.Next(anzahlKinder / 7, anzahlKinder / 5);
        // wähle nächste Eltern
        var tmpKinder = Kinder;
        if (wahl == Wahlverfahren.determenistic //)
           || wahl == Wahlverfahren.roulette)
        {
            tmpKinder = tmpKinder.OrderBy(tier => tier.Wert).ToList();
            //foreach (var tier in tmpKinder)
            //{
            //    newEltern.Add(tier);
            //    if (anzahlKinder / 7 <= newEltern.Count && newEltern.Count <= anzahlKinder / 5)
            //        break;
            //}
        }
        //else
        //{
            while (anzahlNeueEltern > newEltern.Count)
            {
                addChild( randomizer, newEltern, tmpKinder, wahl);
            }
        //}
        return newEltern;
    }

    public static List<Tierchen> plusSelection(Random randomizer, List<Tierchen> Eltern, List<Tierchen> Kinder, Wahlverfahren wahl)
    {
        var newEltern = new List<Tierchen>();
        int anzahlKinder = Kinder.Count;
        int anzahlEltern = Eltern.Count;
        // wähle nächste Eltern
        var tmpKinder = Kinder.Union(Eltern).ToList();
        if (wahl == Wahlverfahren.determenistic //)
           || wahl == Wahlverfahren.roulette)
        {
            tmpKinder = tmpKinder.OrderBy(tier => tier.Wert).ToList();
        //    foreach (var tier in tmpKinder)
        //    {
        //        newEltern.Add(tier);
        //        if (newEltern.Count >= anzahlEltern)
        //            break;
        //    }
        }
        //else
        //{
            while (anzahlEltern > newEltern.Count)
            {
                addChild(randomizer, newEltern, tmpKinder, wahl);
            }
        //}
        return newEltern;
    }

    #endregion

    #region Auswahlverfahren

    private static void addChild(Random randomizer, List<Tierchen> newEltern, List<Tierchen> tmpKinder, Wahlverfahren wahl)
    {
        switch (wahl)
        {
            case Wahlverfahren.zufall:
                newEltern.Add(chooseChildRandom( randomizer, tmpKinder));
                break;
            case Wahlverfahren.determenistic:
                newEltern.Add(tmpKinder[newEltern.Count]);
                break;
            case Wahlverfahren.roulette:
                newEltern.Add(chooseChildRoulette( randomizer, tmpKinder));
                break;
        }
    }

    private static Tierchen chooseChildRoulette(Random randomizer, List<Tierchen> tmpKinder)
    {
        //var childs = tmpKinder.OrderBy(tier => tier.Wert ).ToList();
        //Bestimme Zufallszahl z 2 [0; 1]
        // Wahle Ai e P(t) genau dann, wenn
        //summe(j=0, j<i, P(Aj) ) <= z < summe(j=0, j<=i, P(Aj) ) 
        var childs = tmpKinder;
        int index = randomizer.Next(0, childs.Count());
        return childs[index];
    }

    private static Tierchen chooseChildRandom(Random randomizer, List<Tierchen> tmpKinder)
    {
        //var childs = tmpKinder.OrderBy(tier => tier.Wert ).ToList();
        var childs = tmpKinder;
        int index = randomizer.Next(0, childs.Count());
        return childs[index];
    }

    #endregion

    public static Double getSelectionPressure(IList<Tierchen> Population)
    {
        return 0.0;
    }

    public static int CalculateMutations(MutationType mutationType, int mutationRateStart, int mutationRateEnd, int countOfChilds, int generationCount, int generation)
    {
        int initialMutationsByRate = countOfChilds * mutationRateStart / 100;
        int endMutationsByRate = countOfChilds * mutationRateEnd / 100;
        switch (mutationType)
        {
            case MutationType.konstant:
                return initialMutationsByRate;
            case MutationType.linear:
                return initialMutationsByRate + ((endMutationsByRate - initialMutationsByRate) / generationCount) * generation;
            case MutationType.exponentiel:
                return (int)(initialMutationsByRate * Math.Exp(Math.Log(endMutationsByRate/initialMutationsByRate) ) * generation);
            case MutationType.potenziert:
                return 0;
        }
        return initialMutationsByRate;
    }
}
}