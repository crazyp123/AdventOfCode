using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2018
{
    public class Day1 : Day
    {
        private readonly List<int> _ints = new List<int> { 0 };
        private List<int> input;

        public Day1()
        {
            input = Input.AsListOf<int>();
        }

        public override object Result1()
        {
            return input.Sum();
        }

        public override object Result2()
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
            return answer;
        }
    }
}