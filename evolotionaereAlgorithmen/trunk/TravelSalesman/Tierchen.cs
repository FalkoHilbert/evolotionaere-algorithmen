using System;
using System.Collections.Generic;
using System.Linq;

namespace TravelSalesman
{

    public class Tierchen
    {
        public List<int> GenCode;
        public Dictionary<int, List<int>> neighbours = new Dictionary<int, List<int>>();
        public int Wert;

        public string GenCodeString
        {
            get
            {
                var GenCode = "";
                foreach (var item in this.GenCode)
                {
                    GenCode += string.Format("{0}", item);
                }
                return GenCode;
            }
        }

        public void BuildNeighbours()
        {
            neighbours = null;
            neighbours = new Dictionary<int, List<int>>();
            for (int index = 0; index < GenCode.Count; index++)
            {
                neighbours.Add(GenCode[index], getNeighbours(index));
            }
        }

        private List<int> getNeighbours(int index)
        {
            var Neighbours = new List<int>();
            Neighbours.Add(GenCode[(index + 1 + (GenCode.Count - 2)) % GenCode.Count]);
            Neighbours.Add(GenCode[(index + 1) % GenCode.Count]);
            return Neighbours;
        }

        public void Mutation()
        {
            Random randomizer = new Random();
            int Stelle1 = randomizer.Next(GenCode.Count - 2);
            int Stelle2 = randomizer.Next(Stelle1 + 1, GenCode.Count - 1);
            GenCode.Reverse(Stelle1, Stelle2 - Stelle1);
            this.BuildNeighbours();
        }

        public void InzestMutation()
        {
            Random randomizer = new Random();
            int Stelle1 = randomizer.Next(GenCode.Count - 2);
            int Stelle2 = randomizer.Next(Stelle1 + 1, GenCode.Count - 1);
            int tmpValue = GenCode[Stelle1];
            GenCode[Stelle1] = GenCode[Stelle2];
            GenCode[Stelle2] = tmpValue;
            this.BuildNeighbours();
        }

        public Tierchen(List<int> GenCode)
        {
            if (GenCode.Count != 0)
            {
                this.GenCode = GenCode;
                this.BuildNeighbours();
            }
        }

        public void Bewerte(Dictionary<int, Dictionary<int, int>> bewertungsMatrix)
        {
            int lastGen = 0;
            for (int gen = 0; gen < GenCode.Count - 1; gen++)
            {
                Wert += bewertungsMatrix[GenCode[gen]][GenCode[gen + 1]];
                lastGen = gen + 1;
            }
            Wert += bewertungsMatrix[GenCode[lastGen]][GenCode[0]];
        }

    }
}