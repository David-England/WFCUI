using System;
using WaveFunctionCollapse.Data;

namespace WaveFunctionCollapse.Heuristics
{
    /// <summary>
    /// Chooses an unobserved node according to the Shannon entropy; selects a pattern for that node using a weighted sum of the remaining possible patterns.
    /// </summary>
    public class EntropyHeuristic : IChoiceHeuristic, IPatternHeuristic
    {
        public int NextUnobservedNode(TileData tiles, PatternData patterns, GridData grid, Random random)
        {
            double min = 1E+4;
            int argmin = -1;
            int n = patterns.N;

            for (int i = 0; i < tiles.WaveMatrix.Length; i++)
            {
                if (!grid.IsPeriodic && (i % grid.X + n > grid.X || i / grid.X + n > grid.Y)) continue;
                if (tiles.AvailablePatternsCounter[i] > 1 && tiles.Entropies[i] <= min)
                {
                    double noise = 1E-6 * random.NextDouble();
                    if (tiles.Entropies[i] + noise < min)
                    {
                        min = tiles.Entropies[i] + noise;
                        argmin = i;
                    }
                }
            }
            return argmin;
        }

        public double[] GetPatternWeights(TileData tileData, PatternData patternData, GridData gridData)
        {
            return patternData.Weights;
        }
    }
}