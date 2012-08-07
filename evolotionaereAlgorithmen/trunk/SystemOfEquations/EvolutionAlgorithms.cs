using System;
using System.Linq;
using System.Collections.Generic;

namespace SystemOfEquations
{

public class EvolutionAlgorithms
{
    #region Rekombination/Mutation

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
            Tierchen.NPointRecombination(elternGen[index1], elternGen[index2], out Kind1, out Kind2, pointOfRecombination);
            kindGen.Add(Kind1);
            kindGen.Add(Kind2);
            kindGen = kindGen.Distinct(new TierchenComparer()).ToList();
        }
        return kindGen;
    }

    #endregion
    #region Selektionen

    public static List<Tierchen> commaSelection(List<Tierchen> Kinder, Wahlverfahren wahl)
    {
        Random randomizer = new Random();
        var newEltern = new List<Tierchen>();
        int anzahlKinder = Kinder.Count;
        int anzahlNeueEltern = randomizer.Next(anzahlKinder / 7, anzahlKinder / 5);
        // wähle nächste Eltern
        var tmpKinder = Kinder;
        if (wahl == Wahlverfahren.determenistic //)
           || wahl == Wahlverfahren.roulette)
        {
            //tmpKinder = tmpKinder.OrderBy(tier => tier.Wert).ToList();
            //foreach (var tier in tmpKinder)
            //{
            //    newEltern.Add(tier);
            //    if (anzahlKinder / 7 <= newEltern.Count && newEltern.Count <= anzahlKinder / 5)
            //        break;
            //}
        }
        //else
        //{
            while (true)
            {
                addChild(newEltern, tmpKinder, wahl);
                if ( anzahlNeueEltern >= newEltern.Count)
                    break;
            }
        //}
        return newEltern;
    }

    public static List<Tierchen> plusSelection(List<Tierchen> Eltern, List<Tierchen> Kinder, Wahlverfahren wahl)
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
            while (true)
            {
                addChild(newEltern, tmpKinder, wahl);
                if (newEltern.Count >= anzahlEltern)
                    break;
            }
        //}
        return newEltern;
    }

    #endregion

    #region Auswahlverfahren

    private static void addChild(List<Tierchen> newEltern, List<Tierchen> tmpKinder, Wahlverfahren wahl)
    {
        switch (wahl)
        {
            case Wahlverfahren.determenistic:
                newEltern.Add(tmpKinder[newEltern.Count]);
                break;
            case Wahlverfahren.roulette:
                newEltern.Add(chooseChildRoulette(tmpKinder));
                break;
        }
    }

    private static Tierchen chooseChildRoulette(List<Tierchen> tmpKinder)
    {
        Random randomizer = new Random();
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
}
}