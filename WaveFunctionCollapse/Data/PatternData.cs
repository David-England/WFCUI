using System;

namespace WaveFunctionCollapse.Data
{
    public class PatternData
    {
        public int N { get; set; }
        public int[][][] Adjacencies { get; set; }
        public double[] Weights { get; set; }

        public PatternData(int n)
        {
            N = n;
        }
    }
}
