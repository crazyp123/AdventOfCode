using System;
using System.ComponentModel;
using System.Linq;
using AoC.Utils;

namespace AoC.y2021;

public class Day05 : Day
{
    private int _highPoints;
    private int _allPoints;

    public Day05()
    {
        var test = @"0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2";

        var input = Input;

        var lines = input.AsListOf<string>("\n", StringSplitOptions.RemoveEmptyEntries).Select(s =>
        {
            var coords = s.Split("->", StringSplitOptions.TrimEntries).Select(s => s.AsListOf<int>(",")).ToList();
            var a = new Point(coords[0][0], coords[0][1]);
            var b = new Point(coords[1][0], coords[1][1]);
            return new Line(a, b);
        }).ToList();

       _highPoints = lines.Where(line => !line.IsDiagonal()).SelectMany(line => line.GetPoints()).GroupBy(p => p)
           .Count(g => g.Count() > 1);

       _allPoints = lines.SelectMany(line => line.GetPoints()).GroupBy(p => p)
           .Count(g => g.Count() > 1);
    }

    public override object Result1()
    {
        return _highPoints;
    }

    public override object Result2()
    {
        return _allPoints;
    }//27052 too high
}