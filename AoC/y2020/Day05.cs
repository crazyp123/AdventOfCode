using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2020
{
    public class Day05 : Day
    {
        public override object Result1()
        {
            return Input.AsListOf<string>().Max(GetId);
        }

        private static int GetId(string code)
        {
            var froRow = 0;
            var toRow = 127;

            var froCol = 0;
            var toCol = 7;

            foreach (var inst in code)
            {
                var hR = (toRow - (double)froRow) / 2d;
                var hC = (toCol - (double) froCol) / 2d;

                switch (inst)
                {
                    case 'F':
                        toRow = (int)Math.Floor(toRow - hR);
                        break;
                    case 'B':
                        froRow = (int)Math.Ceiling(toRow - hR);
                        break;
                    case 'R':
                        froCol = (int)Math.Ceiling(toCol - hC);
                        break;
                    case 'L':
                        toCol = (int)Math.Floor(toCol - hC);
                        break;
                }
            }

            return toRow * 8 + toCol;
        }

        public override object Result2()
        {
            var ids = Input.AsListOf<string>().Select(GetId)
                .OrderBy(i => i)
                .ToList();

            for (int i = 0; i < ids.Count -1; i++)
            {
                if (Math.Abs(ids[i] - ids[i + 1]) == 2)
                {
                    return (ids[i] + ids[i + 1]) / 2;
                }
            }

            return -1;
        }
    }
}