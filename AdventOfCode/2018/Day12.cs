using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AdventOfCode._2018
{
    public class Day12
    {
        protected internal List<Rule> Rules;
        protected internal string InitialState;

        private int originalLength = 0;

        private string test = @"initial state: #..#.#..##......###...###

...## => #
..#.. => #
.#... => #
.#.#. => #
.#.## => #
.##.. => #
.#### => #
#.#.# => #
#.### => #
##.#. => #
##.## => #
###.. => #
###.# => #
####. => #";

        protected internal int sides = 100000;

        public Day12()
        {

            var strings = Utils.GetInput(2018, 12).AsListOf<string>();

            var trim = strings[0].Replace("initial state:", "").Trim();

            originalLength = trim.Length;

            InitialState = new string('.', sides) + trim + new string('.', sides);

            strings.RemoveAt(0);

            Rules = strings.Select(s => new Rule(s)).ToList();

            Part1();

     //       Part2();
        }

        void Part1()
        {
            var sum = Run(20);

            Utils.Answer(12, 1, sum);
        }

        void Part2()
        {
            var sum = Run(50000000000);

            Utils.Answer(12, 2, sum);
        }

        private long Run(long generations)
        {
            var currentGen = InitialState.ToList();
            var next = InitialState.ToList();

            var p = 0f;

            for (long gen = 0; gen < generations; gen++)
            {
                for (int i = 0; i < currentGen.Count; i++)
                {
                    var current = currentGen[i];

                    var l1 = i >= 2 ? currentGen[i - 2] : '.';
                    var l2 = i >= 1 ? currentGen[i - 1] : '.';
                    var r1 = i < currentGen.Count - 1 ? currentGen[i + 1] : '.';
                    var r2 = i < currentGen.Count - 2 ? currentGen[i + 2] : '.';

                    var rule = Rules.FirstOrDefault(r => r.Compatible(l1, l2, current, r1, r2));
                    if (rule != null)
                    {
                        next[i] = rule.Result;
                    }
                    else
                    {
                        next[i] = '.';
                    }
                }

              //  Console.WriteLine(Sum(currentGen));

                currentGen = next.ToList();
                var x = (float)gen / generations;
                if (x - p >= 0.01f)
                {
               //    Console.WriteLine(x.ToString("P"));
                    p = x;

                }

                //  if (currentGen.All(c => c=='.')) break;
            }

            var sum = Sum(currentGen);

            return sum;
        }

        private  long Sum(List<char> currentGen)
        {
            long sum = 0;
            for (int i = 0; i < currentGen.Count; i++)
            {
                var n = i - sides;

                if (currentGen[i-1] == '#') Console.WriteLine($"{n}: {sum += n}");
            }

            return sum;
        }


        public class Rule
        {
            public char Result { get; }

            private string _schema;

            public Rule(string schema)
            {
                var split = schema.Replace(" ", "").Split("=>".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);

                _schema = split[0].Trim();
                Result = split[1].Trim()[0];
            }

            public bool Compatible(char l1, char l2, char current, char r1, char r2)
            {
                return _schema[0] == l1 && _schema[1] == l2 && _schema[2] == current && _schema[3] == r1 &&
                       _schema[4] == r2;
            }
        }
    }
}