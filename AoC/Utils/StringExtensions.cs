using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Utils;

public static class StringExtensions
{
    public static string Sort(this string value)
    {
        return new string(value.OrderBy(c => c).ToArray());
    }

    public static string SortDesc(this string value)
    {
        return new string(value.OrderByDescending(c => c).ToArray());
    }

    public static string Shuffle(this string value, List<string> exclusions)
    {
        var list = value.ToList();
        list.Shuffle();
        return new string(list.ToArray());
    }

    public static List<T> AsListOf<T>(this string i, string separator = "\n",
        StringSplitOptions options = StringSplitOptions.None)
    {
        return i.Split(separator, options).Select(s => (T)Convert.ChangeType(s, typeof(T))).ToList();
    }

    public static List<int> AsListOfNumbers(this string i)
    {
        return i.Select(c => (int)char.GetNumericValue(c)).ToList();
    }
}