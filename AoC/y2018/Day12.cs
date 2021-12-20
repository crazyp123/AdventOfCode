using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2018
{
    public class Day12: Day
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

        protected internal int sides = 500;

        public Day12()
        {

            var strings = Input.AsListOf<string>();

            var trim = strings[0].Replace("initial state:", "").Trim();

            originalLength = trim.Length;

            InitialState = new string('.', sides) + trim + new string('.', sides);

            strings.RemoveAt(0);

            Rules = strings.Select(s => new Rule(s)).ToList();

        }

        public override object Result1()
        {
            return Run(20);
        }

        public override object Result2()
        {
            var sum2 = Run(96);

            long sum = sum2 + (50000000000 - 96) * 32;
            return sum;
        }

        private int Run(int generations)
        {
            var currentGen = InitialState.ToList();
            var next = InitialState.ToList();

            var p = 0f;

            var prevSum = 0;

            for (var gen = 0; gen < generations; gen++)
            {
                for (int i = 0; i < currentGen.Count; i++)
                {
                    var current = currentGen[i];

                    var l1 = i >= 2 ? currentGen[i - 2] : '.';
                    var l2 = i >= 1 ? currentGen[i - 1] : '.';
                    var r1 = i < currentGen.Count - 1 ? currentGen[i + 1] : '.';
                    var r2 = i < currentGen.Count - 2 ? currentGen[i + 2] : '.';

                    var rule = Rules.First(r => r.Compatible(l1, l2, current, r1, r2));
                    next[i] = rule.Result;
                }

                currentGen = next.ToList();

                var s = Sum(currentGen);

                //   Console.WriteLine($"diff={prevSum - s} \n {gen}: {s} \n\n");

                prevSum = s;
            }

            var sum = Sum(currentGen);

            return sum;
        }

        private int Sum(List<char> currentGen)
        {
            var sum = 0;
            for (int i = 0; i < currentGen.Count; i++)
            {
                var n = i - sides;

                if (currentGen[i] == '#')
                {
                    sum += n;
                }
            }

            return sum;
        }


        public class Rule
        {
            public char Result { get; }

            private string _schema;

            public Rule(string schema)
            {
                var split = schema.Replace(" ", "").Split("=>".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

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