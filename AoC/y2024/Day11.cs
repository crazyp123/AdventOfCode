using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;
using Memoizer;

namespace AoC.y2024;

public class Day11 : Day
{
    private List<long> _nums;

    public Day11()
    {
        _nums = Input.AsListOf<long>(" ");
    }

    public override object Result1()
    {
        return Stones(25, _nums);
    }

    public override object Result2()
    {
        return Stones(75, _nums);
    }

    private long Stones(int blinks, List<long> numbers)
    {
        return numbers.Sum(n => CountStonesRec(n, blinks));
    }

    [Cache]
    private long CountStonesRec(long number, int depth)
    {
        if (depth == 0) return 1;

        if (number == 0) return CountStonesRec(1, depth - 1);
        var s = number.ToString();
        if (s.Length % 2 == 0)
        {
            var left = s[..(s.Length / 2)].AsLong();
            var right = s[(s.Length / 2)..].AsLong();
            return CountStonesRec(left, depth - 1) + CountStonesRec(right, depth - 1);
        }

        return CountStonesRec(number * 2024, depth - 1);
    }
}