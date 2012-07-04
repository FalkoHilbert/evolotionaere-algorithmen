using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace TravelSalesman
{
    public class GenCodeComparer : IEqualityComparer<Tierchen>
    {
        public bool Equals(Tierchen x, Tierchen y)
        {
            return x.GenCodeString == y.GenCodeString;
        }

        public int GetHashCode(Tierchen obj)
        {
            return obj.GenCodeString.GetHashCode();
        }
    }
}
