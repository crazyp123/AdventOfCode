using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2022;

public class Day01 : Day
{
    private List<List<int>> _groups;

    public Day01()
    {
        _groups = Input.AsListOfGroups<int>();
    }

    public override object Result1()
    {
        return _groups.Select(l => l.Sum()).Max();
    }

    public override object Result2()
    {
        return _groups.Select(l => l.Sum()).OrderByDescending(i => i).Take(3).Sum();
    }
}