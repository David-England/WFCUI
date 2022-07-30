using System;
using WaveFunctionCollapse.Data;

namespace WaveFunctionCollapse.Heuristics
{
    public interface IPatternHeuristic
    {
        public double[] GetPatternWeights(TileData tileData, PatternData patternData, GridData gridData);
    }
}