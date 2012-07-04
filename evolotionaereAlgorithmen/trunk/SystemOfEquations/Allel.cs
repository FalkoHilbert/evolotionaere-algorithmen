using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemOfEquations
{
    public class Allel
    {
        public List<bool> BinärCode = new List<bool>();
        private int m_MaxLenght;
        private Interval m_interval;

        public Allel(int MaxLenght, Interval interval )
        {
            m_MaxLenght = MaxLenght;
            m_interval = interval;
            BinärCode.Capacity = MaxLenght;
        }

        public Allel(List<bool> list, Interval interval)
        {
            // TODO: Complete member initialization
            m_MaxLenght = list.Count();
            m_interval = interval;
            BinärCode.Capacity = m_MaxLenght;
            this.BinärCode = list;
        }

        public void Add(List<bool> binärCode)
        {
            if (binärCode.Count == m_MaxLenght )
            {
                BinärCode = binärCode;
            }
        }

        public void Add(List<byte> binärCode)
        {
            var temp = new List<bool>();
            foreach (var item in binärCode)
            {
                temp.Add(item == 1 ? true : false);
            }
            this.Add(temp);
        }

        public string BinärCodeString
        {
            get 
            {
                string code ="";
                foreach(var item in BinärCode )
                {
                    code += (item ? 1 : 0).ToString();
                }

                return code;
            }
        }

        public double DecimalValue
        {
            get 
            {
                double value = 0.0;
                for(int j = 0; j < m_MaxLenght; j++)
                {
                    value += (BinärCode[m_MaxLenght-j - 1] ? 1 : 0) * Math.Pow(2, j);
                }

                return m_interval.start + ( this.Granularity * value);
            }
        }

        public double Granularity
        {
            get
            {
                return (m_interval.end - m_interval.start) / (Math.Pow(2, m_MaxLenght) - 1);
            }
        }

        public int Size 
        {
            get 
            {
                if (BinärCode.Count() < m_MaxLenght) return m_MaxLenght;
                else return BinärCode.Count();
            }
        }
        
        public Interval Interval
        {
            get
            {
                return m_interval;
            }
        }
    }
}
