using System;

namespace WaveFunctionCollapse.Data
{
    public class TileData
    {
        public bool[][] WaveMatrix { get; set; }
        public int[] ObservedValues { get; set; }
        public int ObservedSoFar { get; set; }      // used only by ScanlineHeuristic
        public int[][][] PossiblesCounter { get; set; }

        public int[] AvailablePatternsCounter { get; set; }
        public double[] Entropies { get; set; }
    }
}
