using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AoC.Utils;

namespace AoC.y2021;

public class Day06 : Day
{
    private List<int> _fish;

    public Day06()
    {
        var input = "3,4,3,1,2";

        _fish = Input.AsListOf<int>(",", StringSplitOptions.RemoveEmptyEntries);
    }

    private long FishBirths(int days, List<int> initialFish)
    {
        // count num of fish on each day
        var tracker = Enumerable.Range(0, 9).Select(i => (long)initialFish.Count(x => x == i)).ToArray();

        for (int day = 0; day < days; day++)
        {
            tracker[(day + 7) % 9] += tracker[day % 9];
        }

        return tracker.Sum();
    }

    public override object Result1()
    {
        return FishBirths(80, _fish);
    }

    public override object Result2()
    {
        return FishBirths(256, _fish);
    }
}