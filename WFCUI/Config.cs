using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveFunctionCollapse.Heuristics;

namespace WFCUI
{
    internal static class Config
    {
        internal class ConfigInstance
        {
            public bool IsOverlapping
            {
                get
                {
                    return Config.IsOverlapping;
                }
                set
                {
                    Config.IsOverlapping = value;
                }
            }

            public string ChoiceHeuristic
            {
                get
                {
                    switch (Config.ChoiceHeuristic)
                    {
                        case EntropyHeuristic:
                            return "entropy";
                        case MRVHeuristic:
                            return "mrv";
                        case ScanlineHeuristic:
                            return "scanline";
                        default:
                            throw new Exception("Unrecognised choice heuristic");
                    }
                }
                set
                {
                    switch (value)
                    {
                        case "entropy":
                            Config.ChoiceHeuristic = new EntropyHeuristic();
                            break;
                        case "mrv":
                            Config.ChoiceHeuristic = new MRVHeuristic();
                            break;
                        case "scanline":
                            Config.ChoiceHeuristic = new ScanlineHeuristic();
                            break;
                        case "":
                            Config.ChoiceHeuristic = new EntropyHeuristic();
                            break;
                        default:
                            throw new Exception("We didn't recognise that choice heuristic.");
                    }
                }
            }

            public string PatternHeuristic
            {
                get
                {
                    switch (Config.PatternHeuristic)
                    {
                        case EntropyHeuristic:
                            return "entropy";
                        default:
                            throw new Exception("Unrecognised choice heuristic");
                    }
                }
                set
                {
                    switch (value)
                    {
                        case "entropy":
                            Config.PatternHeuristic = new EntropyHeuristic();
                            break;
                        case "":
                            Config.PatternHeuristic = new EntropyHeuristic();
                            break;
                        default:
                            throw new Exception("We didn't recognise that choice heuristic.");
                    }
                }
            }

            public int? MaxAttempts
            {
                get
                {
                    return Config.MaxAttempts;
                }
                set
                {
                    if (value.HasValue) Config.MaxAttempts = value.Value;
                }
            }

            public int? Size
            {
                get
                {
                    return Config.Size ?? 48;
                }
                set
                {
                    if (value.HasValue) Config.Size = value.Value;
                }
            }

            public int? N
            {
                get
                {
                    return Config.N;
                }
                set
                {
                    if (value.HasValue) Config.N = value.Value;
                }
            }
        }

        public static bool IsOverlapping { get; set; } = true;
        public static IChoiceHeuristic ChoiceHeuristic { get; set; } = new EntropyHeuristic();
        public static IPatternHeuristic PatternHeuristic { get; set; } = new EntropyHeuristic();
        public static int MaxAttempts { get; set; } = 10;
        public static int? Size { get; set; }
        public static int N { get; set; } = 3;

        public static ConfigInstance GetModder()
        {
            return new ConfigInstance();
        }
    }
}
