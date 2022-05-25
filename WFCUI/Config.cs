using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFCUI
{
    internal static class Config
    {
        public static bool IsOverlapping { get; set; } = true;
        public static int? Height { get; set; } = null;
        public static string Heuristic { get; set; } = "entropy";
        public static int Limit { get; set; } = -1;
        public static int MaxAttempts { get; set; } = 10;
        public static bool IsPeriodic { get; set; } = false;
        public static int? Size { get; set; } = null;
        public static bool IsTextOutput { get; set; } = false;
        public static int? Width { get; set; } = null;
        public static int Ground { get; set; } = 0;
        public static int N { get; set; } = 3;
        public static bool IsPeriodicInput { get; set; } = true;
        public static int Symmetry { get; set; } = 8;
        public static bool IsBlackBackground { get; set; } = false;
        public static string Subset { get; set; } = null;
    }
}
