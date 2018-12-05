using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode._2018
{
    public class Day5
    {
        protected internal string Input;

        public Day5()
        {
            Input = Utils.GetInput(2018, 5).Trim();

            Part1();
            Part2();
        }


        void Part1()
        {
            var reacted = Reduce(Input);
            Console.WriteLine($"Day 5 (1/2) Answer is: {reacted.Length}");
        }

        void Part2()
        {
            var alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var results = new List<int>();

            foreach (var x in alpha)
            {
                var clean = Clean(Input, x.ToString());
                var reacted = Reduce(clean);
                results.Add(reacted.Length);
            }

            Console.WriteLine($"Day 5 (2/2) Answer is: {results.Min()}");
        }

        string Clean(string input, string x)
        {
            return input.Replace(x.ToLower(), "").Replace(x.ToUpper(), "");
        }

        string Reduce(string value)
        {
            for (int i = 0; i < value.Length-1; i++)
            {
                if (IsOpposite(value[i], value[i + 1]))
                {
                    value = value.Remove(i, 2);
                    i = Math.Max(i - 2, -1);
                }
            }
            return value;
        }

        public bool IsOpposite(char x, char y)
        {
            var z = Char.IsUpper(x);
            var w = Char.IsLower(y);
            var same = Char.ToLower(x) == Char.ToLower(y);

            return same && ((z && w) || (!z && !w));
        }

    }
}