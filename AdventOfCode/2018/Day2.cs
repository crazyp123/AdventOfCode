using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018
{
    public class Day2
    {
        private List<string> _input;

        public Day2()
        {
            _input = Utils.GetInput(2018, 2).AsListOf<string>();

            Part1();
            Part2();
        }

        private void Part1()
        {
            var twos = 0;
            var threes = 0;

            foreach (var s in _input)
            {
                var duplicates = s.GroupBy(c => c).Select(c => c.Count()).ToList();

                if (duplicates.Contains(2)) twos++;
                if (duplicates.Contains(3)) threes++;
            }

            Utils.Answer(2,1, twos * threes);
        }

        void Part2()
        {
            var lenght = _input.First().Length;

            foreach (var s in _input)
            {
                foreach (var s2 in _input)
                {
                    var common = s.Where((c, i) => s2[i] == c).ToArray();
                    if (common.Length == lenght - 1)
                    {
                        Utils.Answer(2,2, new string(common));
                        return;
                    }
                }
            }
        }
    }
}