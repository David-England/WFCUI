using System;
using WaveFunctionCollapse.Data;

namespace WaveFunctionCollapse.Heuristics
{
    public class ScanlineHeuristic : IChoiceHeuristic
    {
        public int NextUnobservedNode(TileData tiles, PatternData patterns, GridData grid, Random random)
        {
            int n = patterns.N;

            for (int i = tiles.ObservedSoFar; i < tiles.WaveMatrix.Length; i++)
            {
                if (!grid.IsPeriodic && (i % grid.X + n > grid.X || i / grid.X + n > grid.Y)) continue;
                if (tiles.AvailablePatternsCounter[i] > 1)
                {
                    tiles.ObservedSoFar = i + 1;
                    return i;
                }
            }
            return -1;
        }
    }
}