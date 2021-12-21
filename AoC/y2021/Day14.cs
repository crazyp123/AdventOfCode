using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoC.Utils;
using Newtonsoft.Json.Serialization;

namespace AoC.y2021
{
    public class Day14 : Day
    {
        private string _polymer;
        private Dictionary<string, string[]> _rules;

        public Day14()
        {
            // test input
            var input =
                "NNCB\n\nCH -> B\nHH -> N\nCB -> H\nNH -> C\nHB -> C\nHC -> B\nHN -> C\nNN -> C\nBH -> H\nNC -> B\nNB -> B\nBN -> B\nBB -> N\nBC -> B\nCC -> N\nCN -> C";

             input = Input;

            var list = input.AsListOf<string>();
            _polymer = list[0];
            _rules = list.Skip(2).Select(s => s.ParsePattern<string, string>("s -> s"))
                .Select(t => (t.Item1, new[] { $"{t.Item1[0]}{t.Item2}", $"{t.Item2}{t.Item1[1]}" }))
                .ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
        }

        public override object Result1()
        {
            return RunSteps(10);
        }

        public override object Result2()
        {
            return RunSteps(40);
        }

        private object RunSteps(int steps)
        {
            var pairs = BuildPolyDict(_polymer);

            for (var i = 0; i < steps; i++)
            {
                var copyPairs = new Dictionary<string, long>(pairs);

                foreach (var pair in copyPairs)
                {
                    pairs[pair.Key] -= pair.Value;

                    if (!_rules.ContainsKey(pair.Key)) continue;

                    var inserts = _rules[pair.Key];
                    pairs.TryAdd(inserts[0], 0);
                    pairs.TryAdd(inserts[1], 0);
                    pairs[inserts[0]] += pair.Value;
                    pairs[inserts[1]] += pair.Value;
                }
            }

            var dict = new Dictionary<char, long>();

            foreach (var pair in pairs)
            {
                var key = pair.Key[0];
                if (dict.ContainsKey(key))
                {
                    dict[key] += pair.Value;
                }
                else
                {
                    dict.Add(key, pair.Value);
                }
            }

            dict[_polymer.Last()]++;

            var max = dict.Max(c => c.Value);
            var min = dict.Min(c => c.Value);

            return max - min;
        }

        private static Dictionary<string, long> BuildPolyDict(string sb)
        {
            var polyDict = new Dictionary<string, long>();

            for (int i = 0; i < sb.Length - 1; i++)
            {
                var key = new string(new[] { sb[i], sb[i + 1] });
                if (polyDict.ContainsKey(key))
                {
                    polyDict[key]++;
                }
                else
                {
                    polyDict.Add(key, 1);
                }
            }
            return polyDict;
        }
    }
}