using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemOfEquations
{
    public class Interval
    {
        public int start;
        public int end;
        public static bool TryParse(string text, out Interval interval)
        {
            interval = null;
            interval = new Interval();
            if (text.Contains("[") || text.Contains("]"))
                text = text.Replace("[", "").Replace("]", "");
            var values = text.Split(',',';');
            if (values.Count() != 2 ) return false;
            var couldParsed = Int32.TryParse(values[0], out interval.start);
            couldParsed = couldParsed ? Int32.TryParse(values[1], out interval.end) : false;
            return couldParsed;
        }
    }
}
