using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2024;

public class Day25 : Day
{
    private List<int[]> _keys;
    private List<int[]> _locks;

    public Day25()
    {
        _keys = new List<int[]>();
        _locks = new List<int[]>();

        var schematics = Input.Split("\n\n", StringSplitOptions.TrimEntries);
        foreach (var schematic in schematics)
            if (schematic.StartsWith("."))
                _keys.Add(Parse(schematic));
            else
                _locks.Add(Parse(schematic));
    }

    private int[] Parse(string s)
    {
        var results = new int[5];
        var strings = s.AsListOf<string>();

        for (var i = 0; i < results.Length; i++) results[i] = strings.Count(s => s[i] == '#');

        return results;
    }

    private bool Test(int[] key, int[] _lock)
    {
        return key.Zip(_lock, (k, l) => k + l).All(r => r <= 7);
    }

    public override object Result1()
    {
        var count = 0;

        foreach (var key in _keys)
        foreach (var @lock in _locks)
            if (Test(key, @lock))
                count++;

        return count;
    }

    public override object Result2()
    {
        throw new NotImplementedException();
    }
}