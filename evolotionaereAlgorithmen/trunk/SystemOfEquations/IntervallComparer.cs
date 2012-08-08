using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace SystemOfEquations
{
    public class IntervallComparer : IEqualityComparer<Intervall>
    {
        public bool Equals(Intervall x, Intervall y)
        {
            return Intervall.ReferenceEquals(x, y) || x.ToString().Equals(y.ToString());
        }

        public int GetHashCode(Intervall obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}