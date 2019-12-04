using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2018
{
    public class Day1
    {
        private readonly List<int> _ints = new List<int> { 0 };

        public Day1()
        {
            var input = Utils.Utils.GetInput(2018, 1).AsListOf<int>();

            Part1(input);
            Part2(input);
        }

        private void Part1(List<int> input)
        {
            Utils.Utils.Answer(1, 1, input.Sum());
        }

        private void Part2(List<int> input)
        {
            var answer = 0;
            var found = false;
            while (!found)
            {
                foreach (var num in input)
                {
                    var z = _ints.Last() + num;

                    if (_ints.Contains(z))
                    {
                        answer = z;
                        found = true;
                    }

                    _ints.Add(z);
                }
            }
            Utils.Utils.Answer(1, 2, answer);
        }
    }
}