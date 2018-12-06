using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2018
{
    public class Day3
    {
        private List<Claim> _claims;

        public Day3()
        {
            var input = Utils.GetInput(2018, 3).AsListOf<string>();

            var regex = new Regex(@"([0-9]\d*)");

            _claims = input.Select(s =>
                {
                    var matches = regex.Matches(s);
                    return new Claim
                    {
                        id = matches[0].Value,
                        pos = new Tuple<int, int>(int.Parse(matches[1].Value), int.Parse(matches[2].Value)),
                        size = new Tuple<int, int>(int.Parse(matches[3].Value), int.Parse(matches[4].Value))
                    };
                }
            ).ToList();

            Part1();
            Part2();

        }

        void Part1()
        {
            var overlaps = 0;
            var fabric = new int[1000, 1000];

            foreach (var claim in _claims)
            {
                for (int w = 0; w < claim.size.Item1; w++)
                {
                    for (int h = 0; h < claim.size.Item2; h++)
                    {
                        var x = claim.pos.Item1 + w;
                        var y = claim.pos.Item2 + h;

                        if (fabric[x, y] == 1)
                        {
                            overlaps++;
                        }
                        fabric[x, y]++;
                    }
                }
            }

            Utils.Answer(3,1, overlaps);
        }

        void Part2()
        {
            var fabric = new int[1000, 1000];

            foreach (var claim in _claims)
            {
                for (int w = 0; w < claim.size.Item1; w++)
                {
                    for (int h = 0; h < claim.size.Item2; h++)
                    {
                        var x = claim.pos.Item1 + w;
                        var y = claim.pos.Item2 + h;
                        fabric[x, y]++;
                    }
                }
            }

            foreach (var claim in _claims)
            {
                bool intact = true;

                for (int w = 0; w < claim.size.Item1; w++)
                {
                    for (int h = 0; h < claim.size.Item2; h++)
                    {
                        var x = claim.pos.Item1 + w;
                        var y = claim.pos.Item2 + h;
                        if (fabric[x, y] > 1)
                        {
                            intact = false;
                            break;
                        }

                    }
                    if (!intact) break;
                }

                if (intact)
                {
                   Utils.Answer(3,2, claim.id);
                   return;
                }
            }
        }
    }

    public class Claim
    {
        public string id;
        public Tuple<int, int> pos;
        public Tuple<int, int> size;

    }
}