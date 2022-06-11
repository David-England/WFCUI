using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            public string Heuristic
            {
                get
                {
                    return Config.Heuristic ?? "entropy";
                }
                set
                {
                    if (value != "") Config.Heuristic = value;
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
        public static string Heuristic { get; set; } = "entropy";
        public static int MaxAttempts { get; set; } = 10;
        public static int? Size { get; set; }
        public static int N { get; set; } = 3;

        public static ConfigInstance GetModder()
        {
            return new ConfigInstance();
        }
    }
}
