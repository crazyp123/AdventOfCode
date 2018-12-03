using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day1
    {
        protected  List<int> Ints = new List<int> { 0 };

        public Day1(List<int> input)
        {
            while (!Day1Part2(input))
            {
                Console.WriteLine("...still crunching!");
            }
        }

        private bool Day1Part2(List<int> input)
        {
            foreach (var num in input)
            {
                var z = Ints.Last() + num;

                if (Ints.Contains(z))
                {
                    Console.WriteLine($"Duplicate is {z}");
                    return true;
                }

                Ints.Add(z);
            }
            return false;
        }
    }
}