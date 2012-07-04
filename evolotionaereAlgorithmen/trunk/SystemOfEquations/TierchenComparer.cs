using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace SystemOfEquations
{
    public class TierchenComparer : IEqualityComparer<Tierchen>
    {
        public bool Equals(Tierchen x, Tierchen y)
        {
            return Tierchen.ReferenceEquals(x, y) || x.ToString().Equals(y.ToString());
        }

        public int GetHashCode(Tierchen obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}