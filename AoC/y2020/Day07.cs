using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2020
{
    public class Day07 : Day
    {
        private Dictionary<string, List<(int, string)>> _bags;

        public Day07()
        {
            _bags = Input.AsListOf<string>()
                .Select(s =>
                    s.Replace(".", "").Replace("bags", "").Replace("bag", "")
                        .Split("contain"))
                .ToDictionary(strings => strings[0].Trim(),
                    strings => strings[1]
                        .Split(",")
                        .Select(s =>
                        {
                            var element = s.Trim();
                            var hasN = int.TryParse(element.Substring(0, 1), out var n);
                            return (hasN ? n : 0, hasN ? element.Substring(1).Trim() : "");
                        })
                        .Where(tuple => tuple.Item1 != 0)
                        .ToList()
                );

        }

        public override object Result1()
        {
            var bagNames = _bags.Keys.ToList();
            return bagNames.Count(bag => GetBagsRec(bag).Any(t => t.Item2 == "shiny gold"));
        }
        public override object Result2()
        {
            return GetBagCountRec("shiny gold");
        }

        List<(int, string)> GetBagsRec(string name)
        {
            var result = new List<(int, string)>();
            result.AddRange(_bags[name]);
            result.AddRange(_bags[name].SelectMany(tuple => GetBagsRec(tuple.Item2)));
            return result;
        }

        int GetBagCountRec(string name)
        {
            return _bags[name].Sum(t => t.Item1 + t.Item1 * GetBagCountRec(t.Item2));
        }


    }
}
