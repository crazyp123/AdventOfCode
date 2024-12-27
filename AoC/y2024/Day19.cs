using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;
using Memoizer;

namespace AoC.y2024;

public class Day19 : Day
{
    private List<string> _stock;
    private List<string> _designs;

    public Day19()
    {
        var parts = Input.Split("\n\n");
        _stock = parts[0].AsListOf<string>(",", StringSplitOptions.TrimEntries);
        _designs = parts[1].AsListOf<string>("\n");
    }

    public override object Result1()
    {
        return _designs.Count(CanCreate);
    }

    [Cache]
    public bool CanCreate(string design)
    {
        if (design.Length == 0) return true;
        var parts = _stock.Where(design.StartsWith).ToList();
        return parts.Count != 0 && parts.Any(s => CanCreate(design.Substring(s.Length)));
    }

    public override object Result2()
    {
        return _designs.Sum(CanCreateOptions);
    }

    [Cache]
    public long CanCreateOptions(string design)
    {
        if (design.Length == 0) return 1;
        var parts = _stock.Where(design.StartsWith).ToList();
        return parts.Count == 0 ? 0 : parts.Sum(s => CanCreateOptions(design.Substring(s.Length)));
    }
}