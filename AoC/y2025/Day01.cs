using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2025;

public class Day01 : Day
{
    private List<int> _intrs;

    public Day01()
    {
        var test = "L68\nL30\nR48\nL5\nR60\nL55\nL1\nL99\nR14\nL82";
        _intrs = Input.AsListOf<string>()
            .Select(s => (s[0].ToString() == "L" ? -1 : 1, int.Parse(s[1..])))
            .Select(t => t.Item1 * t.Item2)
            .ToList();
    }

    private int NextNum(int r)
    {
        if (r > 99) return r % 100;
        if (r < 0) return (100 - r * -1 % 100) % 100;
        return r;
    }

    public override object Result1()
    {
        var start = 50;
        return _intrs.Count(x =>
        {
            start = NextNum(start + x);
            return start == 0;
        });
    }

    public override object Result2()
    {
        var start = 50;
        return _intrs.Sum(x =>
        {
            var y = start + x;
            var next = NextNum(y);
            var clicks = y switch
            {
                >= 100 => y / 100,
                < 0 => -y / 100 + (start == 0 ? 0 : 1),
                _ => next == 0 ? 1 : 0
            };
            start = next;
            return clicks;
        });
    }
}