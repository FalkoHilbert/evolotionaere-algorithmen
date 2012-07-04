using System;
using System.Linq;
using System.Collections.Generic;

namespace TravelSalesman
{

public class EvolutionAlgorithms
{
    public static Tierchen Paaren(Tierchen A, Tierchen B)
    {
        var newDNA = new List<int>();
        var adjunkte = new Dictionary<int, List<int>>();
        if (A.GenCode.Count == B.GenCode.Count)
        {
            for (int index = 0; index < A.GenCode.Count; index++)
            {
                adjunkte.Add(index, A.neighbours[index].Union(B.neighbours[index]).Distinct().OrderBy(x => x).ToList());
            }
            Random randomizer = new Random();
            int start = randomizer.Next(0, 2);
            newDNA.Add(start == 0 ? A.GenCode[0] : B.GenCode[0]);
            for (int index = 0; index < A.GenCode.Count - 1; index++)
            {
                var möglicheAlleleWithCount = adjunkte[newDNA[index]].Where(nab => !newDNA.Contains(nab))
                                                      .Select(nab => new { nab = nab, count = adjunkte[nab].Count(x => !newDNA.Contains(x)) })
                                                      .OrderBy(x => x.count)
                                                      .ToList();
                var minCount = möglicheAlleleWithCount.Min(y => y.count);
                var möglicheAllele = möglicheAlleleWithCount.Where(x => x.count == minCount)
                                               .Select(x => x.nab)
                                               .ToList();
                int allelIndex = randomizer.Next(0, möglicheAllele.Count);
                newDNA.Add(möglicheAllele[allelIndex]);
            }
        }
        return new Tierchen(newDNA);



    }

    #region Selektionen

    public static List<Tierchen> commaSelection(List<Tierchen> Kinder, Wahlverfahren wahl)
    {
        var newEltern = new List<Tierchen>();
        int anzahlKinder = Kinder.Count;
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
                if (anzahlKinder / 7 <= newEltern.Count && newEltern.Count <= anzahlKinder / 5)
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