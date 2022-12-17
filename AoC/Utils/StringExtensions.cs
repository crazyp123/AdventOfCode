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
        return i.Split(separator, options).Select(ConvertTo<T>).ToList();
    }

    public static T ConvertTo<T>(string s)
    {
        try
        {
            return (T)Convert.ChangeType(s, typeof(T));
        }
        catch
        {
            return default;
        }
    }

    public static List<int> AsListOfNumbers(this string i)
    {
        return i.Select(c => (int)char.GetNumericValue(c)).ToList();
    }

    public static List<List<T>> AsListOfGroups<T>(this string i, string separator = "")
    {
        var x = i.AsListOf<string>()
            .SplitBy((s, i1) => s.Equals(separator));

        return x.Select(group => group.Select(ConvertTo<T>).ToList())
        .ToList();
    }

    public static int AsInt(this string i)
    {
        return int.Parse(i);
    }

    public static long AsLong(this string i)
    {
        return long.Parse(i);
    }

    public static ulong AsULong(this string i)
    {
        return ulong.Parse(i);
    }

    public static int AsInt(this char i)
    {
        return i.ToString().AsInt();
    }
}