using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemOfEquations
{
    public class Intervall
    {
        public int start;
        public int end;

        public Intervall(){}

        public Intervall(int inStart, int inEnd)
        {
            // TODO: Complete member initialization
            this.start = inStart;
            this.end = inEnd;
        }
        public static bool TryParse(string text, out Intervall interval)
        {
            interval = null;
            interval = new Intervall();
            if (text.Contains("[") || text.Contains("]"))
                text = text.Replace("[", "").Replace("]", "");
            var values = text.Split(',',';');
            if (values.Count() != 2 ) return false;
            var couldParsed = Int32.TryParse(values[0], out interval.start);
            couldParsed = couldParsed ? Int32.TryParse(values[1], out interval.end) : false;
            return couldParsed;
        }

        public override string ToString()
        {
            return String.Format("[{0},{1}]",start, end);
        }
    }
}
