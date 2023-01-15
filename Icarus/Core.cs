using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icarus
{
    public static class Core
    {
        public static int GetPlayerLevel(int xp)
        {
            foreach (var kvp in XPRanges)
            {
                if (xp >= kvp.Value.min && xp <= kvp.Value.max)
                {
                    return kvp.Key;
                }
            }
            return -1;
        }

        public static Dictionary<int, (int min, int max)> XPRanges = new Dictionary<int, (int min, int max)>
        {
            {0, (0, 0)},
            {1, (1, 2400)},
            {2, (2401, 8610)},
            {3, (8611, 18730)},
            {4, (18731, 32530)},
            {5, (32531, 48630)},
            {6, (48631, 67830)},
            {7, (67831, 89430)},
            {8, (89431, 111030)},
            {9, (111031, 135030)},
            {10, (135031, 161500)},
            {11, (161501, 194400)},
            {12, (194401, 227300)},
            {13, (227301, 260200)},
            {14, (260201, 293100)},
            {15, (293101, 326000)},
            {16, (326001, 380800)},
            {17, (380801, 435600)},
            {18, (435601, 490400)},
            {19, (490401, 545200)},
            {20, (545201, 600000)},
            {25, (600001, 975000)},
            {30, (975001, 1400000)},
            {35, (1400001, 1942000)},
            {40, (1942001, 2550000)},
            {45, (2550001, 3200000)},
            {50, (3200001, 3890000)},
            {55, (3890001, 4625000)},
            {60, (4625001, 5400000)},
            {51, (5400001, 999999999) }
        };
    }
}
