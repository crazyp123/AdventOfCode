using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018
{
    public class Day1
    {
        private readonly List<int> _ints = new List<int> { 0 };

        public Day1()
        {
            var input = Utils.GetInput(2018, 1).AsListOf<int>();

            Part1(input);

            while (!Part2(input))
            {
            }
        }

        private void Part1(List<int> input)
        {
            Console.WriteLine($"Day 1 (1/2) Answer is: {input.Sum()}");
        }

        private bool Part2(List<int> input)
        {
            foreach (var num in input)
            {
                var z = _ints.Last() + num;

                if (_ints.Contains(z))
                {
                    Console.WriteLine($"Day 1 (2/2) Answer is: {z}");
                    return true;
                }

                _ints.Add(z);
            }
            return false;
        }
    }
}