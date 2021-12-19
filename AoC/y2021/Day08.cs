using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Objects;
using AoC.Utils;

namespace AoC.y2021;

public partial class Day08 : Day
{
    private List<(List<string>, List<string>)> _list;
    private List<string> _permutations;

    public Day08()
    {
        // testInput
        var input = "";

        input = Input;

        _list = input.AsListOf<string>().Select(line =>
        {
            var split = line.Split("|");

            return (split[0].AsListOf<string>(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => new string(s.OrderBy(c => c).ToArray())).ToList(),
                split[1].AsListOf<string>(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => new string(s.OrderBy(c => c).ToArray())).ToList());
        }).ToList();
    }

    public override object Result1()
    {
        var y = new[] { 2, 4, 3, 7 };
        return _list.Sum(tuple => tuple.Item2.Count(s => y.Contains(s.Length)));
    }

    public override object Result2()
    {
        _permutations = "abcdefg".Permutations().Select(c => new string(c)).ToList();
        var sum = 0L;
        foreach (var (patterns, inputs) in _list)
        {
            var ss = GetSegmentMap(patterns);
            var num = string.Concat(inputs.Select(x => ss.GetNumFromCode(x)));
            sum += long.Parse(num);
        }
        return sum;
    }

    SevenSegmentNum GetSegmentMap(List<string> input)
    {
        var ss = new SevenSegmentNum();

        foreach (var permutation in _permutations)
        {
            ss.Map = permutation;
            if (input.All(s => ss.GetNumFromCode(s) != -1))
            {
                return ss;
            }
        }

        return null;
    }
}