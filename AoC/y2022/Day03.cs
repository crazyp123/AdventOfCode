using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2022;

public class Day03 : Day
{
    private List<(string, string)> _rucksacks;

    public override object Result1()
    {
        _rucksacks = Input.AsListOf<string>().Select(s => (s[..(s.Length / 2)], s[(s.Length / 2)..])).ToList();
        return _rucksacks.Select(t => t.Item1.Intersect(t.Item2).First().PositionInAlphabet()).Sum();
    }

    public override object Result2()
    {
        return Input.AsListOf<string>()
            .SplitInEqualGroups(3)
            .Select(group =>
                group[0].Intersect(group[1]).Intersect(group[2]).First().PositionInAlphabet())
            .Sum();
    }
}