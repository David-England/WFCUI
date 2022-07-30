/*
The MIT License(MIT)
Copyright(c) mxgmn 2016.
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
The software is provided "as is", without warranty of any kind, express or implied, including but not limited to the warranties of merchantability, fitness for a particular purpose and noninfringement. In no event shall the authors or copyright holders be liable for any claim, damages or other liability, whether in an action of contract, tort or otherwise, arising from, out of or in connection with the software or the use or other dealings in the software.
*/

using System;
using System.Drawing;
using WaveFunctionCollapse.Data;
using WaveFunctionCollapse.Heuristics;

namespace WaveFunctionCollapse
{
    public static class Top
    {
        public static Bitmap Fire(string name, IChoiceHeuristic choiceHeuristic, IPatternHeuristic patternHeuristic, bool isOverlapping = true,
            int? height = null, int limit = -1, int maxAttempts = 10, bool isPeriodic = false, int? size = null, bool isTextOutput = false, int? width = null,
            /* overlapping model params */ int ground = 0, int n = 3, bool isPeriodicInput = true, int symmetry = 8,
            /* simpletiled model params */ bool isBlackBackground = false, string subset = null)
        {
            Model m;
            SetNullables();

            if (isOverlapping)
            {
                m = new OverlappingModel(name, n, new GridData(width.Value, height.Value, isPeriodic), choiceHeuristic, patternHeuristic, isPeriodicInput, symmetry, ground);
            }
            else
            {
                m = new SimpleTiledModel(name, subset, new GridData(width.Value, height.Value, isPeriodic), choiceHeuristic, patternHeuristic, isBlackBackground);
            }

            return RunModelUntilSuccess(m);

            void SetNullables()
            {
                size ??= (isOverlapping ? 48 : 24);

                height ??= size;
                width ??= size;
            }

            Bitmap RunModelUntilSuccess(Model model)
            {
                Random random = new Random();
                
                for (int attempt = 0; attempt < maxAttempts; attempt++)
                {
                    bool success = model.Run(random.Next(), limit);

                    if (success)
                    {
                        return model.Graphics();
                    }
                }

                throw new Exception();
            }
        }
    }
}
