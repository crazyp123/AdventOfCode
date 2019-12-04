using System;
using System.Linq;
using AoC.Utils;

namespace AoC.y2019
{
    public class Day4 : Day
    {
        private int _min = 138307;
        private int _max = 654504;

        public override object Result1()
        {
            return Count(num => NumbersIncrease(num) && num.FindAdjacentItems().Any(t => t.Item2 > 1));
        }

        public override object Result2()
        {
            return Count(s => NumbersIncrease(s) && s.FindAdjacentItems().Any(t => t.Item2 == 2));
        }

        bool NumbersIncrease(string num)
        {
            var prev = int.Parse(num[0].ToString());
            for (var i = 1; i < num.Length; i++)
            {
                var n = int.Parse(num[i].ToString());
                if (prev > n) return false;

                prev = n;
            }
            return true;
        }

        private int Count(Func<string, bool> validate)
        {
            int count = 0;
            var x = _min;

            while (x != _max)
            {
                if (validate(x.ToString()))
                    count++;

                x++;
            }
            return count;
        }
    }
}