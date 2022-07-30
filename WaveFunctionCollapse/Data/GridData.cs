using System;

namespace WaveFunctionCollapse.Data
{
    public class GridData
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsPeriodic { get; set; }

        public GridData(int x, int y, bool isPeriodic)
        {
            X = x;
            Y = y;
            IsPeriodic = isPeriodic;
        }
    }
}
