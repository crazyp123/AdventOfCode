using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2024;

public class Day02 : Day
{
    private readonly List<List<int>> _reports;

    public Day02()
    {
        var test = @"7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9";
        _reports = Input.AsListOfLists<int>();
    }

    public override object Result1()
    {
        return _reports.Count(IsSafe);
    }

    public override object Result2()
    {
        return _reports.Count(r =>
        {
            return IsSafe(r) || r.Select((i, x) =>
            {
                var copy = r.ToList();
                copy.RemoveAt(x);
                return IsSafe(copy);
            }).Any(b => b);
        });
    }

    private bool IsSafe(List<int> r)
    {
        var diffs = r.Zip(r.Skip(1), (x, next) => next - x).ToList();
        return diffs.All(diff => Math.Abs(diff) <= 3 && diff < 0) ||
               diffs.All(diff => Math.Abs(diff) <= 3 && diff > 0);
    }
}