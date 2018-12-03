using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day1
    {
        protected List<int> Ints = new List<int> { 0 };

        public Day1(List<int> input)
        {
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
                var z = Ints.Last() + num;

                if (Ints.Contains(z))
                {
                    Console.WriteLine($"Day 1 (2/2) Answer is: {z}");
                    return true;
                }

                Ints.Add(z);
            }
            return false;
        }
    }
}