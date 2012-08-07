using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemOfEquations
{
    public enum ProblemType
    {
        SystemOfEquations = 1,
        Griewank_Funktion = 2,
        Ackley_Funktion = 3,
        C_Funktion = 4
    }

    public class Problem
    {
        public ProblemType ProblemType;

        public Problem(ProblemType problem)
        {
            ProblemType = problem;
        }

        public Problem(int problem)
        {
            // TODO: Complete member initialization
            ProblemType = (ProblemType)problem;
        }
        // input: Nummer des Problems in der Problemtype auflistung
        public static bool TryParse(string input, out Problem problem)
        {
            int number;
            problem = null;
            if (Int32.TryParse(input, out number))
            {
                try
                {
                    var problemtype = (ProblemType)number;
                    problem = new Problem(problemtype);
                    return true;
                }
                catch (Exception)
                {
                    problem = null;
                }
            }
            return false;
        }

        public double Löse(List<Allel> GenCode)
        {
            switch ( ProblemType )
            {
                case ProblemType.SystemOfEquations:
                    return Math.Sqrt(Math.Pow(SystemOfEquations_funktion1(GenCode), 2) + Math.Pow(SystemOfEquations_funktion2(GenCode), 2) + Math.Pow(SystemOfEquations_funktion3(GenCode), 2));
                case ProblemType.Griewank_Funktion:
                    return Griewank_funktion1(GenCode);
                default:
                    return Math.Sqrt(Math.Pow(SystemOfEquations_funktion1(GenCode), 2) + Math.Pow(SystemOfEquations_funktion2(GenCode), 2) + Math.Pow(SystemOfEquations_funktion3(GenCode), 2));
            }
        }

        #region Funktionen des Gleichungssystem

        private double SystemOfEquations_funktion1(List<Allel> GenCode)
        {
            return Math.Pow(GenCode[0].DecimalValue, 2) + 2 * Math.Pow(GenCode[1].DecimalValue, 2) - 4;
        }
        private double SystemOfEquations_funktion2(List<Allel> GenCode)
        {
            return Math.Pow(GenCode[0].DecimalValue, 2) + 2 * Math.Pow(GenCode[1].DecimalValue, 2) + GenCode[2].DecimalValue - 8;
        }
        private double SystemOfEquations_funktion3(List<Allel> GenCode)
        {
            return Math.Pow(GenCode[0].DecimalValue - 1, 2) + Math.Pow(2 * GenCode[1].DecimalValue - Math.Sqrt(2), 2) + Math.Pow(GenCode[2].DecimalValue - 5, 2) - 4;
        }

        #endregion

        #region Griewank-Funktion

        private double Griewank_funktion1(List<Allel> GenCode)
        {
            double sum = GenCode.Sum(gen => (Math.Pow(gen.DecimalValue,2)/400*GenCode.Count()));
            double product = 0;
            int index = 1;
            foreach ( var gen in GenCode )
            {
                if (product == 0) product = Math.Cos(gen.DecimalValue / Math.Sqrt(index));
                else product *= Math.Cos(gen.DecimalValue / Math.Sqrt(index));
                index++;
            }
            return 1 + sum - product;
        }

        #endregion

        #region Ackley-Funktion

        private double Ackley_funktion1(List<Allel> GenCode)
        {
            return 20 
                 + Math.Exp(1) 
                 - 20 * Math.Exp( 
                       -0.2 * Math.Sqrt( 
                            (1/GenCode.Count()) * GenCode.Sum(gen => Math.Pow(gen.DecimalValue, 2)))
                 - Math.Exp((1 / GenCode.Count()) * GenCode.Sum(gen => Math.Cos(2 * Math.PI * gen.DecimalValue))));
        }

        #endregion

        #region C-Funktion

        private double C_funktion1(List<Allel> GenCode)
        {
            double result = 0.0;
            for(int i = 0; i < GenCode.Count()-1; i++)
            {
                for(int j = i+1; j < GenCode.Count(); j++)
                {
                    result += Math.Abs(GenCode[j].DecimalValue - GenCode[i].DecimalValue)/ (j-i);
                }
            }

            return 2 * result;
        }

        #endregion
    }
}
