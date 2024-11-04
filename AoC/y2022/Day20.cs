using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2022
{
    public class Day20 : Day
    {
        private List<int> _numbers;

        public Day20()
        {
            // test input
            var input = @"1
2
-3
3
-2
0
4";

            //   input = Input;

            _numbers = input.AsListOf<int>();

        }

        public override object Result1()
        {
            var j = 0;
            for (int i = 0; i < _numbers.Count; i++)
            {
                j += Move(_numbers, j);
                Console.WriteLine(string.Join(", ", _numbers));

            }

            var count = _numbers.Count;
            return new[] { _numbers[1000 % count], _numbers[2000 % count], _numbers[3000 % count] }.Sum();
        }

        int Move(List<int> list, int itemIx)
        {
            var value = list[itemIx];

            var insertAt = value + 1 % list.Count;
            if (value < 0)
            {
                insertAt = value + itemIx + list.Count;
            }

            list.Insert(insertAt, value);

            var shift = 1;

            if (insertAt >= itemIx)
            {
                list.RemoveAt(itemIx);
                //  shift--;
            }
            else
            {
                list.RemoveAt(itemIx + 1);
                shift++;
            }

            return shift;
        }

        public override object Result2()
        {
            throw new System.NotImplementedException();
        }
    }
}