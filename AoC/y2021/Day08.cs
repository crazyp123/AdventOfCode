using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2021;

public class Day08 : Day
{
    private List<(List<string>, List<string>)> _list;

    public Day08()
    {
        // testInput
        var input = "";

        input = Input;

        _list = input.AsListOf<string>().Select(line =>
        {
            var split = line.Split("|");

            return (split[0].AsListOf<string>(" ", StringSplitOptions.RemoveEmptyEntries),
                split[1].AsListOf<string>(" ", StringSplitOptions.RemoveEmptyEntries));
        }).ToList();
    }

    public override object Result1()
    {
        var y = new[] { 2, 4, 3, 7 };
        return _list.Sum(tuple => tuple.Item2.Count(s => y.Contains(s.Length)));
    }

    public override object Result2()
    {
        throw new System.NotImplementedException();
    }
}