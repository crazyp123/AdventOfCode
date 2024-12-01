using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2024;

public class Day01 : Day
{
    private readonly List<int> _left;
    private readonly List<int> _right;

    public Day01()
    {
        var list = Input.AsListOfPatterns<int, int>("n   n");
        _left = list.Select(x => x.Item1).Order()
            .ToList();

        _right = list.Select(x => x.Item2).Order().ToList();
    }

    public override object Result1()
    {
        return _left.Zip(_right, (l, r) => Math.Abs(l - r)).Sum();
    }

    public override object Result2()
    {
        var rightCounts = _right.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
        return _left.Sum(l => l * rightCounts.GetValueOrDefault(l, 0));
    }
}