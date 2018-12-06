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
            var reacted = Reduce(Input.ToList());
            Utils.Answer(5,1,reacted);
        }

        void Part2()
        {
            var results = new List<int>();
            for (var x = 'a'; x <= 'z'; x++)
            {
                var clean = Clean(Input, x);
                var reacted = Reduce(clean);
                results.Add(reacted);
            }

            Utils.Answer(5,2,results.Min());
        }

        List<char> Clean(string input, char x)
        {
            return input.Where(c => c != char.ToLower(x) && c != char.ToUpper(x)).ToList();
        }

        int Reduce(List<char> x)
        {
            for (int i = 0; i < x.Count-1; i++)
            {
                if (IsOpposite(x[i], x[i + 1]))
                {
                    x.RemoveRange(i, 2);
                    i = Math.Max(i - 2, -1);
                }
            }
            return x.Count;
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