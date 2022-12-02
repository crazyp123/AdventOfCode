using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2022;

public class Day01 : Day
{
    private List<List<int>> _groups;

    public override object Result1()
    {
        _groups = new List<List<int>>();
        var g = new List<int>();
        foreach (var i in Input.AsListOf<string>())
        {
            if (string.IsNullOrWhiteSpace(i))
            {
                _groups.Add(g);
                g = new List<int>();
                continue;
            }

            var num = int.Parse(i);
            g.Add(num);
        }

        return _groups.Select(l => l.Sum()).Max();
    }

    public override object Result2()
    {
        return _groups.Select(l => l.Sum()).OrderByDescending(i => i).Take(3).Sum();
    }
}