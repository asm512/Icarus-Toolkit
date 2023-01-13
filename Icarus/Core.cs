using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icarus
{
    public class Core
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

        internal static Dictionary<int, (int min, int max)> XPRanges = new Dictionary<int, (int min, int max)>
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
            {99, (600001, 999999999) }
        };
    }
}
