using System;
using WaveFunctionCollapse.Data;

namespace WaveFunctionCollapse.Heuristics
{
    /// <summary>
    /// Chooses an unobserved node by finding one with the minimum remaining values (MRV)--the fewest remaining possible patterns.
    /// </summary>
    public class MRVHeuristic : IChoiceHeuristic
    {
        public int NextUnobservedNode(TileData tiles, PatternData patterns, GridData grid, Random random)
        {
            double min = 1E+4;
            int argmin = -1;
            int n = patterns.N;

            for (int i = 0; i < tiles.WaveMatrix.Length; i++)
            {
                if (!grid.IsPeriodic && (i % grid.X + n > grid.X || i / grid.X + n > grid.Y)) continue;
                if (tiles.AvailablePatternsCounter[i] > 1 && tiles.AvailablePatternsCounter[i] <= min)
                {
                    double noise = 1E-6 * random.NextDouble();
                    if (tiles.AvailablePatternsCounter[i] + noise < min)
                    {
                        min = tiles.AvailablePatternsCounter[i] + noise;
                        argmin = i;
                    }
                }
            }
            return argmin;
        }
    }
}
