/*
The MIT License(MIT)
Copyright(c) mxgmn 2016.
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
The software is provided "as is", without warranty of any kind, express or implied, including but not limited to the warranties of merchantability, fitness for a particular purpose and noninfringement. In no event shall the authors or copyright holders be liable for any claim, damages or other liability, whether in an action of contract, tort or otherwise, arising from, out of or in connection with the software or the use or other dealings in the software.
*/

using System;
using WaveFunctionCollapse.Data;
using WaveFunctionCollapse.Heuristics;

namespace WaveFunctionCollapse
{
    abstract class Model
    {
        (int, int)[] stack;
        int stacksize;

        protected int T;

        double[] weightLogWeights;

        double sumOfWeights, sumOfWeightLogWeights, startingEntropy;
        double[] sumsOfWeights, sumsOfWeightLogWeights;

        public int N { get; init; }
        public TileData Tiles { get; init; } = new();
        public PatternData Patterns { get; init; }
        public GridData Grid { get; init; }
        public IChoiceHeuristic ChoiceHeuristic { get; init; }
        public IPatternHeuristic PatternHeuristic { get; init; }

        protected Model(int n, GridData grid, IChoiceHeuristic choiceHeuristic, IPatternHeuristic patternHeuristic)
        {
            N = n;
            Patterns = new PatternData(n);
            Grid = grid;
            ChoiceHeuristic = choiceHeuristic;
            PatternHeuristic = patternHeuristic;
        }

        void Init()
        {
            Tiles.WaveMatrix = new bool[Grid.X * Grid.Y][];
            Tiles.PossiblesCounter = new int[Tiles.WaveMatrix.Length][][];
            for (int i = 0; i < Tiles.WaveMatrix.Length; i++)
            {
                Tiles.WaveMatrix[i] = new bool[T];
                Tiles.PossiblesCounter[i] = new int[T][];
                for (int t = 0; t < T; t++) Tiles.PossiblesCounter[i][t] = new int[4];
            }
            Tiles.ObservedValues = new int[Grid.X * Grid.Y];

            weightLogWeights = new double[T];
            sumOfWeights = 0;
            sumOfWeightLogWeights = 0;

            for (int t = 0; t < T; t++)
            {
                weightLogWeights[t] = Patterns.Weights[t] * Math.Log(Patterns.Weights[t]);
                sumOfWeights += Patterns.Weights[t];
                sumOfWeightLogWeights += weightLogWeights[t];
            }

            startingEntropy = Math.Log(sumOfWeights) - sumOfWeightLogWeights / sumOfWeights;

            Tiles.AvailablePatternsCounter = new int[Grid.X * Grid.Y];
            sumsOfWeights = new double[Grid.X * Grid.Y];
            sumsOfWeightLogWeights = new double[Grid.X * Grid.Y];
            Tiles.Entropies = new double[Grid.X * Grid.Y];

            stack = new (int, int)[Tiles.WaveMatrix.Length * T];
            stacksize = 0;
        }

        public bool Run(int seed, int limit)
        {
            if (Tiles.WaveMatrix == null) Init();

            Clear();
            Random random = new Random(seed);

            for (int l = 0; l < limit || limit < 0; l++)
            {
                int node = ChoiceHeuristic.NextUnobservedNode(Tiles, Patterns, Grid, random);
                if (node >= 0)
                {
                    Observe(node, random);
                    bool success = Propagate();
                    if (!success) return false;
                }
                else
                {
                    for (int i = 0; i < Tiles.WaveMatrix.Length; i++) for (int t = 0; t < T; t++) if (Tiles.WaveMatrix[i][t]) { Tiles.ObservedValues[i] = t; break; }
                    return true;
                }
            }

            return true;
        }

        void Observe(int node, Random random)
        {
            var weightDistribution = new double[T];
            PatternHeuristic.GetPatternWeights(Tiles, Patterns, Grid).CopyTo(weightDistribution, 0);

            for (int t = 0; t < T; t++)
            {
                if (!Tiles.WaveMatrix[node][t])
                {
                    weightDistribution[t] = 0.0;
                }
            }

            int chosenNode = weightDistribution.Random(random.NextDouble());
            for (int t = 0; t < T; t++)
            {
                if (Tiles.WaveMatrix[node][t] != (t == chosenNode))
                {
                    Ban(node, t);
                }
            }
        }

        protected bool Propagate()
        {
            while (stacksize > 0)
            {
                (int i1, int t1) = stack[stacksize - 1];
                stacksize--;

                int x1 = i1 % Grid.X;
                int y1 = i1 / Grid.Y;

                for (int d = 0; d < 4; d++)
                {
                    int x2 = x1 + dx[d];
                    int y2 = y1 + dy[d];
                    if (!Grid.IsPeriodic && (x2 < 0 || y2 < 0 || x2 + N > Grid.X || y2 + N > Grid.Y)) continue;

                    if (x2 < 0) x2 += Grid.X;
                    else if (x2 >= Grid.X) x2 -= Grid.X;
                    if (y2 < 0) y2 += Grid.Y;
                    else if (y2 >= Grid.Y) y2 -= Grid.Y;

                    int i2 = x2 + y2 * Grid.X;
                    int[] p = Patterns.Adjacencies[d][t1];
                    int[][] compat = Tiles.PossiblesCounter[i2];

                    for (int l = 0; l < p.Length; l++)
                    {
                        int t2 = p[l];
                        int[] comp = compat[t2];

                        comp[d]--;
                        if (comp[d] == 0) Ban(i2, t2);
                    }
                }
            }

            return Tiles.AvailablePatternsCounter[0] > 0;
        }

        protected void Ban(int i, int t)
        {
            Tiles.WaveMatrix[i][t] = false;

            int[] comp = Tiles.PossiblesCounter[i][t];
            for (int d = 0; d < 4; d++) comp[d] = 0;
            stack[stacksize] = (i, t);
            stacksize++;

            Tiles.AvailablePatternsCounter[i] -= 1;
            sumsOfWeights[i] -= Patterns.Weights[t];
            sumsOfWeightLogWeights[i] -= weightLogWeights[t];

            double sum = sumsOfWeights[i];
            Tiles.Entropies[i] = Math.Log(sum) - sumsOfWeightLogWeights[i] / sum;
        }

        protected virtual void Clear()
        {
            for (int i = 0; i < Tiles.WaveMatrix.Length; i++)
            {
                for (int t = 0; t < T; t++)
                {
                    Tiles.WaveMatrix[i][t] = true;
                    for (int d = 0; d < 4; d++) Tiles.PossiblesCounter[i][t][d] = Patterns.Adjacencies[opposite[d]][t].Length;
                }

                Tiles.AvailablePatternsCounter[i] = Patterns.Weights.Length;
                sumsOfWeights[i] = sumOfWeights;
                sumsOfWeightLogWeights[i] = sumOfWeightLogWeights;
                Tiles.Entropies[i] = startingEntropy;
                Tiles.ObservedValues[i] = -1;
            }
            Tiles.ObservedSoFar = 0;
        }

        public abstract System.Drawing.Bitmap Graphics();

        protected static int[] dx = { -1, 0, 1, 0 };
        protected static int[] dy = { 0, 1, 0, -1 };
        static int[] opposite = { 2, 3, 0, 1 };
    }
}
