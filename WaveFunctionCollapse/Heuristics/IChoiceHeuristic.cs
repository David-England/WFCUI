using System;
using WaveFunctionCollapse.Data;

namespace WaveFunctionCollapse.Heuristics
{
    public interface IChoiceHeuristic
    {
        public int NextUnobservedNode(TileData tileData, PatternData patternData, GridData gridData, Random random);
    }
}