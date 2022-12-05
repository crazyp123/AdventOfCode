using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2022;

public class Day04 : Day
{
    private List<(int, int, int, int)> _data;

    public override object Result1()
    {
        _data = Input.AsListOfPatterns<int, int, int, int>("n-n,n-n");

        return _data.Count(t => DoOverlap(t.Item1, t.Item2, t.Item3, t.Item4));
    }


    public override object Result2()
    {
        return _data.Count(t => DoOverlap(t.Item1, t.Item2, t.Item3, t.Item4, false));
    }
    static bool DoOverlap(int a, int b, int x, int y, bool fullOverlap = true)
    {
        var first = Enumerable.Range(a, b - a + 1).ToList();
        var second = Enumerable.Range(x, y - x + 1).ToList();

        var overlaps = first.Intersect(second).Count();
        return fullOverlap ? overlaps == first.Count || overlaps == second.Count : overlaps > 0;
    }
}